using System.Collections.Generic;
using System.Collections.Immutable;
using Core.Extensions;
using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class ExpressionTest
    {
        private readonly Parser _parser;

        public ExpressionTest()
        {
            _parser = new Parser();
        }

        [Fact]
        public void Test__Invocation_Empty()
        {
            // Arrange
            const string text = "foo()";

            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new FunctionCallToken(new VariableToken("foo"), new Tokens(ImmutableList<Token>.Empty.AsValueSemantics())), reply.Result);
        }
        
        [Fact]
        public void Test__Invocation_One()
        {
            // Arrange
            const string text = "foo(null)";

            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new FunctionCallToken(new VariableToken("foo"),
                    new Tokens(new List<Token> { new AtomicToken(null) }.AsValueSemantics())),
                reply.Result);
        }

        [Fact]
        public void Test__Invocation_Many()
        {
            // Arrange
            const string text = "foo( null, null)";

            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new FunctionCallToken(new VariableToken("foo"),
                    new Tokens(new List<Token> { new AtomicToken(null), new AtomicToken(null) }
                        .AsValueSemantics())), reply.Result);
        }

        [Fact]
        public void Test__Declaration()
        {
            // Arrange
            const string text = "var foo: Bar = null";

            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new VarDeclToken("foo", "Bar", new AtomicToken(null)), reply.Result);
        }

        [Fact]
        public void Test__Assignment()
        {
            // Arrange
            const string text = "foo = null";

            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new AssignToken("foo", new AtomicToken(null)), reply.Result);
        }

        [Fact]
        public void Test__Variable()
        {
            // Arrange
            const string text = "foo";

            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new VariableToken("foo"), reply.Result);
        }

        [Fact]
        public void Test__Block()
        {
            // Arrange
            const string text = "{ foo }";

            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            var blockToken = Assert.IsType<BlockToken>(reply.Result);
            Assert.Collection(blockToken.Tokens.Inner, token => { Assert.Equal(new VariableToken("foo"), token); });
        }

        [Fact]
        public void Test__Operation()
        {
            // Arrange
            const string text = "1+2*3-1/-3";

            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new SubtractToken(
                    new AddToken(new AtomicToken(1), new MultiplyToken(new AtomicToken(2), new AtomicToken(3))),
                    new DivideToken(new AtomicToken(1), new NegateToken(new AtomicToken(3)))), reply.Result);
        }
        
        [Fact]
        public void Test__Operation_With_Parentheses()
        {
            // Arrange
            const string text = "((1 + 2) * ((3 - 1) / -3))";

            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new MultiplyToken(
                    new AddToken(new AtomicToken(1), new AtomicToken(2)),
                    new DivideToken(new SubtractToken(new AtomicToken(3), new AtomicToken(1)), new NegateToken(new AtomicToken(3)))), reply.Result);
        }
        
        [Fact]
        public void Test__Amir()
        {
            // Arrange
            const string text = "amir () amir";

            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new MultiplyToken(
                    new AddToken(new AtomicToken(1), new AtomicToken(2)),
                    new DivideToken(new SubtractToken(new AtomicToken(3), new AtomicToken(1)), new NegateToken(new AtomicToken(3)))), reply.Result);
        }
    }
}