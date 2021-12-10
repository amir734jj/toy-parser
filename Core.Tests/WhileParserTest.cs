using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class WhileParserTest
    {
        [Theory]
        [InlineData("while(foo) bar")]
        [InlineData(" while(foo) bar")]
        [InlineData("while(foo) bar ")]
        [InlineData(" while(foo) bar ")]
        public void Test_Loop(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new WhileToken(
                    new VariableToken("foo"),
                    new VariableToken("bar")),
                reply.Result);
        }
    }
}