using StringEnricher.Debug;
using StringEnricher.Helpers;
using StringEnricher.Nodes.Shared;
using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Tests.Nodes;

public class CompositeNodeTests
{
    [Fact]
    public void Test_ExplicitExtensionMethod()
    {
        // Arrange
        var leftBold = BoldMarkdownV2.Apply("left");
        var rightBold = BoldMarkdownV2.Apply("right");
        var expectedTotalLength = rightBold.TotalLength + leftBold.TotalLength;
        var expectedSyntaxLength = rightBold.SyntaxLength + leftBold.SyntaxLength;
        var expectedString = leftBold.ToString() + rightBold.ToString();

        // Act
        var composite = CompositeNodeExtensions.CombineWith(leftBold, rightBold);

        // Assert
        Assert.Equal(expectedTotalLength, composite.TotalLength);
        Assert.Equal(expectedSyntaxLength, composite.SyntaxLength);
        Assert.Equal(expectedString, composite.ToString());
    }

    [Fact]
    public void Test_ImplicitExtensionMethod()
    {
        // Arrange
        var leftBold = BoldMarkdownV2.Apply("left");
        var rightBold = BoldMarkdownV2.Apply("right");
        var expectedTotalLength = rightBold.TotalLength + leftBold.TotalLength;
        var expectedSyntaxLength = rightBold.SyntaxLength + leftBold.SyntaxLength;
        var expectedString = leftBold.ToString() + rightBold.ToString();

        // Act
        var composite = leftBold.CombineWith(rightBold);

        // Assert
        Assert.Equal(expectedTotalLength, composite.TotalLength);
        Assert.Equal(expectedSyntaxLength, composite.SyntaxLength);
        Assert.Equal(expectedString, composite.ToString());
    }

    #region ISpanFormattable Tests

    [Fact]
    public void ToString_WithFormatAndProvider_PassesToBothNodes()
    {
        // Arrange
        var left = new IntegerNode(123);
        var right = new IntegerNode(456);
        var composite = new CompositeNode<IntegerNode, IntegerNode>(left, right);

        // Act - format and provider are passed to underlying nodes
        var result = composite.ToString("X", System.Globalization.CultureInfo.InvariantCulture);

        // Assert - both integers should be formatted in hex
        Assert.Equal("7B1C8", result);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        var left = new PlainTextNode("Hello");
        var right = new PlainTextNode("World");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);
        Span<char> destination = stackalloc char[50];

