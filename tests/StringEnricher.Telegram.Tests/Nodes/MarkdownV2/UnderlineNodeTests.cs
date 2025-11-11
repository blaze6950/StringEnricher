using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Nodes.MarkdownV2;

public class UnderlineNodeTests
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

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var underline = UnderlineMarkdownV2.Apply("test");
        const string expected = "__test__";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = underline.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(8)] // "__test__" length is 8
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var underline = UnderlineMarkdownV2.Apply("test");

        // Act
        var result = underline.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}
