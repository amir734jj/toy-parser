using FParsec.CSharp;
using Xunit;

namespace Core.Tests
{
    public class AtomicParserTest
    {
        private readonly Parser _parser;

        public AtomicParserTest()
        {
            _parser = new Parser();
        }

        [Theory]
        [InlineData("null")]
        [InlineData(" null")]
        [InlineData("null ")]
        [InlineData(" null ")]
        public void Test_Null(string text)
        {
            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new AtomicToken(null), reply.Result);
        }

        [Theory]
        [InlineData("true")]
        [InlineData(" true")]
        [InlineData("true ")]
        [InlineData(" true ")]
        public void Test_Boolean_True(string text)
        {
            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new AtomicToken(true), reply.Result);
        }

        [Theory]
        [InlineData("false")]
        [InlineData(" false")]
        [InlineData("false ")]
        [InlineData(" false ")]
        public void Test_Boolean_False(string text)
        {
            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new AtomicToken(false), reply.Result);
        }

        [Theory]
        [InlineData("123456789")]
        [InlineData(" 123456789")]
        [InlineData("123456789 ")]
        [InlineData(" 123456789 ")]
        public void Test__Number(string text)
        {
            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new AtomicToken(123456789), reply.Result);
        }

        [Theory]
        [InlineData(@"""Hello world!""")]
        [InlineData(@" ""Hello world!""")]
        [InlineData(@"""Hello world!"" ")]
        [InlineData(@" ""Hello world!"" ")]
        public void Test__Atomic_String(string text)
        {
            // Act
            var reply = _parser.Expression().ParseString(text);

            // Assert
            Assert.True(reply.IsOk());
            Assert.Equal(new AtomicToken("Hello world!"), reply.Result);
        }
    }
}