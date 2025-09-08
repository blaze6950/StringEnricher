using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class DoubleNodeTests
{
    [Fact]
    public void Constructor_WithPositiveDouble_InitializesCorrectly()
    {
        // Arrange
        const double value = 123.45;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // DoubleNode has no syntax characters

        // Act
        var node = new DoubleNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithNegativeDouble_InitializesCorrectly()
    {
        // Arrange
        const double value = -456.78;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // DoubleNode has no syntax characters

        // Act
        var node = new DoubleNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithZero_InitializesCorrectly()
    {
        // Arrange
        const double value = 0.0;
        const int expectedTotalLength = 1; // "0" has 1 character
        const int expectedSyntaxLength = 0; // DoubleNode has no syntax characters

        // Act
        var node = new DoubleNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(12.34)]
    [InlineData(123.456)]
    [InlineData(-1.0)]
    [InlineData(-12.34)]
    [InlineData(-123.456)]
    public void TotalLength_WithVariousDoubles_ReturnsCorrectLength(double value)
    {
        // Arrange & Act
        var node = new DoubleNode(value);
        var expectedLength = value.ToString().Length;

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithDouble_CopiesCorrectly()
    {
        // Arrange
        const double value = 123.45;
        var node = new DoubleNode(value);
        Span<char> destination = stackalloc char[30];
        var expectedString = value.ToString();
        var expectedBytesWritten = expectedString.Length;

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
        const double value = 123.45;
        var node = new DoubleNode(value);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[2]; // Too small
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("Destination span too small.", exception.Message);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const double value = 123.45;
        var node = new DoubleNode(value);
        var expected = value.ToString();

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
    [InlineData(20)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new DoubleNode(123.45);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void ImplicitConversion_FromDouble_CreatesDoubleNode()
    {
        // Arrange
        const double value = 42.5;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        DoubleNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithDouble_ReturnsCorrectString()
    {
        // Arrange
        const double value = 789.123;
        var node = new DoubleNode(value);
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
        Assert.Equal(0, new DoubleNode(0.0).SyntaxLength);
        Assert.Equal(0, new DoubleNode(123.45).SyntaxLength);
        Assert.Equal(0, new DoubleNode(-456.78).SyntaxLength);
        Assert.Equal(0, new DoubleNode(double.MaxValue).SyntaxLength);
        Assert.Equal(0, new DoubleNode(double.MinValue).SyntaxLength);
    }

    [Theory]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(double.NaN)]
    public void SpecialDoubleValues_HandleCorrectly(double value)
    {
        // Arrange & Act
        var node = new DoubleNode(value);
        var expectedString = value.ToString();

        // Assert
        Assert.Equal(expectedString.Length, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }
}
