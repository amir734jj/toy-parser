using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class DeclarationParserTest
    {
        [Theory]
        [InlineData("var foo: Bar = baz")]
        [InlineData(" var foo: Bar = baz")]
        [InlineData("var foo: Bar = baz ")]
        [InlineData(" var foo: Bar = baz ")]
        public void Test_Declaration(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new VarDeclToken("foo", "Bar", new VariableToken("baz")), reply.Result);
        }
    }
}