using System.Collections.Generic;
using Core.Extensions;
using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class ClassesParserTest
    {
        [Theory]
        [InlineData("class Foo() extends native { } class Bar() extends native { }")]
        public void Test_Natives( string text)
        {
            // Act
            var reply = Parser.Classes().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new Classes(new List<ClassToken>
            {
                new ClassToken("Foo", new Formals(new List<Formal>().AsValueSemantics()), "native",
                    new Tokens(new List<Token>().AsValueSemantics()), new Tokens(new List<Token>().AsValueSemantics())),
                new ClassToken("Bar", new Formals(new List<Formal>().AsValueSemantics()), "native",
                    new Tokens(new List<Token>().AsValueSemantics()), new Tokens(new List<Token>().AsValueSemantics()))
            }.AsValueSemantics()), reply.Result);
        }
    }
}