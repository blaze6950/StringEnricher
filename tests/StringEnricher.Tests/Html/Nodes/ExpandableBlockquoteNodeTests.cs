using StringEnricher.Nodes;
using StringEnricher.Nodes.Html.Formatting;

namespace StringEnricher.Tests.Html.Nodes;

public class ExpandableBlockquoteNodeTests
{
    [Fact]
    public void Apply_WithSimpleText_ReturnsCorrectlyFormattedExpandableBlockquote()
    {
        // Arrange
        const string inputText = "This is a simple expandable quote";
        const string expected = "<blockquote expandable>This is a simple expandable quote</blockquote>";

        // Act
        var result = ExpandableBlockquoteHtml.Apply(inputText).ToString();

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
        const string expected = "<blockquote expandable>Line 1\nLine 2\nLine 3</blockquote>";

        // Act
        var result = ExpandableBlockquoteHtml.Apply(inputText).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithEmptyText_ReturnsExpandableBlockquoteWithEmptyContent()
    {
        // Arrange
        const string inputText = "";
        const string expected = "<blockquote expandable></blockquote>";

        // Act
        var result = ExpandableBlockquoteHtml.Apply(inputText).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithSpecialCharacters_ReturnsCorrectlyFormattedExpandableBlockquote()
    {
        // Arrange
        const string inputText = "Quote with <special> & characters \"quoted\"";
        const string expected = "<blockquote expandable>Quote with <special> & characters \"quoted\"</blockquote>";

        // Act
        var result = ExpandableBlockquoteHtml.Apply(inputText).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Apply_WithLongText_ReturnsCorrectlyFormattedExpandableBlockquote()
    {
        // Arrange
        const string inputText = "This is a very long expandable blockquote that contains multiple sentences and should demonstrate that the expandable blockquote formatting works correctly even with lengthy content that might span multiple lines when displayed.";
        var expected = $"<blockquote expandable>{inputText}</blockquote>";

        // Act
        var result = ExpandableBlockquoteHtml.Apply(inputText).ToString();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var expandableBlockquote = ExpandableBlockquoteHtml.Apply("test");
        const string expected = "<blockquote expandable>test</blockquote>";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = expandableBlockquote.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(40)] // "<blockquote expandable>test</blockquote>" length is 40
    [InlineData(100)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var expandableBlockquote = ExpandableBlockquoteHtml.Apply("test");

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
        var expandableBlockquote = ExpandableBlockquoteHtml.Apply("");
        const string expected = "<blockquote expandable></blockquote>";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = expandableBlockquote.TryGetChar(i, out var ch);
            Assert.True(result, $"TryGetChar should return true for index {i}");
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TotalLength_ReturnsCorrectLength()
    {
        // Arrange
        const string testText = "sample text";
        var expandableBlockquote = ExpandableBlockquoteHtml.Apply(testText);
        const int expectedPrefixLength = 23; // "<blockquote expandable>"
        const int expectedSuffixLength = 13; // "</blockquote>"
        var expectedTotalLength = expectedPrefixLength + testText.Length + expectedSuffixLength;

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
        var expandableBlockquote = ExpandableBlockquoteHtml.Apply(testText);

        // Act
        var innerLength = expandableBlockquote.InnerLength;

        // Assert
        Assert.Equal(testText.Length, innerLength);
    }

    [Fact]
    public void SyntaxLength_ReturnsCorrectSyntaxLength()
    {
        // Arrange
        var expandableBlockquote = ExpandableBlockquoteHtml.Apply("any text");
        const int expectedSyntaxLength = 36; // "<blockquote expandable>" (23) + "</blockquote>" (13)

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
        var expandableBlockquote = ExpandableBlockquoteHtml.Apply(testText);
        const string expected = "<blockquote expandable>test content</blockquote>";
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
        var expandableBlockquote = ExpandableBlockquoteHtml.Apply(testText);
        var insufficientBuffer = new char[10]; // Much smaller than needed

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => expandableBlockquote.CopyTo(insufficientBuffer.AsSpan()));
        Assert.Contains("destination span is too small", exception.Message);
    }

    [Fact]
    public void CopyTo_WithExactBuffer_CopiesCorrectly()
    {
        // Arrange
        const string testText = "exact";
        var expandableBlockquote = ExpandableBlockquoteHtml.Apply(testText);
        var buffer = new char[expandableBlockquote.TotalLength];
        const string expected = "<blockquote expandable>exact</blockquote>";

        // Act
        var copiedLength = expandableBlockquote.CopyTo(buffer.AsSpan());

        // Assert
        Assert.Equal(expandableBlockquote.TotalLength, copiedLength);
        Assert.Equal(expected, new string(buffer));
    }

    [Fact]
    public void Constants_HaveCorrectValues()
    {
        // Act & Assert
        Assert.Equal("<blockquote expandable>", ExpandableBlockquoteNode<PlainTextNode>.Prefix);
        Assert.Equal("</blockquote>", ExpandableBlockquoteNode<PlainTextNode>.Suffix);
    }
}
