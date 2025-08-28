using StringEnricher.StringStyles.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2.StringStyles;

public class UnderlineStyleTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedUnderline = "__underline__";

        // Act
        var styledUnderline = UnderlineMarkdownV2.Apply("underline").ToString();

        // Assert
        Assert.NotNull(styledUnderline);
        Assert.NotEmpty(styledUnderline);
        Assert.Equal(expectedUnderline, styledUnderline);
    }
}

