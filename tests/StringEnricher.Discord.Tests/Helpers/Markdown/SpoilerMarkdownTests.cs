using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers.Markdown;

public class SpoilerMarkdownTests
{
    [Fact]
    public void Apply_WithPlainText_ReturnsSpoiler()
    {
        // Arrange
        const string expected = "||spoiler text||";

        // Act
        var result = SpoilerMarkdown.Apply("spoiler text").ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithBool_ReturnsSpoiler()
    {
        // Arrange
        const string expected = "||True||";

        // Act
        var result = SpoilerMarkdown.Apply(true).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var node = SpoilerMarkdown.Apply("test");
        const string expected = "||test||";

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
    [InlineData(8)] // "||test||" length is 8
    public void TryGetChar_OutOfRange_ReturnsFalse(int index)
    {
        // Arrange
        var node = SpoilerMarkdown.Apply("test");

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
        var node = SpoilerMarkdown.Apply("content");

        // Assert
        Assert.Equal(4, node.SyntaxLength); // "||" + "||"
    }
}
