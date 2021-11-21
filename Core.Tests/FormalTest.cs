using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class FormalTest
    {
        [Theory]
        [InlineData("foo:Bar")]
        [InlineData(" foo:Bar")]
        [InlineData("foo:Bar ")]
        [InlineData(" foo:Bar ")]
        [InlineData("foo : Bar")]
        [InlineData(" foo : Bar")]
        [InlineData("foo : Bar ")]
        [InlineData(" foo : Bar ")]
        public void Test__Formal(string text)
        {
            // Act
            var reply = Parser.Formal().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new Formal("foo", "Bar"), reply.Result);
        }
    }
}