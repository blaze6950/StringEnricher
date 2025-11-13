using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers.Markdown;

public class BlockquoteMarkdownTests
{
    [Fact]
    public void Apply_WithPlainText_ReturnsBlockquote()
    {
        // Arrange
        const string expected = "> single line quote";

        // Act
        var result = BlockquoteMarkdown.Apply("single line quote").ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithBool_ReturnsBlockquote()
    {
        // Arrange
        const string expected = "> True";

        // Act
        var result = BlockquoteMarkdown.Apply(true).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var blockQuote = BlockquoteMarkdown.Apply("test quote");
        const string expected = "> test quote";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var ok = blockQuote.TryGetChar(i, out var ch);
            Assert.True(ok);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(12)] // "> test quote" length is 12
    public void TryGetChar_OutOfRange_ReturnsFalse(int index)
    {
        // Arrange
        var blockQuote = BlockquoteMarkdown.Apply("test quote");

        // Act
        var ok = blockQuote.TryGetChar(index, out var ch);

        // Assert
        Assert.False(ok);
        Assert.Equal('\0', ch);
    }
}
