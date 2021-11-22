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
        public void Test_NoVar_Empty(string text)
        {
            // Act
            var reply = Parser.Formals(false).ParseString(text);

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
        public void Test_NoVar_One(string text)
        {
            // Act
            var reply = Parser.Formals(false).ParseString(text);

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
        public void Test_NoVar_Many(string text)
        {
            // Act
            var reply = Parser.Formals(false).ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new Formals(new List<Formal> { new Formal("foo", "Bar"), new Formal("baz", "Qux") }
                    .AsValueSemantics()), reply.Result);
        }
        
        [Theory]
        [InlineData("()")]
        [InlineData(" ()")]
        [InlineData("() ")]
        [InlineData(" () ")]
        [InlineData("( )")]
        [InlineData(" ( )")]
        [InlineData("( ) ")]
        [InlineData(" ( ) ")]
        public void Test_WithVar_Empty(string text)
        {
            // Act
            var reply = Parser.Formals(true).ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new Formals(new List<Formal>().AsValueSemantics()), reply.Result);
        }
        
        [Theory]
        [InlineData("(var foo:Bar)")]
        [InlineData(" (var foo:Bar)")]
        [InlineData("(var foo:Bar) ")]
        [InlineData(" (var foo:Bar) ")]
        [InlineData("(var foo : Bar)")]
        [InlineData(" (var foo : Bar)")]
        [InlineData("(var foo : Bar) ")]
        [InlineData(" (var foo : Bar) ")]
        public void Test_WithVar_One(string text)
        {
            // Act
            var reply = Parser.Formals(true).ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new Formals(new List<Formal> { new Formal("foo", "Bar") }.AsValueSemantics()),
                reply.Result);
        }
        
        [Theory]
        [InlineData("(var foo:Bar,var baz:Qux)")]
        [InlineData(" (var foo:Bar,var baz:Qux)")]
        [InlineData("(var foo:Bar,var baz:Qux) ")]
        [InlineData(" (var foo:Bar,var baz:Qux) ")]
        [InlineData("(var foo : Bar , var baz : Qux)")]
        [InlineData(" (var foo : Bar,var baz : Qux)")]
        [InlineData("(var foo : Bar,var baz : Qux) ")]
        [InlineData(" (var foo : Bar,var baz : Qux) ")]
        public void Test_WithVar_Many(string text)
        {
            // Act
            var reply = Parser.Formals(true).ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new Formals(new List<Formal> { new Formal("foo", "Bar"), new Formal("baz", "Qux") }
                    .AsValueSemantics()), reply.Result);
        }
    }
}
