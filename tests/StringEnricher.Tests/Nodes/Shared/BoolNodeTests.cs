using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class BoolNodeTests
{
    [Fact]
    public void Constructor_WithTrue_InitializesCorrectly()
    {
        // Arrange
        const bool value = true;
        const int expectedTotalLength = 4; // "True" has 4 characters
        const int expectedSyntaxLength = 0; // BoolNode has no syntax characters

        // Act
        var node = new BoolNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithFalse_InitializesCorrectly()
    {
        // Arrange
        const bool value = false;
        const int expectedTotalLength = 5; // "False" has 5 characters
        const int expectedSyntaxLength = 0; // BoolNode has no syntax characters

        // Act
        var node = new BoolNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(true, 4)]
    [InlineData(false, 5)]
    public void TotalLength_WithBoolValues_ReturnsCorrectLength(bool value, int expectedLength)
    {
        // Arrange & Act
        var node = new BoolNode(value);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithTrue_CopiesCorrectly()
    {
        // Arrange
        const bool value = true;
        var node = new BoolNode(value);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 4; // "True" has 4 characters
        const string expectedString = "True";

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithFalse_CopiesCorrectly()
    {
        // Arrange
        const bool value = false;
        var node = new BoolNode(value);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 5; // "False" has 5 characters
        const string expectedString = "False";

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
        const bool value = false;
        var node = new BoolNode(value);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[3]; // Too small for "False"
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
        const bool value = true;
        var node = new BoolNode(value);
        Span<char> destination = stackalloc char[4]; // Exact size for "True"
        const int expectedBytesWritten = 4;
        const string expectedString = "True";

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }

    [Fact]
    public void TryGetChar_ValidIndicesForTrue_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const bool value = true;
        var node = new BoolNode(value);
        const string expected = "True";

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = node.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TryGetChar_ValidIndicesForFalse_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const bool value = false;
        var node = new BoolNode(value);
        const string expected = "False";

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
    [InlineData(4)] // "True" has indices 0, 1, 2, 3
    [InlineData(10)]
    public void TryGetChar_OutOfRangeIndicesForTrue_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new BoolNode(true);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(5)] // "False" has indices 0, 1, 2, 3, 4
    [InlineData(10)]
    public void TryGetChar_OutOfRangeIndicesForFalse_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new BoolNode(false);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Theory]
    [InlineData(0, 'T')]
    [InlineData(1, 'r')]
    [InlineData(2, 'u')]
    [InlineData(3, 'e')]
    public void TryGetChar_TrueValue_ReturnsCorrectCharAtIndex(int index, char expectedChar)
    {
        // Arrange
        var node = new BoolNode(true);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedChar, ch);
    }

    [Theory]
    [InlineData(0, 'F')]
    [InlineData(1, 'a')]
    [InlineData(2, 'l')]
    [InlineData(3, 's')]
    [InlineData(4, 'e')]
    public void TryGetChar_FalseValue_ReturnsCorrectCharAtIndex(int index, char expectedChar)
    {
        // Arrange
        var node = new BoolNode(false);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedChar, ch);
    }

    [Fact]
    public void ImplicitConversion_FromBool_CreatesBoolNode()
    {
        // Arrange
        const bool value = true;
        const int expectedTotalLength = 4; // "True" has 4 characters
        const int expectedSyntaxLength = 0; // BoolNode has no syntax characters

        // Act
        BoolNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ImplicitConversion_FromFalseBool_CreatesBoolNode()
    {
        // Arrange
        const bool value = false;
        const int expectedTotalLength = 5; // "False" has 5 characters
        const int expectedSyntaxLength = 0; // BoolNode has no syntax characters

        // Act
        BoolNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithTrue_ReturnsTrue()
    {
        // Arrange
        var node = new BoolNode(true);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal("True", result);
    }

    [Fact]
    public void ToString_WithFalse_ReturnsFalse()
    {
        // Arrange
        var node = new BoolNode(false);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal("False", result);
    }

    [Fact]
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new BoolNode(true).SyntaxLength);
        Assert.Equal(0, new BoolNode(false).SyntaxLength);
    }
}
