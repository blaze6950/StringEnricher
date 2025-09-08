using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class CharNodeTests
{
    [Fact]
    public void Constructor_WithChar_InitializesCorrectly()
    {
        // Arrange
        const char value = 'A';
        const int expectedTotalLength = 1; // Single char has 1 character
        const int expectedSyntaxLength = 0; // CharNode has no syntax characters

        // Act
        var node = new CharNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData('A')]
    [InlineData('z')]
    [InlineData('1')]
    [InlineData(' ')]
    [InlineData('\n')]
    [InlineData('\t')]
    public void TotalLength_WithVariousChars_ReturnsOne(char value)
    {
        // Arrange & Act
        var node = new CharNode(value);

        // Assert
        Assert.Equal(1, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithChar_CopiesCorrectly()
    {
        // Arrange
        const char value = 'X';
        var node = new CharNode(value);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 1;
        const string expectedString = "X";

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
        const char value = 'A';
        var node = new CharNode(value);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[0]; // Too small
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
        const char value = 'B';
        var node = new CharNode(value);
        Span<char> destination = stackalloc char[1]; // Exact size
        const int expectedBytesWritten = 1;
        const string expectedString = "B";

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }

    [Fact]
    public void TryGetChar_ValidIndex_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const char value = 'C';
        var node = new CharNode(value);

        // Act
        var result = node.TryGetChar(0, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(value, ch);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(10)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new CharNode('D');

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Theory]
    [InlineData('A')]
    [InlineData('z')]
    [InlineData('5')]
    [InlineData('#')]
    [InlineData(' ')]
    public void TryGetChar_VariousChars_ReturnsCorrectChar(char expectedChar)
    {
        // Arrange
        var node = new CharNode(expectedChar);

        // Act
        var result = node.TryGetChar(0, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedChar, ch);
    }

    [Fact]
    public void ImplicitConversion_FromChar_CreatesCharNode()
    {
        // Arrange
        const char value = 'E';
        const int expectedTotalLength = 1;
        const int expectedSyntaxLength = 0;

        // Act
        CharNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData('F')]
    [InlineData('9')]
    [InlineData('@')]
    public void ToString_WithChar_ReturnsCharAsString(char value)
    {
        // Arrange
        var node = new CharNode(value);
        var expected = value.ToString();

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new CharNode('A').SyntaxLength);
        Assert.Equal(0, new CharNode('z').SyntaxLength);
        Assert.Equal(0, new CharNode('1').SyntaxLength);
        Assert.Equal(0, new CharNode(' ').SyntaxLength);
    }
}
