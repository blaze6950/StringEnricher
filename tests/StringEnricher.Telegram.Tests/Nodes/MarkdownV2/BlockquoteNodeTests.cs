﻿using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Telegram.Tests.Nodes.MarkdownV2;

public class BlockquoteNodeTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        const string expectedCodeBlock =
            ">Block quotation started\n>Block quotation continued\n>The last line of the block quotation";

        // Act
        var styledCodeBlock = BlockquoteMarkdownV2
            .Apply("Block quotation started\nBlock quotation continued\nThe last line of the block quotation")
            .ToString();

        // Assert
        Assert.NotNull(styledCodeBlock);
        Assert.NotEmpty(styledCodeBlock);
        Assert.Equal(expectedCodeBlock, styledCodeBlock);
    }

    [Fact]
    public void Test_TryGetChar()
    {
        // Arrange
        var blockQuote =
            BlockquoteMarkdownV2.Apply(
                "Block quotation started\nBlock quotation continued\nThe last line of the block quotation");
        const string expectedCodeBlock =
            ">Block quotation started\n>Block quotation continued\n>The last line of the block quotation";
        var expectedTotalLength = expectedCodeBlock.Length;

        // Act & Assert
        for (var i = 0; i < expectedTotalLength; i++)
        {
            var result = blockQuote.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expectedCodeBlock[i], actualChar);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(89)] // expected string length is 88
    public void Test_TryGetChar_OutOfRange_ShouldReturnFalse(int index)
    {
        // Arrange
        var blockQuote =
            BlockquoteMarkdownV2.Apply(
                "Block quotation started\nBlock quotation continued\nThe last line of the block quotation");

        // Act
        var result = blockQuote.TryGetChar(index, out var actualChar);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', actualChar);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string innerText = "test";
        var node = BlockquoteMarkdownV2.Apply(innerText);
        Span<char> destination = stackalloc char[20];
        const string expected = ">test";

        // Act
        var success = node.TryFormat(destination, out var charsWritten, default, null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        var node = BlockquoteMarkdownV2.Apply("test");
        Span<char> destination = stackalloc char[3];

        // Act
        var success = node.TryFormat(destination, out var charsWritten, default, null);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_EmptyDestination_ReturnsFalse()
    {
        // Arrange
        var node = BlockquoteMarkdownV2.Apply("text");
        Span<char> destination = Span<char>.Empty;

        // Act
        var success = node.TryFormat(destination, out var charsWritten, default, null);

        // Assert
        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryFormat_ExactSizeDestination_ReturnsTrue()
    {
        // Arrange
        const string innerText = "test";
        var node = BlockquoteMarkdownV2.Apply(innerText);
        const string expected = ">test";
        Span<char> destination = stackalloc char[expected.Length];

        // Act
        var success = node.TryFormat(destination, out var charsWritten, default, null);

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
        var node = BlockquoteMarkdownV2.Apply(value);
        const string expected = ">123";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }
}