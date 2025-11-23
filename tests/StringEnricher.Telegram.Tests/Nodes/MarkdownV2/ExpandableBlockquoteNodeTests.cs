using StringEnricher.Nodes.Shared;
using StringEnricher.Telegram.Helpers.MarkdownV2;
using StringEnricher.Telegram.Nodes.MarkdownV2.Formatting;

namespace StringEnricher.Telegram.Tests.Nodes.MarkdownV2;

public class ExpandableBlockquoteNodeTests
{
    [Fact]
    public void Apply_WithSimpleText_ReturnsCorrectlyFormattedExpandableBlockquote()
    {
        // Arrange
        const string inputText = "This is a simple expandable quote";
        const string expected = ">This is a simple expandable quote||";

        // Act
        var result = ExpandableBlockquoteMarkdownV2.Apply(inputText).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithMultiLineText_ReturnsCorrectlyFormattedExpandableBlockquote()
    {
        // Arrange
        const string inputText = "Line 1\nLine 2\nLine 3";
        const string expected = ">Line 1\n>Line 2\n>Line 3||";

        // Act
        var result = ExpandableBlockquoteMarkdownV2.Apply(inputText).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithEmptyText_ReturnsExpandableBlockquoteWithOnlyPrefixAndSuffix()
    {
        // Arrange
        const string inputText = "";
        const string expected = ">||";

        // Act
        var result = ExpandableBlockquoteMarkdownV2.Apply(inputText).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithComplexMultiLineText_ReturnsCorrectFormat()
    {
        // Arrange
        const string inputText = "Block quotation started\nBlock quotation continued\nThe last line of the block quotation";
        const string expected = ">Block quotation started\n>Block quotation continued\n>The last line of the block quotation||";

        // Act
        var result = ExpandableBlockquoteMarkdownV2.Apply(inputText).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithSpecialCharacters_PreservesCharacters()
    {
        // Arrange
        const string inputText = "Quote with *bold* & _italic_ text";
        const string expected = ">Quote with *bold* & _italic_ text||";

        // Act
        var result = ExpandableBlockquoteMarkdownV2.Apply(inputText).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithSingleLineBreak_AddsCorrectPrefixes()
    {
        // Arrange
        const string inputText = "First line\nSecond line";
        const string expected = ">First line\n>Second line||";

        // Act
        var result = ExpandableBlockquoteMarkdownV2.Apply(inputText).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithMultipleConsecutiveLineBreaks_HandlesCorrectly()
    {
        // Arrange
        const string inputText = "Line 1\n\nLine 3";
        const string expected = ">Line 1\n>\n>Line 3||";

        // Act
        var result = ExpandableBlockquoteMarkdownV2.Apply(inputText).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply("test");
        const string expected = ">test||";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = expandableBlockquote.TryGetChar(i, out var ch);
            Assert.True(result, $"TryGetChar should return true for index {i}");
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TryGetChar_MultiLineText_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply("Line 1\nLine 2");
        const string expected = ">Line 1\n>Line 2||";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = expandableBlockquote.TryGetChar(i, out var ch);
            Assert.True(result, $"TryGetChar should return true for index {i}");
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(7)] // ">test||" length is 7
    [InlineData(100)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply("test");

        // Act
        var result = expandableBlockquote.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result, $"TryGetChar should return false for out-of-range index {index}");
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryGetChar_EmptyContent_WorksCorrectlyForPrefixAndSuffix()
    {
        // Arrange
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply("");
        const string expected = ">||";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = expandableBlockquote.TryGetChar(i, out var ch);
            Assert.True(result, $"TryGetChar should return true for index {i}");
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TotalLength_SingleLine_ReturnsCorrectLength()
    {
        // Arrange
        const string testText = "sample text";
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply(testText);
        // Expected: ">sample text||" = 1 (>) + 11 (text) + 2 (||) = 14
        const int expectedTotalLength = 14;

        // Act
        var totalLength = expandableBlockquote.TotalLength;

        // Assert
        Assert.Equal(expectedTotalLength, totalLength);
    }

    [Fact]
    public void TotalLength_MultiLine_ReturnsCorrectLength()
    {
        // Arrange
        const string testText = "line1\nline2";
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply(testText);
        // Expected: ">line1\n>line2||" = 1 (>) + 5 (line1) + 1 (\n) + 1 (>) + 5 (line2) + 2 (||) = 15
        const int expectedTotalLength = 15;

        // Act
        var totalLength = expandableBlockquote.TotalLength;

        // Assert
        Assert.Equal(expectedTotalLength, totalLength);
    }

    [Fact]
    public void InnerLength_ReturnsCorrectInnerTextLength()
    {
        // Arrange
        const string testText = "inner content";
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply(testText);

        // Act
        var innerLength = expandableBlockquote.InnerLength;

        // Assert
        Assert.Equal(testText.Length, innerLength);
    }

    [Fact]
    public void SyntaxLength_SingleLine_ReturnsCorrectSyntaxLength()
    {
        // Arrange
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply("any text");
        const int expectedSyntaxLength = 3; // 1 line prefix (>) + 2 suffix (||)

        // Act
        var syntaxLength = expandableBlockquote.SyntaxLength;

        // Assert
        Assert.Equal(expectedSyntaxLength, syntaxLength);
    }

    [Fact]
    public void SyntaxLength_MultiLine_ReturnsCorrectSyntaxLength()
    {
        // Arrange
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply("line1\nline2\nline3");
        const int expectedSyntaxLength = 5; // 3 line prefixes (>) + 2 suffix (||)

        // Act
        var syntaxLength = expandableBlockquote.SyntaxLength;

        // Assert
        Assert.Equal(expectedSyntaxLength, syntaxLength);
    }

    [Fact]
    public void CopyTo_WithSufficientBuffer_CopiesCorrectly()
    {
        // Arrange
        const string testText = "test content";
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply(testText);
        const string expected = ">test content||";
        var buffer = new char[expected.Length];

        // Act
        var copiedLength = expandableBlockquote.CopyTo(buffer.AsSpan());

        // Assert
        Assert.Equal(expected.Length, copiedLength);
        Assert.Equal(expected, new string(buffer));
    }

    [Fact]
    public void CopyTo_WithMultiLineContent_CopiesCorrectly()
    {
        // Arrange
        const string testText = "line1\nline2";
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply(testText);
        const string expected = ">line1\n>line2||";
        var buffer = new char[expected.Length];

        // Act
        var copiedLength = expandableBlockquote.CopyTo(buffer.AsSpan());

        // Assert
        Assert.Equal(expected.Length, copiedLength);
        Assert.Equal(expected, new string(buffer));
    }

    [Fact]
    public void CopyTo_WithInsufficientBuffer_ThrowsArgumentException()
    {
        // Arrange
        const string testText = "test content";
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply(testText);
        var insufficientBuffer = new char[5]; // Much smaller than needed

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => expandableBlockquote.CopyTo(insufficientBuffer.AsSpan()));
        Assert.Contains("Destination span is not large enough to hold the written characters.", exception.Message);
    }

    [Fact]
    public void CopyTo_WithExactBuffer_CopiesCorrectly()
    {
        // Arrange
        const string testText = "exact";
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply(testText);
        var buffer = new char[expandableBlockquote.TotalLength];
        const string expected = ">exact||";

        // Act
        var copiedLength = expandableBlockquote.CopyTo(buffer.AsSpan());

        // Assert
        Assert.Equal(expandableBlockquote.TotalLength, copiedLength);
        Assert.Equal(expected, new string(buffer));
    }

    [Theory]
    [InlineData("Short")]
    [InlineData("Medium length text with some words")]
    [InlineData("Line 1\nLine 2")]
    [InlineData("Multiple\nLines\nWith\nContent")]
    [InlineData("Very long text that contains multiple sentences and should test the expandable blockquote functionality with extensive content.")]
    public void Apply_WithVariousTextLengths_ProducesCorrectOutput(string inputText)
    {
        // Arrange
        var lines = inputText.Split('\n');
        var expectedLines = lines.Select(line => $">{line}");
        var expected = string.Join("\n", expectedLines) + "||";

        // Act
        var result = ExpandableBlockquoteMarkdownV2.Apply(inputText).ToString();

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(expected.Length, ExpandableBlockquoteMarkdownV2.Apply(inputText).TotalLength);
    }

    [Fact]
    public void Constants_HaveCorrectValues()
    {
        // Act & Assert
        Assert.Equal('>', ExpandableBlockquoteNode<PlainTextNode>.LinePrefix);
        Assert.Equal('\n', ExpandableBlockquoteNode<PlainTextNode>.LineSeparator);
        Assert.Equal("||", ExpandableBlockquoteNode<PlainTextNode>.Suffix);
    }

    [Fact]
    public void Apply_WithGenericNode_WorksCorrectly()
    {
        // Arrange
        var plainTextNode = new PlainTextNode("nested content");

        // Act
        var result = ExpandableBlockquoteMarkdownV2.Apply(plainTextNode);
        var output = result.ToString();

        // Assert
        Assert.Equal(">nested content||", output);
        Assert.Equal(plainTextNode.TotalLength, result.InnerLength);
    }

    [Fact]
    public void Apply_WithGenericNodeMultiLine_WorksCorrectly()
    {
        // Arrange
        var plainTextNode = new PlainTextNode("line1\nline2");

        // Act
        var result = ExpandableBlockquoteMarkdownV2.Apply(plainTextNode);
        var output = result.ToString();

        // Assert
        Assert.Equal(">line1\n>line2||", output);
        Assert.Equal(plainTextNode.TotalLength, result.InnerLength);
    }

    [Fact]
    public void CharacterIterator_WorksCorrectlyWithComplexText()
    {
        // Arrange
        const string testText = "First\nSecond\nThird";
        var expandableBlockquote = ExpandableBlockquoteMarkdownV2.Apply(testText);
        const string expected = ">First\n>Second\n>Third||";

        // Act - Use ToString which internally uses the character iterator
        var result = expandableBlockquote.ToString();

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(expected.Length, expandableBlockquote.TotalLength);
    }
}
