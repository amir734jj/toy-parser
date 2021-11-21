using System.Collections.Generic;
using Core.Extensions;
using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class FormalsTest
    {
        [Theory]
        [InlineData("()")]
        [InlineData(" ()")]
        [InlineData("() ")]
        [InlineData(" () ")]
        [InlineData("( )")]
        [InlineData(" ( )")]
        [InlineData("( ) ")]
        [InlineData(" ( ) ")]
        public void Test_Empty(string text)
        {
            // Act
            var reply = Parser.Formals().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new Formals(new List<Formal>().AsValueSemantics()), reply.Result);
        }
        
        [Theory]
        [InlineData("(foo:Bar)")]
        [InlineData(" (foo:Bar)")]
        [InlineData("(foo:Bar) ")]
        [InlineData(" (foo:Bar) ")]
        [InlineData("(foo : Bar)")]
        [InlineData(" (foo : Bar)")]
        [InlineData("(foo : Bar) ")]
        [InlineData(" (foo : Bar) ")]
        public void Test_One(string text)
        {
            // Act
            var reply = Parser.Formals().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new Formals(new List<Formal> { new Formal("foo", "Bar") }.AsValueSemantics()),
                reply.Result);
        }
        
        [Theory]
        [InlineData("(foo:Bar,baz:Qux)")]
        [InlineData(" (foo:Bar,baz:Qux)")]
        [InlineData("(foo:Bar,baz:Qux) ")]
        [InlineData(" (foo:Bar,baz:Qux) ")]
        [InlineData("(foo : Bar , baz : Qux)")]
        [InlineData(" (foo : Bar,baz : Qux)")]
        [InlineData("(foo : Bar,baz : Qux) ")]
        [InlineData(" (foo : Bar,baz : Qux) ")]
        public void Test_Many(string text)
        {
            // Act
            var reply = Parser.Formals().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new Formals(new List<Formal> { new Formal("foo", "Bar"), new Formal("baz", "Qux") }
                    .AsValueSemantics()), reply.Result);
        }
    }
}
