using StringEnricher.Nodes.MarkdownV2.Formatting;

namespace StringEnricher.Tests.MarkdownV2.Nodes;

public class SpecificCodeBlockNodeTests
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

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var specificCodeBlock = SpecificCodeBlockMarkdownV2.Apply("test", "csharp");
        const string expected = "```csharp\ntest\n```";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = specificCodeBlock.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(18)] // "```csharp\ntest\n```" length is 18
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var specificCodeBlock = SpecificCodeBlockMarkdownV2.Apply("test", "csharp");

        // Act
        var result = specificCodeBlock.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}