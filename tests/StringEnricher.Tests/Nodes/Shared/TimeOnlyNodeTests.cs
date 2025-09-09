using StringEnricher.Nodes.Shared;
using System.Globalization;

namespace StringEnricher.Tests.Nodes.Shared;

public class TimeOnlyNodeTests
{
    private static readonly TimeOnly TestTimeOnly = new(14, 30, 45, 123);

    [Fact]
    public void Constructor_WithTimeOnly_InitializesCorrectly()
    {
        // Arrange
        var value = TestTimeOnly;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // TimeOnlyNode has no syntax characters

        // Act
        var node = new TimeOnlyNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithTimeOnlyAndFormat_InitializesCorrectly()
    {
        // Arrange
        var value = TestTimeOnly;
        const string format = "HH:mm:ss";
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeOnlyNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithTimeOnlyFormatAndProvider_InitializesCorrectly()
    {
        // Arrange
        var value = TestTimeOnly;
        const string format = "h:mm:ss tt";
        var provider = CultureInfo.GetCultureInfo("en-US");
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeOnlyNode(value, format, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("HH:mm:ss")] // 24-hour format
    [InlineData("h:mm:ss tt")] // 12-hour format with AM/PM
    [InlineData("HH:mm")] // Hours and minutes only
    [InlineData("t")] // Short time pattern
    [InlineData("T")] // Long time pattern
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        var value = TestTimeOnly;
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new TimeOnlyNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("en-GB")]
    [InlineData("fr-FR")]
    [InlineData("de-DE")]
    [InlineData("ja-JP")]
    public void Constructor_WithVariousProviders_InitializesCorrectly(string cultureName)
    {
        // Arrange
        var value = TestTimeOnly;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = value.ToString(provider);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new TimeOnlyNode(value, null, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Theory]
    [InlineData(0, 0, 0)] // Midnight
    [InlineData(12, 0, 0)] // Noon
    [InlineData(23, 59, 59)] // End of day
    [InlineData(9, 15, 30)] // Morning
    public void TotalLength_WithVariousTimes_ReturnsCorrectLength(int hour, int minute, int second)
    {
        // Arrange
        var value = new TimeOnly(hour, minute, second);
        var node = new TimeOnlyNode(value, provider: CultureInfo.InvariantCulture);
        var expectedLength = value.ToString(CultureInfo.InvariantCulture).Length;

        // Act & Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithTimeOnly_CopiesCorrectly()
    {
        // Arrange
        var value = TestTimeOnly;
        var node = new TimeOnlyNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[20];
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
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
        var value = TestTimeOnly;
        var node = new TimeOnlyNode(value, provider: CultureInfo.InvariantCulture);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[4]; // Too small
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("Destination span too small.", exception.Message);
    }

    [Fact]
    public void CopyTo_WithFormat_CopiesCorrectly()
    {
        // Arrange
        var value = TestTimeOnly;
        const string format = "HH:mm:ss";
        var node = new TimeOnlyNode(value, format, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[20];
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithFormatAndProvider_CopiesCorrectly()
    {
        // Arrange
        var value = TestTimeOnly;
        const string format = "h:mm:ss tt";
        var provider = CultureInfo.GetCultureInfo("en-US");
        var node = new TimeOnlyNode(value, format, provider);
        Span<char> destination = stackalloc char[20];
        var expectedString = value.ToString(format, provider);
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var value = TestTimeOnly;
        var node = new TimeOnlyNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);

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
        var node = new TimeOnlyNode(TestTimeOnly, provider: CultureInfo.InvariantCulture);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryGetChar_WithFormat_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var value = TestTimeOnly;
        const string format = "HH:mm";
        var node = new TimeOnlyNode(value, format, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(format, provider: CultureInfo.InvariantCulture);

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = node.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TryGetChar_WithFormatAndProvider_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var value = TestTimeOnly;
        const string format = "t";
        var provider = CultureInfo.GetCultureInfo("en-US");
        var node = new TimeOnlyNode(value, format, provider);
        var expected = value.ToString(format, provider);

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = node.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void ImplicitConversion_FromTimeOnly_CreatesTimeOnlyNode()
    {
        // Arrange
        var value = TestTimeOnly;
        var expectedString = value.ToString(CultureInfo.CurrentCulture);
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
        var value = TestTimeOnly;
        var node = new TimeOnlyNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithFormat_ReturnsFormattedString()
    {
        // Arrange
        var value = TestTimeOnly;
        const string format = "T";
        var node = new TimeOnlyNode(value, format, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(format, provider: CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithFormatAndProvider_ReturnsFormattedString()
    {
        // Arrange
        var value = TestTimeOnly;
        const string format = "h:mm tt";
        var provider = CultureInfo.GetCultureInfo("en-US");
        var node = new TimeOnlyNode(value, format, provider);
        var expected = value.ToString(format, provider);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new TimeOnlyNode(TimeOnly.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new TimeOnlyNode(TimeOnly.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new TimeOnlyNode(TestTimeOnly, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0,
            new TimeOnlyNode(TimeOnly.FromDateTime(DateTime.Now), provider: CultureInfo.InvariantCulture).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        var value = TestTimeOnly;
        var node = new TimeOnlyNode(value, provider: CultureInfo.InvariantCulture);
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
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
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeOnlyNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }
}