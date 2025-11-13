using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Nodes.Markdown;

public class HeaderNodeTests
{
    [Fact]
    public void Test_Level1Header()
    {
        // Arrange
        const string expectedHeader = "# Header";

        // Act
        var styledHeader = HeaderMarkdown.Apply("Header", 1).ToString();

        // Assert
        Assert.NotNull(styledHeader);
        Assert.NotEmpty(styledHeader);
        Assert.Equal(expectedHeader, styledHeader);
    }

    [Fact]
    public void Test_Level2Header()
    {
        // Arrange
        const string expectedHeader = "## Subheader";

        // Act
        var styledHeader = HeaderMarkdown.Apply("Subheader", 2).ToString();

        // Assert
        Assert.NotNull(styledHeader);
        Assert.NotEmpty(styledHeader);
        Assert.Equal(expectedHeader, styledHeader);
    }

    [Fact]
    public void Test_Level3Header()
    {
        // Arrange
        const string expectedHeader = "### Smaller header";

        // Act
        var styledHeader = HeaderMarkdown.Apply("Smaller header", 3).ToString();

        // Assert
        Assert.NotNull(styledHeader);
        Assert.NotEmpty(styledHeader);
        Assert.Equal(expectedHeader, styledHeader);
    }

    [Fact]
    public void Test_DefaultLevel()
    {
        // Arrange
        const string expectedHeader = "# Default";

        // Act
        var styledHeader = HeaderMarkdown.Apply("Default").ToString();

        // Assert
        Assert.NotNull(styledHeader);
        Assert.NotEmpty(styledHeader);
        Assert.Equal(expectedHeader, styledHeader);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var header = HeaderMarkdown.Apply("test", 1);
        const string expected = "# test";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = header.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)] // "# test" length is 6
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var header = HeaderMarkdown.Apply("test", 1);

        // Act
        var result = header.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}
