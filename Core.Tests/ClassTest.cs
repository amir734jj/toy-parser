using System.Collections.Generic;
using Core.Extensions;
using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class ClassTest
    {
        [Theory]
        [InlineData("class Foo() extends Bar() { }")]
        public void Test_Class_Empty( string text)
        {
            // Act
            var reply = Parser.Class().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new ClassToken("Foo", new Formals(new List<Formal>().AsValueSemantics()), "Bar",
                    new Tokens(new List<Token>().AsValueSemantics()), new Tokens(new List<Token>().AsValueSemantics()))
                , reply.Result);
        }
        
        [Theory]
        [InlineData("class Foo(var baz: Int) extends Bar(baz) { }")]
        public void Test_Class_One( string text)
        {
            // Act
            var reply = Parser.Class().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new ClassToken("Foo", new Formals(new List<Formal>{new Formal("baz", "Int")}.AsValueSemantics()), "Bar",
                    new Tokens(new List<Token>{new VariableToken("baz")}.AsValueSemantics()), new Tokens(new List<Token>().AsValueSemantics()))
                , reply.Result);
        }
    }
}