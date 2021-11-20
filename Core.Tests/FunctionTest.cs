using System.Collections.Immutable;
using Core.Extensions;
using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class FunctionTest
    {
        private readonly Parser _parser;

        public FunctionTest()
        {
            _parser = new Parser();
        }
        
        [Fact]
        public void Test__Function()
        {
            // Arrange
            const string text = "";

            // Act
            var reply = _parser.Formals().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new Formals(new[] { new Formal("foo", "Bar"), new Formal("baz", "Qux") }
                    .AsValueSemantics()), reply.Result);
        }
    }
}