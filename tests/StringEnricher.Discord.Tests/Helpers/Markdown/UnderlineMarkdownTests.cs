using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers.Markdown;

public class UnderlineMarkdownTests
{
    [Fact]
    public void Apply_WithPlainText_ReturnsUnderline()
    {
        // Arrange
        const string expected = "__underlined__";

        // Act
        var result = UnderlineMarkdown.Apply("underlined").ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithBool_ReturnsUnderline()
    {
        // Arrange
        const string expected = "__True__";

        // Act
        var result = UnderlineMarkdown.Apply(true).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var underlineNode = UnderlineMarkdown.Apply("text");
        const string expected = "__text__";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var ok = underlineNode.TryGetChar(i, out var ch);
            Assert.True(ok);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(8)] // "__text__" length is 8
    public void TryGetChar_OutOfRange_ReturnsFalse(int index)
    {
        // Arrange
        var underlineNode = UnderlineMarkdown.Apply("text");

        // Act
        var ok = underlineNode.TryGetChar(index, out var ch);

        // Assert
        Assert.False(ok);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void SyntaxLength_IsCorrect()
    {
        // Arrange & Act
        var underlineNode = UnderlineMarkdown.Apply("content");

        // Assert
        Assert.Equal(4, underlineNode.SyntaxLength); // "__" + "__"
    }
}
