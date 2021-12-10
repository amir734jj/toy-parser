using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class TestNotParserTest
    {
        [Theory]
        [InlineData("!x")]
        [InlineData(" !x")]
        [InlineData("!x ")]
        [InlineData(" !x ")]
        public void Test_Atomic(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new NotToken(
                    new VariableToken("x")), reply.Result);
        }
    }
}