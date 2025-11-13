using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Nodes.Markdown;

public class MultilineQuoteNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedQuote = ">>> multi-line \n quote \n text";

        // Act
        var styledQuote = MultilineQuoteMarkdown.Apply("multi-line \n quote \n text").ToString();

        // Assert
        Assert.NotNull(styledQuote);
        Assert.NotEmpty(styledQuote);
        Assert.Equal(expectedQuote, styledQuote);
    }

    [Fact]
    public void Test_SingleLine()
    {
        // Arrange
        const string expectedQuote = ">>> single line";

        // Act
        var styledQuote = MultilineQuoteMarkdown.Apply("single line").ToString();

        // Assert
        Assert.NotNull(styledQuote);
        Assert.NotEmpty(styledQuote);
        Assert.Equal(expectedQuote, styledQuote);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var quote = MultilineQuoteMarkdown.Apply("test");
        const string expected = ">>> test";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = quote.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(8)] // ">>> test" length is 8
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var quote = MultilineQuoteMarkdown.Apply("test");

        // Act
        var result = quote.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}
