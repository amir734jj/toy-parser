using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class FormalTest
    {
        private readonly Parser _parser;

        public FormalTest()
        {
            _parser = new Parser();
        }
        
        [Fact]
        public void Test__Formal()
        {
            // Arrange
            const string text = "foo: Bar";

            // Act
            var reply = _parser.Formal().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new Formal("foo", "Bar"), reply.Result);
        }
    }
}