using Core.Extensions;
using Core.Interfaces;
using FParsec;
using FParsec.CSharp;
using Microsoft.FSharp.Core;
using static FParsec.CSharp.PrimitivesCS; // combinator functions
using static FParsec.CSharp.CharParsersCS; // pre-defined parsers

namespace Core
{
    public static class Parser
    {
        /// <summary>
        /// Any type of variable name
        /// </summary>
        public static FSharpFunc<CharStream<Unit>, Reply<string>> Name()
        {
            var nameP = Many1Chars(NoneOf(new[] { ':', '"', ' ', '{', '}', '=', '(', ')', '\n', ';', ',', '*' }))
                .Label("name");

            return nameP;
        }

        /// <summary>
        /// Expression are:
        ///   1) Assignment: [Variable] = [Expression]
        ///   2) Declaration: var [Variable] = [Expression] 
        ///   3) Block: { [Expression]* }
        ///   4) Function call: [Expression] ( [Expression]* )
        ///   5) Conditional: if ( [Expression] ) [Expression] else [Expression]
        ///   6) Binary: [Expression] [+-/*] [Expression]
        ///   7) Unary: [-!] [Expression]
        ///   7) Variable: [Name]
        /// </summary>
        public static FSharpFunc<CharStream<Unit>, Reply<Token>> Expression()
        {
            // [Atomic] = Number | Boolean | Null | String
            FSharpFunc<CharStream<Unit>, Reply<Token>> Atomic(
                FSharpFunc<CharStream<Unit>, Reply<Token>> expressionRec)
            {
                var stringP = Wrap('"', ManyChars(NoneOf(new[] { '"' })), '"').Label("string")
                    .Map(x => (Token)new AtomicToken(x));
                var numberP = Int.Lbl("number")
                    .Map(x => (Token)new AtomicToken(x));
                var boolP = StringP("true").Or(StringP("false")).Lbl("bool")
                    .Map(x => (Token)new AtomicToken(x == "true"));
                var nullP = StringP("null").Map(_ => (Token)new AtomicToken(null));

                var atomicP = Choice(nullP, numberP, stringP, boolP).Label("atomic");

                return SkipWs(atomicP);
            }

            static FSharpFunc<CharStream<Unit>, Reply<Token>> Declaration(
                FSharpFunc<CharStream<Unit>, Reply<Token>> expressionRec)
            {
                // Declaration
                var declarationP = Skip("var").AndTry_(WS1).AndRTry(Name()).AndLTry(WS).AndLTry(CharP(':'))
                    .AndLTry(WS).AndTry(Name()).AndLTry(WS).AndLTry(CharP('=')).AndLTry(WS)
                    .AndTry(expressionRec)
                    .Label("decl")
                    .Map(x => (Token)new VarDeclToken(x.Item1.Item1, x.Item1.Item2, x.Item2));

                return SkipWs(declarationP);
            }

            static FSharpFunc<CharStream<Unit>, Reply<Token>> Assignment(
                FSharpFunc<CharStream<Unit>, Reply<Token>> expressionRec)
            {
                // Assignment
                var assignmentP = Name().AndLTry(WS).AndLTry(CharP('=')).AndLTry(WS).AndTry(expressionRec)
                    .Label("assign")
                    .Map(x => (Token)new AssignToken(x.Item1, x.Item2));

                return SkipWs(assignmentP);
            }

            static FSharpFunc<CharStream<Unit>, Reply<Token>> Conditional(
                FSharpFunc<CharStream<Unit>, Reply<Token>> expressionRec)
            {
                // Conditional
                var conditionalP = Skip("if").And_(WS)
                    .AndRTry(Wrap('(', expressionRec, ')'))
                    .AndLTry(WS)
                    .AndTry(expressionRec).AndTry(WS).AndLTry(StringP("else")).AndLTry(WS).AndTry(expressionRec)
                    .Label("cond")
                    .Map(x =>
                        (Token)new CondToken(x.Item1.Item1, x.Item1.Item2, x.Item2));

                return SkipWs(conditionalP);
            }

            static FSharpFunc<CharStream<Unit>, Reply<Token>> Block(
                FSharpFunc<CharStream<Unit>, Reply<Token>> expressionRec)
            {
                var blockExprP = SepBy('{', expressionRec, '}', Skip(';')).Label("block")
                    .Map(x => (Token)new BlockToken(new Tokens(x)));

                return SkipWs(blockExprP);
            }

            static FSharpFunc<CharStream<Unit>, Reply<Token>> Variable(
                FSharpFunc<CharStream<Unit>, Reply<Token>> expressionRec)
            {
                // Variable
                var variableP = Name()
                    .Map(x => (Token)new VariableToken(x));

                return SkipWs(variableP);
            }

            static FSharpFunc<CharStream<Unit>, Reply<Token>> FunctionCall(
                FSharpFunc<CharStream<Unit>, Reply<Token>> expressionRec)
            {
                // Function call
                var functionCallP = Variable(expressionRec)
                    .AndTry(SepBy('(', expressionRec, ')', Skip(',')))
                    .Map(x => (Token)new FunctionCallToken(((VariableToken)(x.Item1)).Variable, new Tokens(x.Item2)));

                return SkipWs(functionCallP);
            }

            // Binary and unary
            var expressionP = new OPPBuilder<Unit, Token, Unit>()
                .WithOperators(ops => ops
                    .AddPrefix("-", 5, WS, token => new NegateToken(token))
                    .AddPrefix("!", 5, WS, token => new NotToken(token))
                    .AddInfix("+", 10, WS, (x, y) => new AddToken(x, y))
                    .AddInfix("-", 10, WS, (x, y) => new SubtractToken(x, y))
                    .AddInfix("*", 20, WS, (x, y) => new MultiplyToken(x, y))
                    .AddInfix("/", 20, WS, (x, y) => new DivideToken(x, y)))
                .WithTerms(term => Choice(
                        Declaration(term),
                        Assignment(term),
                        Block(term),
                        Conditional(term),
                        Atomic(term),
                        FunctionCall(term),
                        Variable(term),
                        Wrap('(', term, ')')
                    )
                )
                .Build()
                .ExpressionParser
                .Label("operator");

            return SkipWs(expressionP);
        }

