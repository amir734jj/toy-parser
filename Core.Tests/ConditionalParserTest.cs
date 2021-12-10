using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class ConditionalParserTest
    {
        [Theory]
        [InlineData("if(foo)bar else(baz)")]
        [InlineData(" if(foo)bar else(baz)")]
        [InlineData("if(foo)bar else(baz) ")]
        [InlineData(" if(foo)bar else(baz) ")]
        public void Test_Conditional(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new CondToken(
                    new VariableToken("foo"),
                    new VariableToken("bar"),
                    new VariableToken("baz")),
                reply.Result);
        }
    }
}