using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class FloatNodeTests
{
    [Fact]
    public void Constructor_WithPositiveFloat_InitializesCorrectly()
    {
        // Arrange
        const float value = 123.45f;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // FloatNode has no syntax characters

        // Act
        var node = new FloatNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithNegativeFloat_InitializesCorrectly()
    {
        // Arrange
        const float value = -456.78f;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // FloatNode has no syntax characters

        // Act
        var node = new FloatNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithZero_InitializesCorrectly()
    {
        // Arrange
        const float value = 0.0f;
        const int expectedTotalLength = 1; // "0" has 1 character
        const int expectedSyntaxLength = 0; // FloatNode has no syntax characters

        // Act
        var node = new FloatNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(1.0f)]
    [InlineData(12.34f)]
    [InlineData(123.456f)]
    [InlineData(-1.0f)]
    [InlineData(-12.34f)]
    [InlineData(-123.456f)]
    public void TotalLength_WithVariousFloats_ReturnsCorrectLength(float value)
    {
        // Arrange & Act
        var node = new FloatNode(value);
        var expectedLength = value.ToString().Length;

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithFloat_CopiesCorrectly()
    {
        // Arrange
        const float value = 123.45f;
        var node = new FloatNode(value);
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
        const float value = 123.45f;
        var node = new FloatNode(value);

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
        const float value = 123.45f;
        var node = new FloatNode(value);
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
        var node = new FloatNode(123.45f);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void ImplicitConversion_FromFloat_CreatesFloatNode()
    {
        // Arrange
        const float value = 42.5f;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        FloatNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithFloat_ReturnsCorrectString()
    {
        // Arrange
        const float value = 789.123f;
        var node = new FloatNode(value);
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
        Assert.Equal(0, new FloatNode(0.0f).SyntaxLength);
        Assert.Equal(0, new FloatNode(123.45f).SyntaxLength);
        Assert.Equal(0, new FloatNode(-456.78f).SyntaxLength);
        Assert.Equal(0, new FloatNode(float.MaxValue).SyntaxLength);
        Assert.Equal(0, new FloatNode(float.MinValue).SyntaxLength);
    }

    [Theory]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.NaN)]
    public void SpecialFloatValues_HandleCorrectly(float value)
    {
        // Arrange & Act
        var node = new FloatNode(value);
        var expectedString = value.ToString();

        // Assert
        Assert.Equal(expectedString.Length, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }
}
