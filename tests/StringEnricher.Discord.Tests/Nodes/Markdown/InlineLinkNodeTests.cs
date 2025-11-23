using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Nodes.Markdown;

public class InlineLinkNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedLink = "[test](https://example.com)";

        // Act
        var styledLink = InlineLinkMarkdown.Apply("test", "https://example.com").ToString();

        // Assert
        Assert.NotNull(styledLink);
        Assert.NotEmpty(styledLink);
        Assert.Equal(expectedLink, styledLink);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var inlineLink = InlineLinkMarkdown.Apply("test", "https://example.com");
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
    [InlineData(27)] // "[test](https://example.com)" length is 27
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var inlineLink = InlineLinkMarkdown.Apply("test", "https://example.com");

        // Act
        var result = inlineLink.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TotalLength_ReturnsCorrectLength()
    {
        // Arrange
        const string text = "test";
        const string url = "https://example.com";
        var node = InlineLinkMarkdown.Apply(text, url);
        const int expectedLength = 27; // "[test](https://example.com)"

        // Act
        var actualLength = node.TotalLength;

        // Assert
        Assert.Equal(expectedLength, actualLength);
    }

    [Fact]
    public void SyntaxLength_ReturnsCorrectLength()
    {
        // Arrange
        var node = InlineLinkMarkdown.Apply("test", "https://example.com");
        const int expectedSyntaxLength = 4; // "[" + "](" + ")"

        // Act
        var actualSyntaxLength = node.SyntaxLength;

        // Assert
        Assert.Equal(expectedSyntaxLength, actualSyntaxLength);
    }

    [Fact]
    public void CopyTo_WithSufficientSpace_CopiesCorrectly()
    {
        // Arrange
        const string text = "test";
        const string url = "https://example.com";
        var node = InlineLinkMarkdown.Apply(text, url);
        Span<char> destination = stackalloc char[50];
        const string expected = "[test](https://example.com)";

        // Act
        var charsWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void ToString_WithFormatAndProvider_ReturnsCorrectString()
    {
        // Arrange
        const int value = 123;
        const string url = "https://example.com";
        var node = InlineLinkMarkdown.Apply(value, url);
        const string expected = "[123](https://example.com)";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string text = "test";
        const string url = "https://example.com";
        var node = InlineLinkMarkdown.Apply(text, url);
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
        var node = InlineLinkMarkdown.Apply("test", "https://example.com");
        Span<char> destination = stackalloc char[10]; // Too small for "[test](https://example.com)"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void ToString_WithFormatParameter_PassesFormatToTextNode()
    {
        // Arrange
        const int value = 12345;
        const string url = "https://example.com";
        var node = InlineLinkMarkdown.Apply(value, url);
        var expected = $"[{value:X}]({url})"; // Hex format

        // Act
        var result = node.ToString("X", null);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithProviderParameter_PassesProviderToTextNode()
    {
        // Arrange
        const int value = 12345;
        const string url = "https://example.com";
        var provider = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");
        var node = InlineLinkMarkdown.Apply(value, url);
        var expected = $"[{value.ToString("N0", provider)}]({url})";

        // Act
        var result = node.ToString("N0", provider);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryFormat_WithFormatParameter_PassesFormatToTextNode()
    {
        // Arrange
        const int value = 255;
        const string url = "https://example.com";
        var node = InlineLinkMarkdown.Apply(value, url);
        Span<char> destination = stackalloc char[50];
        var expected = $"[{value:X}]({url})"; // Hex format: [FF](https://example.com)

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "X".AsSpan(), null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithProviderParameter_PassesProviderToTextNode()
    {
        // Arrange
        const int value = 12345;
        const string url = "https://example.com";
        var provider = System.Globalization.CultureInfo.GetCultureInfo("de-DE");
        var node = InlineLinkMarkdown.Apply(value, url);
        Span<char> destination = stackalloc char[50];
        var expected = $"[{value.ToString("N0", provider)}]({url})";

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "N0".AsSpan(), provider);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }
}
