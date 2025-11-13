using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers.Markdown;

public class CodeBlockMarkdownTests
{
    [Fact]
    public void Apply_WithPlainText_ReturnsCodeBlock()
    {
        // Arrange
        const string expected = "```code block```";

        // Act
        var result = CodeBlockMarkdown.Apply("code block").ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithBool_ReturnsCodeBlock()
    {
        // Arrange
        const string expected = "```True```";

        // Act
        var result = CodeBlockMarkdown.Apply(true).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var node = CodeBlockMarkdown.Apply("test");
        const string expected = "```test```";

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
    [InlineData(10)] // "```test```" length is 10
    public void TryGetChar_OutOfRange_ReturnsFalse(int index)
    {
        // Arrange
        var node = CodeBlockMarkdown.Apply("test");

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
        var node = CodeBlockMarkdown.Apply("content");

        // Assert
        Assert.Equal(6, node.SyntaxLength); // "```" + "```"
    }

    [Fact]
    public void TotalLength_IsCorrect()
    {
        // Arrange & Act
        var node = CodeBlockMarkdown.Apply("hi");

        // Assert
        Assert.Equal(8, node.TotalLength); // "```hi```"
    }
}
