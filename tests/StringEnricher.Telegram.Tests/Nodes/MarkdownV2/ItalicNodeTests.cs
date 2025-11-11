using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Nodes.MarkdownV2;

public class ItalicNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedItalic = "_italic text_";

        // Act
        var styledItalic = ItalicMarkdownV2.Apply("italic text").ToString();

        // Assert
        Assert.NotNull(styledItalic);
        Assert.NotEmpty(styledItalic);
        Assert.Equal(expectedItalic, styledItalic);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var italic = ItalicMarkdownV2.Apply("test");
        const string expected = "_test_";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = italic.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)] // "_test_" length is 6
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var italic = ItalicMarkdownV2.Apply("test");

        // Act
        var result = italic.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}
