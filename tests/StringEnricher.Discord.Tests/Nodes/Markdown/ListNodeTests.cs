using StringEnricher.Discord.Helpers.Markdown;
using StringEnricher.Discord.Nodes.Markdown.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Discord.Tests.Nodes.Markdown;

public class ListNodeTests
{
    [Fact]
    public void Apply_WithSimpleText_ReturnsCorrectlyFormattedList()
    {
        // Arrange
        const string inputText = "This is a simple list item";
        const string expected = "- This is a simple list item";

        // Act
        var result = ListMarkdown.Apply(inputText).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithMultiLineText_ReturnsCorrectlyFormattedList()
    {
        // Arrange
        const string inputText = "Line 1\nLine 2\nLine 3";
        const string expected = "- Line 1\n- Line 2\n- Line 3";

        // Act
        var result = ListMarkdown.Apply(inputText).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithEmptyText_ReturnsListWithOnlyPrefix()
    {
        // Arrange
        const string inputText = "";
        const string expected = "- ";

        // Act
        var result = ListMarkdown.Apply(inputText).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithComplexMultiLineText_ReturnsCorrectFormat()
    {
        // Arrange
        const string inputText =
            "First list item\nSecond list item\nThird list item";
        const string expected =
            "- First list item\n- Second list item\n- Third list item";

        // Act
        var result = ListMarkdown.Apply(inputText).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithSpecialCharacters_PreservesCharacters()
    {
        // Arrange
        const string inputText = "List with *bold* & _italic_ text";
        const string expected = "- List with *bold* & _italic_ text";

        // Act
        var result = ListMarkdown.Apply(inputText).ToString();

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
        const string expected = "- First line\n- Second line";

        // Act
        var result = ListMarkdown.Apply(inputText).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithMultipleConsecutiveLineBreaks_HandlesCorrectly()
    {
        // Arrange
        const string inputText = "Line 1\n\nLine 3";
        const string expected = "- Line 1\n- \n- Line 3";

        // Act
        var result = ListMarkdown.Apply(inputText).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var list = ListMarkdown.Apply("test");
        const string expected = "- test";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = list.TryGetChar(i, out var ch);
            Assert.True(result, $"TryGetChar should return true for index {i}");
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TryGetChar_MultiLineText_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var list = ListMarkdown.Apply("Line 1\nLine 2");
        const string expected = "- Line 1\n- Line 2";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = list.TryGetChar(i, out var ch);
            Assert.True(result, $"TryGetChar should return true for index {i}");
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)] // "- test" length is 6
    [InlineData(100)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var list = ListMarkdown.Apply("test");

        // Act
        var result = list.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result, $"TryGetChar should return false for out-of-range index {index}");
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryGetChar_EmptyContent_WorksCorrectlyForPrefix()
    {
        // Arrange
        var list = ListMarkdown.Apply("");
        const string expected = "- ";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = list.TryGetChar(i, out var ch);
            Assert.True(result, $"TryGetChar should return true for index {i}");
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TotalLength_SingleLine_ReturnsCorrectLength()
    {
        // Arrange
        const string testText = "sample text";
        var list = ListMarkdown.Apply(testText);
        // Expected: "- sample text" = 2 (- ) + 11 (text) = 13
        const int expectedTotalLength = 13;

        // Act
        var totalLength = list.TotalLength;

        // Assert
        Assert.Equal(expectedTotalLength, totalLength);
    }

    [Fact]
    public void TotalLength_MultiLine_ReturnsCorrectLength()
    {
        // Arrange
        const string testText = "line1\nline2";
        var list = ListMarkdown.Apply(testText);
        // Expected: "- line1\n- line2" = 2 (- ) + 5 (line1) + 1 (\n) + 2 (- ) + 5 (line2) = 15
        const int expectedTotalLength = 15;

        // Act
        var totalLength = list.TotalLength;

        // Assert
        Assert.Equal(expectedTotalLength, totalLength);
    }

    [Fact]
    public void InnerLength_ReturnsCorrectInnerTextLength()
    {
        // Arrange
        const string testText = "inner content";
        var list = ListMarkdown.Apply(testText);

        // Act
        var innerLength = list.InnerLength;

        // Assert
        Assert.Equal(testText.Length, innerLength);
    }

    [Fact]
    public void SyntaxLength_SingleLine_ReturnsCorrectSyntaxLength()
    {
        // Arrange
        var list = ListMarkdown.Apply("any text");
        const int expectedSyntaxLength = 2; // 1 line prefix "- " (2 chars)

        // Act
        var syntaxLength = list.SyntaxLength;

        // Assert
        Assert.Equal(expectedSyntaxLength, syntaxLength);
    }

    [Fact]
    public void SyntaxLength_MultiLine_ReturnsCorrectSyntaxLength()
    {
        // Arrange
        var list = ListMarkdown.Apply("line1\nline2\nline3");
        const int expectedSyntaxLength = 6; // 3 line prefixes "- " (2 chars each)

        // Act
        var syntaxLength = list.SyntaxLength;

        // Assert
        Assert.Equal(expectedSyntaxLength, syntaxLength);
    }

    [Fact]
    public void CopyTo_WithSufficientBuffer_CopiesCorrectly()
    {
        // Arrange
        const string testText = "test content";
        var list = ListMarkdown.Apply(testText);
        const string expected = "- test content";
        var buffer = new char[expected.Length];

        // Act
        var copiedLength = list.CopyTo(buffer.AsSpan());

        // Assert
        Assert.Equal(expected.Length, copiedLength);
        Assert.Equal(expected, new string(buffer));
    }

    [Fact]
    public void CopyTo_WithMultiLineContent_CopiesCorrectly()
    {
        // Arrange
        const string testText = "line1\nline2";
        var list = ListMarkdown.Apply(testText);
        const string expected = "- line1\n- line2";
        var buffer = new char[expected.Length];

        // Act
        var copiedLength = list.CopyTo(buffer.AsSpan());

        // Assert
        Assert.Equal(expected.Length, copiedLength);
        Assert.Equal(expected, new string(buffer));
    }

    [Fact]
    public void CopyTo_WithInsufficientBuffer_ThrowsArgumentException()
    {
        // Arrange
        const string testText = "test content";
        var list = ListMarkdown.Apply(testText);
        var insufficientBuffer = new char[5]; // Much smaller than needed

        // Act & Assert
        var exception =
            Assert.Throws<ArgumentException>(() => list.CopyTo(insufficientBuffer.AsSpan()));
        Assert.Contains("destination span is too small", exception.Message);
    }

    [Fact]
    public void CopyTo_WithExactBuffer_CopiesCorrectly()
    {
        // Arrange
        const string testText = "exact";
        var list = ListMarkdown.Apply(testText);
        var buffer = new char[list.TotalLength];
        const string expected = "- exact";

        // Act
        var copiedLength = list.CopyTo(buffer.AsSpan());

        // Assert
        Assert.Equal(list.TotalLength, copiedLength);
        Assert.Equal(expected, new string(buffer));
    }

    [Theory]
    [InlineData("Short")]
    [InlineData("Medium length text with some words")]
    [InlineData("Line 1\nLine 2")]
    [InlineData("Multiple\nLines\nWith\nContent")]
    [InlineData(
        "Very long text that contains multiple sentences and should test the list functionality with extensive content.")]
    public void Apply_WithVariousTextLengths_ProducesCorrectOutput(string inputText)
    {
        // Arrange
        var lines = inputText.Split('\n');
        var expectedLines = lines.Select(line => $"- {line}");
        var expected = string.Join("\n", expectedLines);

        // Act
        var result = ListMarkdown.Apply(inputText).ToString();

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(expected.Length, ListMarkdown.Apply(inputText).TotalLength);
    }

    [Fact]
    public void Constants_HaveCorrectValues()
    {
        // Act & Assert
        Assert.Equal("- ", ListNode<PlainTextNode>.LinePrefix);
        Assert.Equal('\n', ListNode<PlainTextNode>.LineSeparator);
    }

    [Fact]
    public void Apply_WithGenericNode_WorksCorrectly()
    {
        // Arrange
        var plainTextNode = new PlainTextNode("nested content");

        // Act
        var result = ListMarkdown.Apply(plainTextNode);
        var output = result.ToString();

        // Assert
        Assert.Equal("- nested content", output);
        Assert.Equal(plainTextNode.TotalLength, result.InnerLength);
    }

    [Fact]
    public void Apply_WithGenericNodeMultiLine_WorksCorrectly()
    {
        // Arrange
        var plainTextNode = new PlainTextNode("line1\nline2");

        // Act
        var result = ListMarkdown.Apply(plainTextNode);
        var output = result.ToString();

        // Assert
        Assert.Equal("- line1\n- line2", output);
        Assert.Equal(plainTextNode.TotalLength, result.InnerLength);
    }

    [Fact]
    public void CharacterIterator_WorksCorrectlyWithComplexText()
    {
        // Arrange
        const string testText = "First\nSecond\nThird";
        var list = ListMarkdown.Apply(testText);
        const string expected = "- First\n- Second\n- Third";

        // Act - Use ToString which internally uses the character iterator
        var result = list.ToString();

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(expected.Length, list.TotalLength);
    }
}