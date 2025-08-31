using StringEnricher.Nodes.MarkdownV2;
using StringEnricher.Nodes.MarkdownV2.Formatting;

namespace StringEnricher.Tests.MarkdownV2.StringStyles;

public class TgEmojiNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedTgEmoji = "![👍](tg://emoji?id=5368324170671202286)";

        // Act
        var styledTgEmoji = TgEmojiMarkdownV2.Apply("👍", "5368324170671202286").ToString();

        // Assert
        Assert.NotNull(styledTgEmoji);
        Assert.NotEmpty(styledTgEmoji);
        Assert.Equal(expectedTgEmoji, styledTgEmoji);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var tgEmoji = TgEmojiMarkdownV2.Apply("👍", "12345");
        const string expected = "![👍](tg://emoji?id=12345)";

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
    [InlineData(26)] // "[👍](tg://emoji?id=12345)" length is 24
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var tgEmoji = TgEmojiMarkdownV2.Apply("👍", "12345");

        // Act
        var result = tgEmoji.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}