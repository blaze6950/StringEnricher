﻿using StringEnricher.Telegram.Helpers.Html;

namespace StringEnricher.Telegram.Tests.Nodes.Html;

public class SpecificCodeBlockNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedSpecificCodeBlock = "<pre><code class=\"language-csharp\">code block</code></pre>";

        // Act
        var styledSpecificCodeBlock = SpecificCodeBlockHtml.Apply("code block", "csharp").ToString();

        // Assert
        Assert.NotNull(styledSpecificCodeBlock);
        Assert.NotEmpty(styledSpecificCodeBlock);
        Assert.Equal(expectedSpecificCodeBlock, styledSpecificCodeBlock);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var specificCodeBlock = SpecificCodeBlockHtml.Apply("test", "csharp");
        const string expected = "<pre><code class=\"language-csharp\">test</code></pre>";

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
    [InlineData(53)] // "<pre><code class=\"language-csharp\">test</code></pre>" length is 53
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var specificCodeBlock = SpecificCodeBlockHtml.Apply("test", "csharp");

        // Act
        var result = specificCodeBlock.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string code = "var x = 1;";
        const string language = "csharp";
        var node = SpecificCodeBlockHtml.Apply(code, language);
        Span<char> destination = stackalloc char[60];
        const string expected = "<pre><code class=\"language-csharp\">var x = 1;</code></pre>";

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
        const string code = "var x = 1;";
        const string language = "csharp";
        var node = SpecificCodeBlockHtml.Apply(code, language);
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
        const string code = "test";
        const string language = "js";
        var node = SpecificCodeBlockHtml.Apply(code, language);
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
        const string code = "x";
        const string language = "js";
        var node = SpecificCodeBlockHtml.Apply(code, language);
        const string expected = "<pre><code class=\"language-js\">x</code></pre>";
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
        const string language = "csharp";
        var node = SpecificCodeBlockHtml.Apply(value, language);
        const string expected = "<pre><code class=\"language-csharp\">123</code></pre>";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}