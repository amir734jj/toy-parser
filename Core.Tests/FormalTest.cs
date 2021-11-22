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
        public void Test_Formal_NoVar(string text)
        {
            // Act
            var reply = Parser.Formal(false).ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new Formal("foo", "Bar"), reply.Result);
        }
        
        [Theory]
        [InlineData("var foo:Bar")]
        [InlineData(" var foo:Bar")]
        [InlineData("var foo:Bar ")]
        [InlineData(" var foo:Bar ")]
        [InlineData("var foo : Bar")]
        [InlineData(" var foo : Bar")]
        [InlineData("var foo : Bar ")]
        [InlineData(" var foo : Bar ")]
        public void Test_Formal_WithVar(string text)
        {
            // Act
            var reply = Parser.Formal(true).ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new Formal("foo", "Bar"), reply.Result);
        }
    }
}