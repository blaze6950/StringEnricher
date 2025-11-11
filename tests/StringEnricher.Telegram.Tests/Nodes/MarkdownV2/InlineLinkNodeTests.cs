using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Nodes.MarkdownV2;

public class InlineLinkNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedLink = "[test](https://example.com)";

        // Act
        var styledLink = InlineLinkMarkdownV2.Apply("test", "https://example.com").ToString();

        // Assert
        Assert.NotNull(styledLink);
        Assert.NotEmpty(styledLink);
        Assert.Equal(expectedLink, styledLink);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var inlineLink = InlineLinkMarkdownV2.Apply("test", "https://example.com");
        const string expected = "[test](https://example.com)";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = inlineLink.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(27)] // "[test](https://example.com)" length is 26
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var inlineLink = InlineLinkMarkdownV2.Apply("test", "https://example.com");

        // Act
        var result = inlineLink.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }
}