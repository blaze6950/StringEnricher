using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Helpers.Markdown;

public class ItalicMarkdownTests
{
    [Fact]
    public void Apply_WithPlainText_ReturnsItalic()
    {
        // Arrange
        const string expected = "*italic text*";

        // Act
        var result = ItalicMarkdown.Apply("italic text").ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithBool_ReturnsItalic()
    {
        // Arrange
        const string expected = "*False*";

        // Act
        var result = ItalicMarkdown.Apply(false).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var italicNode = ItalicMarkdown.Apply("test");
        const string expected = "*test*";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var ok = italicNode.TryGetChar(i, out var ch);
            Assert.True(ok);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)] // "*test*" length is 6
    public void TryGetChar_OutOfRange_ReturnsFalse(int index)
    {
        // Arrange
        var italicNode = ItalicMarkdown.Apply("test");

        // Act
        var ok = italicNode.TryGetChar(index, out var ch);

        // Assert
        Assert.False(ok);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void SyntaxLength_IsCorrect()
    {
        // Arrange & Act
        var italicNode = ItalicMarkdown.Apply("content");

        // Assert
        Assert.Equal(2, italicNode.SyntaxLength); // "*" + "*"
    }

    [Fact]
    public void TotalLength_IsCorrect()
    {
        // Arrange & Act
        var italicNode = ItalicMarkdown.Apply("hi");

        // Assert
        Assert.Equal(4, italicNode.TotalLength); // "*hi*"
    }
}
