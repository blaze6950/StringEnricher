using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2.StringStyles;

public class TgEmojiStyleTests
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
}