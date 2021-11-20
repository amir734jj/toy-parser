using System.Collections.Generic;
using System.Collections.Immutable;
using Core.Extensions;
using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class FormalsTest
    {
        private readonly Parser _parser;

        public FormalsTest()
        {
            _parser = new Parser();
        }
        
        [Fact]
        public void Test__Formals_Empty()
        {
            // Arrange
            const string text = "()";

            // Act
            var reply = _parser.Formals().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new Formals(new List<Formal>().AsValueSemantics()), reply.Result);
        }
        
        [Fact]
        public void Test__Formals_One()
        {
            // Arrange
            const string text = "(foo: Bar)";

            // Act
            var reply = _parser.Formals().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new Formals(new List<Formal> { new Formal("foo", "Bar") }.AsValueSemantics()),
                reply.Result);
        }
        
        [Fact]
        public void Test__Formals_Many()
        {
            // Arrange
            const string text = "(foo: Bar, baz: Qux)";

            // Act
            var reply = _parser.Formals().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new Formals(new List<Formal> { new Formal("foo", "Bar"), new Formal("baz", "Qux") }
                    .AsValueSemantics()), reply.Result);
        }
    }
}