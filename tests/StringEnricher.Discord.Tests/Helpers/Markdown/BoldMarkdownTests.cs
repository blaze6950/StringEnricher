using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers.Markdown;

public class BoldMarkdownTests
{
    [Fact]
    public void Apply_WithPlainText_ReturnsBold()
    {
        // Arrange
        const string expected = "**bold text**";

        // Act
        var result = BoldMarkdown.Apply("bold text").ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithBool_ReturnsBold()
    {
        // Arrange
        const string expected = "**True**";

        // Act
        var result = BoldMarkdown.Apply(true).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithInteger_ReturnsBold()
    {
        // Arrange
        const string expected = "**42**";

        // Act
        var result = BoldMarkdown.Apply(42).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var boldNode = BoldMarkdown.Apply("test");
        const string expected = "**test**";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var ok = boldNode.TryGetChar(i, out var ch);
            Assert.True(ok);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(10)] // "**test**" length is 10
    public void TryGetChar_OutOfRange_ReturnsFalse(int index)
    {
        // Arrange
        var boldNode = BoldMarkdown.Apply("test");

        // Act
        var ok = boldNode.TryGetChar(index, out var ch);

        // Assert
        Assert.False(ok);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void SyntaxLength_IsCorrect()
    {
        // Arrange & Act
        var boldNode = BoldMarkdown.Apply("content");

        // Assert
        Assert.Equal(4, boldNode.SyntaxLength); // "**" + "**"
    }

    [Fact]
    public void TotalLength_IsCorrect()
    {
        // Arrange & Act
        var boldNode = BoldMarkdown.Apply("hello");

        // Assert
        Assert.Equal(9, boldNode.TotalLength); // "**hello**"
    }
}
