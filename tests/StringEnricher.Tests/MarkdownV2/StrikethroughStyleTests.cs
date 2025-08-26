using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2;

public class StrikethroughStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedStrikethrough = "~strikethrough~";

        // Act
        var styledStrikethrough = StrikethroughMarkdownV2.Apply("strikethrough").ToString();

        // Assert
        Assert.NotNull(styledStrikethrough);
        Assert.NotEmpty(styledStrikethrough);
        Assert.Equal(expectedStrikethrough, styledStrikethrough);
    }
}

