using System.Collections.Generic;
using Core.Extensions;
using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class FunctionTest
    {
        [Theory]
        [InlineData("def foo()=bar")]
        [InlineData(" def foo()=bar")]
        [InlineData("def foo()=bar ")]
        [InlineData(" def foo()=bar ")]
        [InlineData("def foo() = bar")]
        [InlineData(" def foo() = bar")]
        [InlineData("def foo() = bar ")]
        [InlineData(" def foo() = bar ")]
        public void Test_Function( string text)
        {
            // Act
            var reply = Parser.Function().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new FunctionDeclToken("foo", new Formals(new List<Formal>().AsValueSemantics()),
                    new VariableToken("bar")), reply.Result);
        }
    }
}