using System.Collections.Generic;
using Core.Extensions;
using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class ClassParserTest
    {
        [Theory]
        [InlineData("class Foo() extends Bar() { }")]
        public void Test_Extends_EmptyFormals_EmptyActuals( string text)
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
        [InlineData("class Foo() { }")]
        public void Test_ExtendsNone_EmptyFormals( string text)
        {
            // Act
            var reply = Parser.Class().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new ClassToken("Foo", new Formals(new List<Formal>().AsValueSemantics()), "object",
                    new Tokens(new List<Token>().AsValueSemantics()), new Tokens(new List<Token>().AsValueSemantics()))
                , reply.Result);
        }
        
        [Theory]
        [InlineData("class Foo(baz: Int) extends Bar(baz) { }")]
        public void Test_One_Formal( string text)
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