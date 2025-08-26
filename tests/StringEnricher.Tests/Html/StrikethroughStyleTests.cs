using StringEnricher.StringStyles.Html;

namespace StringEnricher.Tests.Html;

public class StrikethroughStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedStrikethrough = "<s>strikethrough</s>";

        // Act
        var styledStrikethrough = StrikethroughHtml.Apply("strikethrough").ToString();

        // Assert
        Assert.NotNull(styledStrikethrough);
        Assert.NotEmpty(styledStrikethrough);
        Assert.Equal(expectedStrikethrough, styledStrikethrough);
    }
}

