using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2;

public class SpoilerStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedSpoiler = "||spoiler text||";

        // Act
        var styledSpoiler = SpoilerMarkdownV2.Apply("spoiler text").ToString();

        // Assert
        Assert.NotNull(styledSpoiler);
        Assert.NotEmpty(styledSpoiler);
        Assert.Equal(expectedSpoiler, styledSpoiler);
    }
}

