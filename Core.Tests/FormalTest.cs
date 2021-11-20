using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class FormalTest
    {
        [Fact]
        public void Test__Formal()
        {
            // Arrange
            const string text = "foo: Bar";

            // Act
            var reply = Parser.Formal().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new Formal("foo", "Bar"), reply.Result);
        }
    }
}