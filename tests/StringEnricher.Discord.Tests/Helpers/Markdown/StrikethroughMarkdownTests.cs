using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers.Markdown;

public class StrikethroughMarkdownTests
{
    [Fact]
    public void Apply_WithPlainText_ReturnsStrikethrough()
    {
        // Arrange
        const string expected = "~~strikethrough~~";

        // Act
        var result = StrikethroughMarkdown.Apply("strikethrough").ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithBool_ReturnsStrikethrough()
    {
        // Arrange
        const string expected = "~~False~~";

        // Act
        var result = StrikethroughMarkdown.Apply(false).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var node = StrikethroughMarkdown.Apply("text");
        const string expected = "~~text~~";

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
    [InlineData(8)] // "~~text~~" length is 8
    public void TryGetChar_OutOfRange_ReturnsFalse(int index)
    {
        // Arrange
        var node = StrikethroughMarkdown.Apply("text");

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
        var node = StrikethroughMarkdown.Apply("content");

        // Assert
        Assert.Equal(4, node.SyntaxLength); // "~~" + "~~"
    }
}
