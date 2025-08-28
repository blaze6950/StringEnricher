using StringEnricher.StringStyles.Html;

namespace StringEnricher.Tests.Html.StringStyles;

public class SpoilerStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedSpoiler = "<tg-spoiler>spoiler text</tg-spoiler>";

        // Act
        var styledSpoiler = SpoilerHtml.Apply("spoiler text").ToString();

        // Assert
        Assert.NotNull(styledSpoiler);
        Assert.NotEmpty(styledSpoiler);
        Assert.Equal(expectedSpoiler, styledSpoiler);
    }
}

