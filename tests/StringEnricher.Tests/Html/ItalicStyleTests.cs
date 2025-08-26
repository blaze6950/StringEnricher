using StringEnricher.StringStyles.Html;

namespace StringEnricher.Tests.Html;

public class ItalicStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedItalic = "<i>italic text</i>";

        // Act
        var styledItalic = ItalicHtml.Apply("italic text").ToString();

        // Assert
        Assert.NotNull(styledItalic);
        Assert.NotEmpty(styledItalic);
        Assert.Equal(expectedItalic, styledItalic);
    }
}

