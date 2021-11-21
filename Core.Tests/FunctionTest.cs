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
        public void Test_Function_Empty( string text)
        {
            // Act
            var reply = Parser.Function().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new FunctionDeclToken("foo", new Formals(new List<Formal>().AsValueSemantics()),
                    new VariableToken("bar")), reply.Result);
        }
        
        [Theory]
        [InlineData("def foo(baz:Qux)=bar")]
        [InlineData(" def foo(baz:Qux)=bar")]
        [InlineData("def foo(baz:Qux)=bar ")]
        [InlineData(" def foo(baz:Qux)=bar ")]
        [InlineData("def foo(baz : Qux) = bar")]
        [InlineData(" def foo(baz : Qux) = bar")]
        [InlineData("def foo(baz : Qux) = bar ")]
        [InlineData(" def foo(baz : Qux) = bar ")]
        public void Test_Function_One( string text)
        {
            // Act
            var reply = Parser.Function().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new FunctionDeclToken("foo",
                    new Formals(new List<Formal> { new Formal("baz", "Qux") }.AsValueSemantics()),
                    new VariableToken("bar")), reply.Result);
        }
        
        [Theory]
        [InlineData("def foo(baz:Qux,taz:Sux)=bar")]
        [InlineData(" def foo(baz:Qux,taz:Sux)=bar")]
        [InlineData("def foo(baz:Qux,taz:Sux)=bar ")]
        [InlineData(" def foo(baz:Qux,taz:Sux)=bar ")]
        [InlineData("def foo(baz : Qux , taz : Sux) = bar")]
        [InlineData(" def foo(baz : Qux , taz : Sux) = bar")]
        [InlineData("def foo(baz : Qux , taz : Sux) = bar ")]
        [InlineData(" def foo(baz : Qux , taz : Sux) = bar ")]
        public void Test_Function_Many( string text)
        {
            // Act
            var reply = Parser.Function().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new FunctionDeclToken("foo",
                    new Formals(
                        new List<Formal> { new Formal("baz", "Qux"), new Formal("taz", "Sux") }.AsValueSemantics()),
                    new VariableToken("bar")), reply.Result);
        }
    }
}