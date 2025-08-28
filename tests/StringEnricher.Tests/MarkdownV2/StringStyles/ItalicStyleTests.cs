using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2.StringStyles;

public class ItalicStyleTests
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
}

