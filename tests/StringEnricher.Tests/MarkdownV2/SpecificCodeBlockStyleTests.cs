using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2;

public class SpecificCodeBlockStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedSpecificCodeBlock = "```csharp\ncode block\n```";

        // Act
        var styledSpecificCodeBlock = SpecificCodeBlockMarkdownV2.Apply("code block", "csharp").ToString();

        // Assert
        Assert.NotNull(styledSpecificCodeBlock);
        Assert.NotEmpty(styledSpecificCodeBlock);
        Assert.Equal(expectedSpecificCodeBlock, styledSpecificCodeBlock);
    }
}