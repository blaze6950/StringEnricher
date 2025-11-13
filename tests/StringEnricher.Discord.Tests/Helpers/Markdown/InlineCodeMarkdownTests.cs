using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers.Markdown;

public class InlineCodeMarkdownTests
{
    [Fact]
    public void Apply_WithPlainText_ReturnsInlineCode()
    {
        // Arrange
        const string expected = "`code text`";

        // Act
        var result = InlineCodeMarkdown.Apply("code text").ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithBool_ReturnsInlineCode()
    {
        // Arrange
        const string expected = "`False`";

        // Act
        var result = InlineCodeMarkdown.Apply(false).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithInteger_ReturnsInlineCode()
    {
        // Arrange
        const string expected = "`42`";

        // Act
        var result = InlineCodeMarkdown.Apply(42).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var node = InlineCodeMarkdown.Apply("test");
        const string expected = "`test`";

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
    [InlineData(6)] // "`test`" length is 6
    public void TryGetChar_OutOfRange_ReturnsFalse(int index)
    {
        // Arrange
        var node = InlineCodeMarkdown.Apply("test");

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
        var node = InlineCodeMarkdown.Apply("content");

        // Assert
        Assert.Equal(2, node.SyntaxLength); // "`" + "`"
    }
}
