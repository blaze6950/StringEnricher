using StringEnricher.Nodes.Html;
using StringEnricher.Nodes.Html.Formatting;

namespace StringEnricher.Tests.Html.StringStyles;

public class TgEmojiNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedTgEmoji = "<tg-emoji emoji-id=\"5368324170671202286\">👍</tg-emoji>";

        // Act
        var styledTgEmoji = TgEmojiHtml.Apply("👍", "5368324170671202286").ToString();

        // Assert
        Assert.NotNull(styledTgEmoji);
        Assert.NotEmpty(styledTgEmoji);
        Assert.Equal(expectedTgEmoji, styledTgEmoji);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var tgEmoji = TgEmojiHtml.Apply("👍", "12345");
        const string expected = "<tg-emoji emoji-id=\"12345\">👍</tg-emoji>";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = tgEmoji.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(42)] // "<tg-emoji emoji-id=\"12345\">👍</tg-emoji>" length is 41
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var tgEmoji = TgEmojiHtml.Apply("👍", "12345");

        // Act
        var result = tgEmoji.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}