using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers.Markdown;

public class MultilineQuoteMarkdownTests
{
    [Fact]
    public void Apply_WithPlainText_ReturnsMultilineQuote()
    {
        // Arrange
        const string expected = ">>> multi-line quote";

        // Act
        var result = MultilineQuoteMarkdown.Apply("multi-line quote").ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithBool_ReturnsMultilineQuote()
    {
        // Arrange
        const string expected = ">>> True";

        // Act
        var result = MultilineQuoteMarkdown.Apply(true).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithMultilineText_AddsOnlyOnePrefix()
    {
        // Arrange
        const string input = "line1\nline2\nline3";
        const string expected = ">>> line1\nline2\nline3";

        // Act
        var result = MultilineQuoteMarkdown.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var node = MultilineQuoteMarkdown.Apply("test");
        const string expected = ">>> test";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var ok = node.TryGetChar(i, out var ch);
            Assert.True(ok);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(8)] // ">>> test" length is 8
    public void TryGetChar_OutOfRange_ReturnsFalse(int index)
    {
        // Arrange
        var node = MultilineQuoteMarkdown.Apply("test");

        // Act
        var ok = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(ok);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void SyntaxLength_IsCorrect()
    {
        // Arrange & Act
        var node = MultilineQuoteMarkdown.Apply("content");

        // Assert
        Assert.Equal(4, node.SyntaxLength); // ">>> "
    }
}
