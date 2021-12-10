using System.Collections.Generic;
using Core.Extensions;
using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class AccessParserTest
    {
        [Theory]
        [InlineData("foo.bar")]
        [InlineData(" foo.bar")]
        [InlineData("foo.bar ")]
        [InlineData(" foo.bar ")]
        public void Test_AccessName(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new AccessToken(new VariableToken("foo"), new VariableToken("bar")), reply.Result);
        }
        
        [Theory]
        [InlineData("foo.bar()")]
        [InlineData(" foo.bar()")]
        [InlineData("foo.bar() ")]
        [InlineData(" foo.bar() ")]
        public void Test_AccessFunction(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(
                new AccessToken(new VariableToken("foo"),
                    new FunctionCallToken(new VariableToken("bar"), new Tokens(new List<Token>().AsValueSemantics()))),
                reply.Result);
        }
    }
}