using System.Collections.Generic;
using Core.Extensions;
using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class CommentsParserTest
    {
        [Theory]
        [InlineData("/** hello \n world! **/ //hello world!")]
        [InlineData("/** hello \n world! **/ //hello world! ")]
        [InlineData(" /** hello \n world! **/ //hello world!")]
        [InlineData(" /** hello \n world! **/ //hello world! ")]
        public void Test_Multiple( string text)
        {
            // Act
            var reply = Parser.Comments().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new CommentsToken(new List<CommentToken>
            {
                new CommentToken("* hello \n world! *"),
                new CommentToken("hello world!")
            }.AsValueSemantics()), reply.Result);
        }
    }
}