using StringEnricher.StringStyles.Html;

namespace StringEnricher.Tests.Html.StringStyles;

public class TgEmojiStyleTests
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
}