using StringEnricher.Helpers.MarkdownV2;

namespace StringEnricher.Tests.MarkdownV2.Nodes;

public class InlineCodeMarkdownV2Tests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedInlineCode = "`inline code`";

        // Act
        var styledInlineCode = InlineCodeMarkdownV2.Apply("inline code").ToString();

        // Assert
        Assert.NotNull(styledInlineCode);
        Assert.NotEmpty(styledInlineCode);
        Assert.Equal(expectedInlineCode, styledInlineCode);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var inlineCode = InlineCodeMarkdownV2.Apply("code");
        const string expected = "`code`";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = inlineCode.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)] // "`code`" length is 6
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var inlineCode = InlineCodeMarkdownV2.Apply("code");

        // Act
        var result = inlineCode.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}
