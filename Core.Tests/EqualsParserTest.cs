using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class EqualsParserTest
    {
        [Theory]
        [InlineData("foo==bar")]
        [InlineData(" foo==bar")]
        [InlineData("foo==bar ")]
        [InlineData(" foo==bar ")]
        [InlineData("foo == bar")]
        [InlineData(" foo == bar")]
        [InlineData("foo == bar ")]
        [InlineData(" foo == bar ")]
        public void Test_Equals(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new EqualsToken(new VariableToken("foo"), new VariableToken("bar")), reply.Result);
        }
    }
}