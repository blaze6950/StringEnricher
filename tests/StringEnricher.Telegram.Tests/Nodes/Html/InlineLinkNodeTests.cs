﻿﻿using StringEnricher.Telegram.Helpers.Html;

namespace StringEnricher.Telegram.Tests.Nodes.Html;

public class InlineLinkNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedLink = "<a href=\"https://example.com\">test</a>";

        // Act
        var styledLink = InlineLinkHtml.Apply("test", "https://example.com").ToString();

        // Assert
        Assert.NotNull(styledLink);
        Assert.NotEmpty(styledLink);
        Assert.Equal(expectedLink, styledLink);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var inlineLink = InlineLinkHtml.Apply("test", "https://example.com");
        const string expected = "<a href=\"https://example.com\">test</a>";

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
    [InlineData(39)] // "<a href=\"https://example.com\">test</a>" length is 39
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var inlineLink = InlineLinkHtml.Apply("test", "https://example.com");

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
        var node = InlineLinkHtml.Apply(text, url);
        Span<char> destination = stackalloc char[60];
        const string expected = "<a href=\"https://example.com\">test</a>";

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
        var node = InlineLinkHtml.Apply(text, url);
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
        var node = InlineLinkHtml.Apply(text, url);
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
        var node = InlineLinkHtml.Apply(text, url);
        const string expected = "<a href=\"https://example.com\">test</a>";
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
        var node = InlineLinkHtml.Apply(value, url);
        const string expected = "<a href=\"https://example.com\">123</a>";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}