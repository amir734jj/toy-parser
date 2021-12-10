using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class VariableParserTest
    {
        [Theory]
        [InlineData("foo")]
        [InlineData(" foo")]
        [InlineData("foo ")]
        [InlineData(" foo ")]
        public void Test_Variable(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new VariableToken("foo"), reply.Result);
        }
    }
}