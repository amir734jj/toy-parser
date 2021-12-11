using System.Collections.Generic;
using Core.Extensions;
using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class MatchParserTest
    {
        [Theory]
        [InlineData("match foo with { case null => null }")]
        public void Test_Atomic(string text)
        {
            // Act
            var reply = Parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new Match(new VariableToken("foo"), new List<ArmToken>
            {
                new ArmToken("null", "Any", new AtomicToken("null"))
            }.AsValueSemantics()), reply.Result);
        }
    }
}