        /// <summary>
        /// Function has the following form:
        ///   def [Variable]() = [Expression]
        /// </summary>
        public static FSharpFunc<CharStream<Unit>, Reply<FunctionDeclToken>> Function()
        {
            var functionDeclP = Skip("def").AndTry_(WS1).AndRTry(Name()).AndLTry(WS).AndTry(Formals())
                .AndLTry(CharP('='))
                .AndTry(Expression())
                .Map(x => new FunctionDeclToken(
                    x.Item1.Item1,
                    x.Item1.Item2,
                    x.Item2));

            return SkipWs(functionDeclP);
        }

        public static FSharpFunc<CharStream<Unit>, Reply<Token>> Feature()
        {
            var featureP = Choice(Function())
                .Map(x => (Token)x);

            return SkipWs(featureP);
        }

        public static FSharpFunc<CharStream<Unit>, Reply<Tokens>> Features()
        {
            var featuresP = Many(Feature())
                .Map(x => new Tokens(x.AsValueSemantics()));

            return SkipWs(featuresP);
        }

        /// <summary>
        /// Formal are argument to functions to argument to class constructors
        /// </summary>
        /// <returns></returns>
        public static FSharpFunc<CharStream<Unit>, Reply<Formal>> Formal()
        {
            var argumentP = Name().AndLTry(WS).AndLTry(CharP(':')).AndLTry(WS).AndTry(Name())
                .Map(x => new Formal(x.Item1, x.Item2));

            return SkipWs(argumentP);
        }

        /// <summary>
        /// Formals are list of formals separated by ','
        /// </summary>
        public static FSharpFunc<CharStream<Unit>, Reply<Formals>> Formals()
        {
            var formalsP = SepBy('(', Formal(), ')', Skip(','))
                .Map(x => new Formals(x));

            return SkipWs(formalsP);
        }

        /// <summary>
        /// Class has the following form:
        ///   class [Variable] ( [Expression]* ) extends [Variable] ( [Expression]* ) { [Feature]* }
        /// </summary>
        public static FSharpFunc<CharStream<Unit>, Reply<ClassToken>> Class()
        {
            var classSignatureP = Name().AndLTry(WS).AndTry(Formals());

            var classP = StringP("class").AndTry_(WS).AndRTry(classSignatureP).AndLTry(WS)
                .AndLTry(StringP("extends"))
                .AndTry(classSignatureP)
                .AndTry(SepBy('{', Feature(), '}', Skip(';')))
                .Map(x => new ClassToken(
                    x.Item1.Item1.Item1,
                    x.Item1.Item1.Item2,
                    x.Item1.Item2.Item1,
                    x.Item1.Item2.Item2,
                    new Tokens(x.Item2)
                ));

            return SkipWs(classP);
        }
        
        private static FSharpFunc<CharStream<Unit>, Reply<T>> Wrap<T>(
            char start,
            FSharpFunc<CharStream<Unit>, Reply<T>> p,
            char end)
        {
            var wrapP = Between(SkipWs(CharP(start)), SkipWs(p), SkipWs(CharP(end)));
            return wrapP;
        }

        private static FSharpFunc<CharStream<Unit>, Reply<IValueCollection<T>>> SepBy<T>(
            char start,
            FSharpFunc<CharStream<Unit>, Reply<T>> p,
            char end,
            FSharpFunc<CharStream<Unit>, Reply<Unit>> delimiterP)
        {
            var arrItems = Many(SkipWs(p), sep: delimiterP, canEndWithSep: false);
            var arrayP = Wrap(start, arrItems, end)
                .Map(elems => elems.AsValueSemantics());
            return arrayP;
        }

        private static FSharpFunc<CharStream<Unit>, Reply<T>> SkipWs<T>(FSharpFunc<CharStream<Unit>, Reply<T>> p)
        {
            return Skip(WS).AndTry(p).AndL(WS);
        }
    }
}