        // Act
        var success = composite.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(10, charsWritten);
        Assert.Equal("HelloWorld", destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithFormatParameter_PassesToBothNodes()
    {
        // Arrange
        var left = new IntegerNode(255);
        var right = new IntegerNode(16);
        var composite = new CompositeNode<IntegerNode, IntegerNode>(left, right);
        Span<char> destination = stackalloc char[50];

        // Act
        var success = composite.TryFormat(destination, out var charsWritten, "X".AsSpan(),
            System.Globalization.CultureInfo.InvariantCulture);

        // Assert
        Assert.True(success);
        Assert.Equal("FF10", destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        var left = new PlainTextNode("Hello");
        var right = new PlainTextNode("World");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);
        Span<char> destination = stackalloc char[5]; // Too small

        // Act
        var success = composite.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TotalLength_IsCachedAfterFirstAccess()
    {
        // Arrange
        var left = new IntegerNode(123, "N0", System.Globalization.CultureInfo.InvariantCulture);
        var right = new IntegerNode(456, "N0", System.Globalization.CultureInfo.InvariantCulture);
        var composite = new CompositeNode<IntegerNode, IntegerNode>(left, right);

        // Act
        var length1 = composite.TotalLength;
        var length2 = composite.TotalLength;

        // Assert
        Assert.Equal(length1, length2);
    }

    [Fact]
    public void SyntaxLength_CombinesBothNodes()
    {
        // Arrange
        var leftBold = BoldMarkdownV2.Apply("test");
        var rightBold = BoldMarkdownV2.Apply("data");
        var composite = leftBold.CombineWith(rightBold);
        var expectedSyntaxLength = leftBold.SyntaxLength + rightBold.SyntaxLength;

        // Act
        var syntaxLength = composite.SyntaxLength;

        // Assert
        Assert.Equal(expectedSyntaxLength, syntaxLength);
        Assert.True(syntaxLength > 0); // Both bold nodes have syntax
    }

    [Fact]
    public void TryFormat_WithMixedNodeTypes_FormatsCorrectly()
    {
        // Arrange
        var left = new PlainTextNode("Count: ");
        var right = new IntegerNode(42);
        var composite = new CompositeNode<PlainTextNode, IntegerNode>(left, right);
        Span<char> destination = stackalloc char[50];

        // Act
        var success = composite.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal("Count: 42", destination[..charsWritten].ToString());
    }

    #endregion

    #region CopyTo Tests

    [Fact]
    public void CopyTo_WithSufficientSpace_CopiesBothNodesCorrectly()
    {
        // Arrange
        var left = new PlainTextNode("Hello");
        var right = new PlainTextNode("World");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);
        Span<char> destination = stackalloc char[20];
        var expectedString = "HelloWorld";
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = composite.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithMixedNodeTypes_CopiesCorrectly()
    {
        // Arrange
        var left = new PlainTextNode("Value: ");
        var right = new IntegerNode(123);
        var composite = new CompositeNode<PlainTextNode, IntegerNode>(left, right);
        Span<char> destination = stackalloc char[20];
        var expectedString = "Value: 123";
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = composite.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithBoldNodes_CopiesWithSyntax()
    {
        // Arrange
        var leftBold = BoldMarkdownV2.Apply("Left");
        var rightBold = BoldMarkdownV2.Apply("Right");
        var composite = leftBold.CombineWith(rightBold);
        Span<char> destination = stackalloc char[30];
        var expectedString = leftBold.ToString() + rightBold.ToString();
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = composite.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        var left = new PlainTextNode("AB");
        var right = new PlainTextNode("CD");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);
        var expectedString = "ABCD";
        Span<char> destination = stackalloc char[expectedString.Length]; // Exact size
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = composite.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }

    [Fact]
    public void CopyTo_WithEmptyLeftNode_CopiesOnlyRightNode()
    {
        // Arrange
        var left = new PlainTextNode("");
        var right = new PlainTextNode("Right");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);
        Span<char> destination = stackalloc char[10];
        var expectedString = "Right";
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = composite.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithEmptyRightNode_CopiesOnlyLeftNode()
    {
        // Arrange
        var left = new PlainTextNode("Left");
        var right = new PlainTextNode("");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);
        Span<char> destination = stackalloc char[10];
        var expectedString = "Left";
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = composite.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithBothEmptyNodes_CopiesNothing()
    {
        // Arrange
        var left = new PlainTextNode("");
        var right = new PlainTextNode("");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 0;

        // Act
        var bytesWritten = composite.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
    }

    [Fact]
    public void CopyTo_WithNumericNodes_CopiesCorrectly()
    {
        // Arrange
        var left = new LongNode(12345L);
        var right = new FloatNode(67.89f);
        var composite = new CompositeNode<LongNode, FloatNode>(left, right);
        Span<char> destination = stackalloc char[30];
        var expectedString = left.ToString() + right.ToString();
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = composite.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    #endregion

    #region TryGetChar Tests

    [Fact]
    public void TryGetChar_ValidIndicesInLeftNode_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var left = new PlainTextNode("Hello");
        var right = new PlainTextNode("World");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);
        var expected = "HelloWorld";

        // Act & Assert
        for (var i = 0; i < 5; i++) // First 5 chars are from left node
        {
            var result = composite.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TryGetChar_ValidIndicesInRightNode_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var left = new PlainTextNode("Hello");
        var right = new PlainTextNode("World");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);
        var expected = "HelloWorld";

        // Act & Assert
        for (var i = 5; i < 10; i++) // Last 5 chars are from right node
        {
            var result = composite.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TryGetChar_AllValidIndices_ReturnsTrueAndCorrectChars()
    {
        // Arrange
        var left = new PlainTextNode("ABC");
        var right = new PlainTextNode("XYZ");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);
        var expected = "ABCXYZ";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = composite.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(-100)]
    public void TryGetChar_NegativeIndex_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var left = new PlainTextNode("Hello");
        var right = new PlainTextNode("World");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);

        // Act
        var result = composite.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Theory]
    [InlineData(10)] // Length is 10, so index 10 is out of range
    [InlineData(11)]
    [InlineData(100)]
    public void TryGetChar_IndexBeyondLength_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var left = new PlainTextNode("Hello");
        var right = new PlainTextNode("World");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);

        // Act
        var result = composite.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryGetChar_OutOfRightRangeIndexWhenTotalLengthWasCalculated_ReturnsFalseAndNullChar()
    {
        // Arrange
        var left = new PlainTextNode("Hello");
        var right = new PlainTextNode("World");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);
        var totalLength = composite.TotalLength;
        DebugCounters.ResetAllCounters();

        // Act
        var result = composite.TryGetChar(totalLength, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
        Assert.Equal(1, DebugCounters.CompositeNode_TryGetChar_CachedTotalLengthEvaluation);
    }

    [Fact]
    public void TryGetChar_WithEmptyLeftNode_GetsCharFromRightNode()
    {
        // Arrange
        var left = new PlainTextNode("");
        var right = new PlainTextNode("Test");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);

        // Act & Assert
        var result = composite.TryGetChar(0, out var ch);
        Assert.True(result);
        Assert.Equal('T', ch);
    }

    [Fact]
    public void TryGetChar_WithEmptyRightNode_GetsCharFromLeftNode()
    {
        // Arrange
        var left = new PlainTextNode("Test");
        var right = new PlainTextNode("");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);

        // Act & Assert
        var result = composite.TryGetChar(0, out var ch);
        Assert.True(result);
        Assert.Equal('T', ch);
    }

    [Fact]
    public void TryGetChar_WithBothEmptyNodes_ReturnsFalse()
    {
        // Arrange
        var left = new PlainTextNode("");
        var right = new PlainTextNode("");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);

        // Act
        var result = composite.TryGetChar(0, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryGetChar_AtLeftNodeBoundary_ReturnsCorrectChar()
    {
        // Arrange
        var left = new PlainTextNode("Hello");
        var right = new PlainTextNode("World");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);

        // Act - Last char of left node
        var result = composite.TryGetChar(4, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal('o', ch);
    }

    [Fact]
    public void TryGetChar_AtRightNodeStart_ReturnsCorrectChar()
    {
        // Arrange
        var left = new PlainTextNode("Hello");
        var right = new PlainTextNode("World");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);

        // Act - First char of right node
        var result = composite.TryGetChar(5, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal('W', ch);
    }

    [Fact]
    public void TryGetChar_WithBoldNodes_GetsCharsIncludingSyntax()
    {
        // Arrange
        var leftBold = BoldMarkdownV2.Apply("A");
        var rightBold = BoldMarkdownV2.Apply("B");
        var composite = leftBold.CombineWith(rightBold);
        var expected = leftBold.ToString() + rightBold.ToString();

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = composite.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TryGetChar_WithNumericNodes_GetsCorrectChars()
    {
        // Arrange
        var left = new IntegerNode(123);
        var right = new LongNode(456L);
        var composite = new CompositeNode<IntegerNode, LongNode>(left, right);
        var expected = "123456";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = composite.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TryGetChar_WithFormattedNumericNodes_GetsCorrectChars()
    {
        // Arrange
        var left = new IntegerNode(123, "X", System.Globalization.CultureInfo.InvariantCulture);
        var right = new IntegerNode(456, "X", System.Globalization.CultureInfo.InvariantCulture);
        var composite = new CompositeNode<IntegerNode, IntegerNode>(left, right);
        var expected = "7B1C8";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = composite.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TryGetChar_BeforeTotalLengthCached_CalculatesCorrectly()
    {
        // Arrange
        var left = new PlainTextNode("Test");
        var right = new PlainTextNode("Data");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);
        // Don't access TotalLength before calling TryGetChar

        // Act & Assert - Should work even without cached TotalLength
        var result = composite.TryGetChar(5, out var ch);
        Assert.True(result);
        Assert.Equal('a', ch);
    }

    [Fact]
    public void TryGetChar_AfterTotalLengthCached_UsesCachedValue()
    {
        // Arrange
        var left = new PlainTextNode("Test");
        var right = new PlainTextNode("Data");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);
        var totalLength = composite.TotalLength; // Cache the total length
        DebugCounters.ResetAllCounters();

        // Act
        var result = composite.TryGetChar(100, out var ch);

        // Assert - Should use cached value for out-of-range check
        Assert.False(result);
        Assert.Equal('\0', ch);
        Assert.Equal(1, DebugCounters.CompositeNode_TryGetChar_CachedTotalLengthEvaluation);
    }

    [Fact]
    public void TryGetChar_WithMixedContentTypes_GetsAllCharsCorrectly()
    {
        // Arrange
        var left = new PlainTextNode("Num:");
        var right = new IntegerNode(42);
        var composite = new CompositeNode<PlainTextNode, IntegerNode>(left, right);
        var expected = "Num:42";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = composite.TryGetChar(i, out var ch);
            Assert.True(result, $"Failed at index {i}");
            Assert.Equal(expected[i], ch);
        }
    }

    #endregion

    #region Constructor and Properties Tests

    [Fact]
    public void Constructor_WithTwoNodes_InitializesCorrectly()
    {
        // Arrange
        var left = new PlainTextNode("Left");
        var right = new PlainTextNode("Right");

        // Act
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);

        // Assert
        Assert.Equal(9, composite.TotalLength);
        Assert.Equal(0, composite.SyntaxLength);
        Assert.Equal("LeftRight", composite.ToString());
    }

    [Fact]
    public void TotalLength_CombinesBothNodesLengths()
    {
        // Arrange
        var left = new PlainTextNode("ABC");
        var right = new PlainTextNode("XYZ");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);

        // Act
        var totalLength = composite.TotalLength;

        // Assert
        Assert.Equal(6, totalLength);
        Assert.Equal(left.TotalLength + right.TotalLength, totalLength);
    }

    [Fact]
    public void SyntaxLength_CombinesBothNodesSyntaxLengths()
    {
        // Arrange
        var leftBold = BoldMarkdownV2.Apply("A");
        var rightBold = BoldMarkdownV2.Apply("B");
        var composite = leftBold.CombineWith(rightBold);

        // Act
        var syntaxLength = composite.SyntaxLength;

        // Assert
        Assert.Equal(leftBold.SyntaxLength + rightBold.SyntaxLength, syntaxLength);
        Assert.True(syntaxLength > 0);
    }

    [Fact]
    public void TotalLength_IsLazilyEvaluated()
    {
        // Arrange
        var left = new IntegerNode(123);
        var right = new IntegerNode(456);
        var composite = new CompositeNode<IntegerNode, IntegerNode>(left, right);

        // Act - First access calculates the value
        var length1 = composite.TotalLength;
        var length2 = composite.TotalLength;
        var length3 = composite.TotalLength;

        // Assert - All accesses return the same cached value
        Assert.Equal(length1, length2);
        Assert.Equal(length2, length3);
        Assert.Equal(6, length1);
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_CombinesBothNodes()
    {
        // Arrange
        var left = new PlainTextNode("Hello ");
        var right = new PlainTextNode("World");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);

        // Act
        var result = composite.ToString();

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void ToString_WithEmptyNodes_ReturnsEmptyString()
    {
        // Arrange
        var left = new PlainTextNode("");
        var right = new PlainTextNode("");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);

        // Act
        var result = composite.ToString();

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void ToString_WithNumericNodes_CombinesCorrectly()
    {
        // Arrange
        var left = new IntegerNode(123);
        var right = new FloatNode(45.6f);
        var composite = new CompositeNode<IntegerNode, FloatNode>(left, right);

        // Act
        var result = composite.ToString();

        // Assert
        Assert.Equal("12345.6", result);
    }

    #endregion

    #region Edge Cases and Boundary Tests

    [Fact]
    public void CompositeNode_WithSingleCharNodes_WorksCorrectly()
    {
        // Arrange
        var left = new PlainTextNode("A");
        var right = new PlainTextNode("B");
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);

        // Act & Assert
        Assert.Equal(2, composite.TotalLength);
        Assert.Equal("AB", composite.ToString());
        Assert.True(composite.TryGetChar(0, out var ch0));
        Assert.Equal('A', ch0);
        Assert.True(composite.TryGetChar(1, out var ch1));
        Assert.Equal('B', ch1);
    }

    [Fact]
    public void CompositeNode_WithLongNodes_WorksCorrectly()
    {
        // Arrange
        var leftText = new string('A', 1000);
        var rightText = new string('B', 1000);
        var left = new PlainTextNode(leftText);
        var right = new PlainTextNode(rightText);
        var composite = new CompositeNode<PlainTextNode, PlainTextNode>(left, right);

        // Act & Assert
        Assert.Equal(2000, composite.TotalLength);
        Assert.True(composite.TryGetChar(999, out var ch1));
        Assert.Equal('A', ch1);
        Assert.True(composite.TryGetChar(1000, out var ch2));
        Assert.Equal('B', ch2);
    }

    [Fact]
    public void CompositeNode_Nested_WorksCorrectly()
    {
        // Arrange
        var node1 = new PlainTextNode("A");
        var node2 = new PlainTextNode("B");
        var composite1 = new CompositeNode<PlainTextNode, PlainTextNode>(node1, node2);

        var node3 = new PlainTextNode("C");
        var composite2 =
            new CompositeNode<CompositeNode<PlainTextNode, PlainTextNode>, PlainTextNode>(composite1, node3);

        // Act & Assert
        Assert.Equal(3, composite2.TotalLength);
        Assert.Equal("ABC", composite2.ToString());
        Assert.True(composite2.TryGetChar(0, out var ch0));
        Assert.Equal('A', ch0);
        Assert.True(composite2.TryGetChar(1, out var ch1));
        Assert.Equal('B', ch1);
        Assert.True(composite2.TryGetChar(2, out var ch2));
        Assert.Equal('C', ch2);
    }

    #endregion
}