﻿﻿using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Nodes.MarkdownV2;

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

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string code = "var x = 1;";
        const string language = "csharp";
        var node = SpecificCodeBlockMarkdownV2.Apply(code, language);
        Span<char> destination = stackalloc char[50];
        const string expected = "```csharp\nvar x = 1;\n```";

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
        var node = SpecificCodeBlockMarkdownV2.Apply(code, language);
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
        var node = SpecificCodeBlockMarkdownV2.Apply(code, language);
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
        const string code = "test";
        const string language = "js";
        var node = SpecificCodeBlockMarkdownV2.Apply(code, language);
        const string expected = "```js\ntest\n```";
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
        var node = SpecificCodeBlockMarkdownV2.Apply(value, language);
        const string expected = "```csharp\n123\n```";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}