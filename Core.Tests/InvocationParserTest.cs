using System.Collections.Generic;
using System.Collections.Immutable;
using Core.Extensions;
using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class InvocationParserTest
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
        public void Test_Invocation_Empty(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new FunctionCallToken(new VariableToken("foo"),
                    new Tokens(ImmutableList<Token>.Empty.AsValueSemantics())), reply.Result);
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
        public void Test_Invocation_One(string text)
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
        public void Test_Invocation_Many(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new FunctionCallToken(new VariableToken("foo"),
                    new Tokens(new List<Token> { new VariableToken("bar"), new VariableToken("baz") }
                        .AsValueSemantics())), reply.Result);
        }
    }
}