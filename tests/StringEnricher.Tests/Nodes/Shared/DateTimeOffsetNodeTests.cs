using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class DateTimeOffsetNodeTests
{
    private static readonly DateTimeOffset TestDateTimeOffset = new DateTimeOffset(2023, 12, 25, 14, 30, 45, TimeSpan.FromHours(-5));

    [Fact]
    public void Constructor_WithDateTimeOffset_InitializesCorrectly()
    {
        // Arrange
        var value = TestDateTimeOffset;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // DateTimeOffsetNode has no syntax characters

        // Act
        var node = new DateTimeOffsetNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMinValue_InitializesCorrectly()
    {
        // Arrange
        var value = DateTimeOffset.MinValue;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new DateTimeOffsetNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMaxValue_InitializesCorrectly()
    {
        // Arrange
        var value = DateTimeOffset.MaxValue;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new DateTimeOffsetNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(-5)] // EST
    [InlineData(0)]  // UTC
    [InlineData(1)]  // CET
    [InlineData(8)]  // PST
    public void TotalLength_WithVariousOffsets_ReturnsCorrectLength(int offsetHours)
    {
        // Arrange
        var offset = TimeSpan.FromHours(offsetHours);
        var value = new DateTimeOffset(2023, 1, 1, 12, 0, 0, offset);
        var node = new DateTimeOffsetNode(value);
        var expectedLength = value.ToString().Length;

        // Act & Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithDateTimeOffset_CopiesCorrectly()
    {
        // Arrange
        var value = TestDateTimeOffset;
        var node = new DateTimeOffsetNode(value);
        Span<char> destination = stackalloc char[50];
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
        var value = TestDateTimeOffset;
        var node = new DateTimeOffsetNode(value);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[10]; // Too small
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
        var value = TestDateTimeOffset;
        var node = new DateTimeOffsetNode(value);
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
    [InlineData(50)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new DateTimeOffsetNode(TestDateTimeOffset);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void ImplicitConversion_FromDateTimeOffset_CreatesDateTimeOffsetNode()
    {
        // Arrange
        var value = TestDateTimeOffset;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        DateTimeOffsetNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithDateTimeOffset_ReturnsCorrectString()
    {
        // Arrange
        var value = TestDateTimeOffset;
        var node = new DateTimeOffsetNode(value);
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
        Assert.Equal(0, new DateTimeOffsetNode(DateTimeOffset.MinValue).SyntaxLength);
        Assert.Equal(0, new DateTimeOffsetNode(DateTimeOffset.MaxValue).SyntaxLength);
        Assert.Equal(0, new DateTimeOffsetNode(TestDateTimeOffset).SyntaxLength);
        Assert.Equal(0, new DateTimeOffsetNode(DateTimeOffset.Now).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        var value = TestDateTimeOffset;
        var node = new DateTimeOffsetNode(value);
        var expectedString = value.ToString();
        Span<char> destination = stackalloc char[expectedString.Length]; // Exact size

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedString.Length, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }

    [Fact]
    public void Constructor_WithUtcOffset_InitializesCorrectly()
    {
        // Arrange
        var value = new DateTimeOffset(2023, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new DateTimeOffsetNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }
}
