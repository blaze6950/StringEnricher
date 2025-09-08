using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class IntegerNodeTests
{
    [Fact]
    public void Constructor_WithPositiveInteger_InitializesCorrectly()
    {
        // Arrange
        const int value = 123;
        const int expectedTotalLength = 3; // "123" has 3 characters
        const int expectedSyntaxLength = 0; // IntegerNode has no syntax characters

        // Act
        var node = new IntegerNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithNegativeInteger_InitializesCorrectly()
    {
        // Arrange
        const int value = -456;
        const int expectedTotalLength = 4; // "-456" has 4 characters
        const int expectedSyntaxLength = 0; // IntegerNode has no syntax characters

        // Act
        var node = new IntegerNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithZero_InitializesCorrectly()
    {
        // Arrange
        const int value = 0;
        const int expectedTotalLength = 1; // "0" has 1 character
        const int expectedSyntaxLength = 0; // IntegerNode has no syntax characters

        // Act
        var node = new IntegerNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(12, 2)]
    [InlineData(123, 3)]
    [InlineData(1234, 4)]
    [InlineData(-1, 2)]
    [InlineData(-12, 3)]
    [InlineData(-123, 4)]
    [InlineData(-1234, 5)]
    [InlineData(int.MaxValue, 10)] // 2147483647
    [InlineData(int.MinValue, 11)] // -2147483648
    public void TotalLength_WithVariousIntegers_ReturnsCorrectLength(int value, int expectedLength)
    {
        // Arrange & Act
        var node = new IntegerNode(value);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithSufficientSpace_CopiesCorrectly()
    {
        // Arrange
        const int value = 123;
        var node = new IntegerNode(value);
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
    public void CopyTo_WithNegativeInteger_CopiesCorrectly()
    {
        // Arrange
        const int value = -456;
        var node = new IntegerNode(value);
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
        const int value = 0;
        var node = new IntegerNode(value);
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
        const int value = 123;
        var node = new IntegerNode(value);

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
        const int value = 789;
        var node = new IntegerNode(value);
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
        const int value = 123;
        var node = new IntegerNode(value);
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
    public void TryGetChar_NegativeInteger_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const int value = -456;
        var node = new IntegerNode(value);
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
        var node = new IntegerNode(123);

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
        const int value = 0;
        var node = new IntegerNode(value);
        const char expectedChar = '0';

        // Act
        var result = node.TryGetChar(0, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedChar, ch);
    }

    [Theory]
    [InlineData(0, '0')]
    [InlineData(1, '1')]
    [InlineData(9, '9')]
    public void TryGetChar_SingleDigitNumbers_ReturnsTrueAndCorrectChar(int value, char expectedChar)
    {
        // Arrange
        var node = new IntegerNode(value);

        // Act
        var result = node.TryGetChar(0, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedChar, ch);
    }

    [Fact]
    public void ImplicitConversion_FromInt_CreatesIntegerNode()
    {
        // Arrange
        const int value = 42;
        const int expectedTotalLength = 2; // "42" has 2 characters
        const int expectedSyntaxLength = 0; // IntegerNode has no syntax characters

        // Act
        IntegerNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ImplicitConversion_FromNegativeInt_CreatesIntegerNode()
    {
        // Arrange
        const int value = -99;
        const int expectedTotalLength = 3; // "-99" has 3 characters
        const int expectedSyntaxLength = 0; // IntegerNode has no syntax characters

        // Act
        IntegerNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(1000000)]
    [InlineData(-1000000)]
    public void CopyTo_WithLargeNumbers_CopiesCorrectly(int value)
    {
        // Arrange
        var node = new IntegerNode(value);
        var expected = value.ToString();
        Span<char> destination = stackalloc char[20]; // Large enough buffer

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
    }

    [Theory]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(1234567890)]
    [InlineData(-987654321)]
    public void TryGetChar_WithLargeNumbers_ReturnsCorrectChars(int value)
    {
        // Arrange
        var node = new IntegerNode(value);
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
        Assert.Equal(0, new IntegerNode(0).SyntaxLength);
        Assert.Equal(0, new IntegerNode(123).SyntaxLength);
        Assert.Equal(0, new IntegerNode(-456).SyntaxLength);
        Assert.Equal(0, new IntegerNode(int.MaxValue).SyntaxLength);
        Assert.Equal(0, new IntegerNode(int.MinValue).SyntaxLength);
    }
}