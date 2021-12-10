using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class TestNegateParserTest
    {
        [Theory]
        [InlineData("-1")]
        [InlineData(" -1")]
        [InlineData("-1 ")]
        [InlineData(" -1 ")]
        public void Test_Atomic(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new NegateToken(
                    new AtomicToken(1)), reply.Result);
        }
    }
}