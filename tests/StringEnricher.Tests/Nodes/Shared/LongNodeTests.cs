using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class LongNodeTests
{
    [Fact]
    public void Constructor_WithPositiveLong_InitializesCorrectly()
    {
        // Arrange
        const long value = 123L;
        const int expectedTotalLength = 3; // "123" has 3 characters
        const int expectedSyntaxLength = 0; // LongNode has no syntax characters

        // Act
        var node = new LongNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithNegativeLong_InitializesCorrectly()
    {
        // Arrange
        const long value = -456L;
        const int expectedTotalLength = 4; // "-456" has 4 characters
        const int expectedSyntaxLength = 0; // LongNode has no syntax characters

        // Act
        var node = new LongNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithZero_InitializesCorrectly()
    {
        // Arrange
        const long value = 0L;
        const int expectedTotalLength = 1; // "0" has 1 character
        const int expectedSyntaxLength = 0; // LongNode has no syntax characters

        // Act
        var node = new LongNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(1L, 1)]
    [InlineData(12L, 2)]
    [InlineData(123L, 3)]
    [InlineData(1234L, 4)]
    [InlineData(-1L, 2)]
    [InlineData(-12L, 3)]
    [InlineData(-123L, 4)]
    [InlineData(-1234L, 5)]
    [InlineData(long.MaxValue, 19)] // 9223372036854775807
    [InlineData(long.MinValue, 20)] // -9223372036854775808
    public void TotalLength_WithVariousLongs_ReturnsCorrectLength(long value, int expectedLength)
    {
        // Arrange & Act
        var node = new LongNode(value);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithSufficientSpace_CopiesCorrectly()
    {
        // Arrange
        const long value = 123L;
        var node = new LongNode(value);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 3; // "123" has 3 characters
        const string expectedString = "123";

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithNegativeLong_CopiesCorrectly()
    {
        // Arrange
        const long value = -456L;
        var node = new LongNode(value);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 4; // "-456" has 4 characters
        const string expectedString = "-456";

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithZero_CopiesCorrectly()
    {
        // Arrange
        const long value = 0L;
        var node = new LongNode(value);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 1; // "0" has 1 character
        const string expectedString = "0";

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithInsufficientSpace_ThrowsArgumentException()
    {
        // Arrange
        const long value = 123L;
        var node = new LongNode(value);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[2]; // Too small for "123"
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("Destination span too small.", exception.Message);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        const long value = 789L;
        var node = new LongNode(value);
        Span<char> destination = stackalloc char[3]; // Exact size
        const int expectedBytesWritten = 3; // "789" has 3 characters
        const string expectedString = "789";

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const long value = 123L;
        var node = new LongNode(value);
        const string expected = "123";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = node.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TryGetChar_NegativeLong_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const long value = -456L;
        var node = new LongNode(value);
        const string expected = "-456";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = node.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3)] // "123" has indices 0, 1, 2
    [InlineData(10)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new LongNode(123L);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryGetChar_ZeroValue_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const long value = 0L;
        var node = new LongNode(value);
        const char expectedChar = '0';

        // Act
        var result = node.TryGetChar(0, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedChar, ch);
    }

    [Theory]
    [InlineData(0L, '0')]
    [InlineData(1L, '1')]
    [InlineData(9L, '9')]
    public void TryGetChar_SingleDigitNumbers_ReturnsTrueAndCorrectChar(long value, char expectedChar)
    {
        // Arrange
        var node = new LongNode(value);

        // Act
        var result = node.TryGetChar(0, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedChar, ch);
    }

    [Fact]
    public void ImplicitConversion_FromInt_CreatesLongNode()
    {
        // Arrange
        const int value = 42;
        const int expectedTotalLength = 2; // "42" has 2 characters
        const int expectedSyntaxLength = 0; // LongNode has no syntax characters

        // Act
        LongNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ImplicitConversion_FromNegativeInt_CreatesLongNode()
    {
        // Arrange
        const int value = -99;
        const int expectedTotalLength = 3; // "-99" has 3 characters
        const int expectedSyntaxLength = 0; // LongNode has no syntax characters

        // Act
        LongNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    [InlineData(1000000000000L)]
    [InlineData(-1000000000000L)]
    public void CopyTo_WithLargeNumbers_CopiesCorrectly(long value)
    {
        // Arrange
        var node = new LongNode(value);
        var expected = value.ToString();
        Span<char> destination = stackalloc char[30]; // Large enough buffer

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
    }

    [Theory]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    [InlineData(1234567890123456789L)]
    [InlineData(-987654321098765432L)]
    public void TryGetChar_WithLargeNumbers_ReturnsCorrectChars(long value)
    {
        // Arrange
        var node = new LongNode(value);
        var expected = value.ToString();

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = node.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new LongNode(0L).SyntaxLength);
        Assert.Equal(0, new LongNode(123L).SyntaxLength);
        Assert.Equal(0, new LongNode(-456L).SyntaxLength);
        Assert.Equal(0, new LongNode(long.MaxValue).SyntaxLength);
        Assert.Equal(0, new LongNode(long.MinValue).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithLongMaxValue_CopiesCorrectly()
    {
        // Arrange
        const long value = long.MaxValue; // 9223372036854775807
        var node = new LongNode(value);
        var expected = value.ToString();
        Span<char> destination = stackalloc char[30];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
        Assert.Equal(19, bytesWritten); // long.MaxValue has 19 digits
    }

    [Fact]
    public void CopyTo_WithLongMinValue_CopiesCorrectly()
    {
        // Arrange
        const long value = long.MinValue; // -9223372036854775808
        var node = new LongNode(value);
        var expected = value.ToString();
        Span<char> destination = stackalloc char[30];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
        Assert.Equal(20, bytesWritten); // long.MinValue has 20 characters (including minus sign)
    }

    [Fact]
    public void GetLongLength_EdgeCases_ReturnsCorrectLength()
    {
        // Arrange & Act & Assert
        Assert.Equal(1, new LongNode(0L).TotalLength);
        Assert.Equal(19, new LongNode(long.MaxValue).TotalLength);
        Assert.Equal(20, new LongNode(long.MinValue).TotalLength);
        Assert.Equal(10, new LongNode(1000000000L).TotalLength); // 1 billion
        Assert.Equal(11, new LongNode(-1000000000L).TotalLength); // -1 billion
    }

    [Theory]
    [InlineData(999999999999999999L, 18)] // 18 digits
    [InlineData(-999999999999999999L, 19)] // 18 digits + minus sign
    [InlineData(100000000000000000L, 18)] // 17 digits
    [InlineData(-100000000000000000L, 19)] // 17 digits + minus sign
    public void TotalLength_WithSpecificLongValues_ReturnsCorrectLength(long value, int expectedLength)
    {
        // Arrange & Act
        var node = new LongNode(value);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }
}
