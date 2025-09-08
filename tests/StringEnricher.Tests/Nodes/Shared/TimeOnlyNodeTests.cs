using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class TimeOnlyNodeTests
{
    private static readonly TimeOnly TestTime = new TimeOnly(14, 30, 45);

    [Fact]
    public void Constructor_WithTimeOnly_InitializesCorrectly()
    {
        // Arrange
        var value = TestTime;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // TimeOnlyNode has no syntax characters

        // Act
        var node = new TimeOnlyNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMinValue_InitializesCorrectly()
    {
        // Arrange
        var value = TimeOnly.MinValue;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeOnlyNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMaxValue_InitializesCorrectly()
    {
        // Arrange
        var value = TimeOnly.MaxValue;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeOnlyNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(0, 0, 0)]    // Midnight
    [InlineData(12, 0, 0)]   // Noon
    [InlineData(23, 59, 59)] // End of day
    [InlineData(9, 15, 30)]  // Morning
    public void TotalLength_WithVariousTimes_ReturnsCorrectLength(int hour, int minute, int second)
    {
        // Arrange
        var value = new TimeOnly(hour, minute, second);
        var node = new TimeOnlyNode(value);
        var expectedLength = value.ToString().Length;

        // Act & Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithTimeOnly_CopiesCorrectly()
    {
        // Arrange
        var value = TestTime;
        var node = new TimeOnlyNode(value);
        Span<char> destination = stackalloc char[20];
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
        var value = TestTime;
        var node = new TimeOnlyNode(value);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[5]; // Too small
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
        var value = TestTime;
        var node = new TimeOnlyNode(value);
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
    [InlineData(15)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new TimeOnlyNode(TestTime);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void ImplicitConversion_FromTimeOnly_CreatesTimeOnlyNode()
    {
        // Arrange
        var value = TestTime;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        TimeOnlyNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithTimeOnly_ReturnsCorrectString()
    {
        // Arrange
        var value = TestTime;
        var node = new TimeOnlyNode(value);
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
        Assert.Equal(0, new TimeOnlyNode(TimeOnly.MinValue).SyntaxLength);
        Assert.Equal(0, new TimeOnlyNode(TimeOnly.MaxValue).SyntaxLength);
        Assert.Equal(0, new TimeOnlyNode(TestTime).SyntaxLength);
        Assert.Equal(0, new TimeOnlyNode(TimeOnly.FromDateTime(DateTime.Now)).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        var value = TestTime;
        var node = new TimeOnlyNode(value);
        var expectedString = value.ToString();
        Span<char> destination = stackalloc char[expectedString.Length]; // Exact size

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedString.Length, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }

    [Fact]
    public void Constructor_WithMilliseconds_InitializesCorrectly()
    {
        // Arrange
        var value = new TimeOnly(14, 30, 45, 123);
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeOnlyNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }
}
