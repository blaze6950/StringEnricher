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

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string text = "test";
        const string url = "https://example.com";
        var node = InlineLinkMarkdownV2.Apply(text, url);
        Span<char> destination = stackalloc char[50];
        const string expected = "[test](https://example.com)";

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        const string text = "test";
        const string url = "https://example.com";
        var node = InlineLinkMarkdownV2.Apply(text, url);
        Span<char> destination = stackalloc char[10];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_EmptyDestination_ReturnsFalse()
    {
        // Arrange
        const string text = "text";
        const string url = "https://example.com";
        var node = InlineLinkMarkdownV2.Apply(text, url);
        Span<char> destination = Span<char>.Empty;

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryFormat_ExactSizeDestination_ReturnsTrue()
    {
        // Arrange
        const string text = "test";
        const string url = "https://example.com";
        var node = InlineLinkMarkdownV2.Apply(text, url);
        const string expected = "[test](https://example.com)";
        Span<char> destination = stackalloc char[expected.Length];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void ToString_WithFormatAndProvider_ReturnsCorrectString()
    {
        // Arrange
        const int value = 123;
        const string url = "https://example.com";
        var node = InlineLinkMarkdownV2.Apply(value, url);
        const string expected = "[123](https://example.com)";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}