using System.Collections.Generic;
using System.Collections.Immutable;
using Core.Extensions;
using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class ExpressionTest
    {
        [Theory]
        [InlineData("foo()")]
        [InlineData(" foo()")]
        [InlineData("foo() ")]
        [InlineData(" foo() ")]
        [InlineData("foo( )")]
        [InlineData(" foo( )")]
        [InlineData("foo( ) ")]
        [InlineData(" foo( ) ")]
        public void Test__Invocation_Empty(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new FunctionCallToken(new VariableToken("foo"), new Tokens(ImmutableList<Token>.Empty.AsValueSemantics())), reply.Result);
        }
        
        [Theory]
        [InlineData("foo(bar)")]
        [InlineData(" foo(bar)")]
        [InlineData("foo(bar) ")]
        [InlineData(" foo(bar) ")]
        [InlineData("foo( bar )")]
        [InlineData(" foo( bar )")]
        [InlineData("foo( bar ) ")]
        [InlineData(" foo( bar ) ")]
        public void Test__Invocation_One(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new FunctionCallToken(new VariableToken("foo"),
                    new Tokens(new List<Token> { new VariableToken("bar") }.AsValueSemantics())),
                reply.Result);
        }

        [Theory]
        [InlineData("foo(bar,baz)")]
        [InlineData(" foo(bar,baz)")]
        [InlineData("foo(bar,baz) ")]
        [InlineData(" foo(bar,baz) ")]
        public void Test__Invocation_Many(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new FunctionCallToken(new VariableToken("foo"),
                    new Tokens(new List<Token> { new VariableToken("bar"), new AtomicToken("baz") }
                        .AsValueSemantics())), reply.Result);
        }

        [Theory]
        [InlineData("var foo: Bar = baz")]
        [InlineData(" var foo: Bar = baz")]
        [InlineData("var foo: Bar = baz ")]
        [InlineData(" var foo: Bar = baz ")]
        public void Test_Declaration(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new VarDeclToken("foo", "Bar", new VariableToken("baz")), reply.Result);
        }

        [Theory]
        [InlineData("foo = null")]
        [InlineData(" foo = null")]
        [InlineData("foo = null ")]
        [InlineData(" foo = null ")]
        public void Test_Assignment(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new AssignToken("foo", new AtomicToken(null)), reply.Result);
        }

        [Theory]
        [InlineData("foo")]
        [InlineData(" foo")]
        [InlineData("foo ")]
        [InlineData(" foo ")]
        public void Test__Variable(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new VariableToken("foo"), reply.Result);
        }

        [Theory]
        [InlineData("{}")]
        [InlineData(" {}")]
        [InlineData("{} ")]
        [InlineData(" {} ")]
        [InlineData("{ }")]
        [InlineData(" { }")]
        [InlineData("{ } ")]
        [InlineData(" { } ")]
        public void Test_Block_Empty(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            var blockToken = Assert.IsType<BlockToken>(reply.Result);
            Assert.Empty(blockToken.Tokens.Inner);
        }
        
        [Theory]
        [InlineData("{foo}")]
        [InlineData(" {foo}")]
        [InlineData("{foo} ")]
        [InlineData(" {foo} ")]
        [InlineData("{ foo }")]
        [InlineData(" { foo }")]
        [InlineData("{ foo } ")]
        [InlineData(" { foo } ")]
        public void Test_Block_One(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            var blockToken = Assert.IsType<BlockToken>(reply.Result);
            Assert.Collection(blockToken.Tokens.Inner, token => { Assert.Equal(new VariableToken("foo"), token); });
        }
        
        [Theory]
        [InlineData("{foo bar}")]
        [InlineData(" {foo bar}")]
        [InlineData("{foo bar} ")]
        [InlineData(" {foo bar} ")]
        [InlineData("{ foo bar }")]
        [InlineData(" { foo bar }")]
        [InlineData("{ foo bar } ")]
        [InlineData(" { foo bar } ")]
        public void Test_Block_Many(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            var blockToken = Assert.IsType<BlockToken>(reply.Result);
            Assert.Equal(
                new Tokens(new List<Token> { new VariableToken("foo"), new VariableToken("foo") }.AsValueSemantics()),
                blockToken.Tokens);
        }

        [Theory]
        [InlineData("1+2*3-1/-3")]
        public void Test__Operation(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new SubtractToken(
                    new AddToken(new AtomicToken(1), new MultiplyToken(new AtomicToken(2), new AtomicToken(3))),
                    new DivideToken(new AtomicToken(1), new NegateToken(new AtomicToken(3)))), reply.Result);
        }
        
        [Theory]
        [InlineData("((1 + 2) * ((3 - 1) / -3))")]
        public void Test__Operation_With_Parentheses(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new MultiplyToken(
                    new AddToken(new AtomicToken(1), new AtomicToken(2)),
                    new DivideToken(new SubtractToken(new AtomicToken(3), new AtomicToken(1)), new NegateToken(new AtomicToken(3)))), reply.Result);
        }
    }
}
