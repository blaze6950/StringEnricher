using StringEnricher.Nodes.Shared;
using System.Globalization;

namespace StringEnricher.Tests.Nodes.Shared;

public class TimeSpanNodeTests
{
    private static readonly TimeSpan
        TestTimeSpan = new(1, 2, 3, 4, 500); // 1 day, 2 hours, 3 minutes, 4 seconds, 500 milliseconds

    [Fact]
    public void Constructor_WithTimeSpan_InitializesCorrectly()
    {
        // Arrange
        var value = TestTimeSpan;
        var expectedString = value.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // TimeSpanNode has no syntax characters

        // Act
        var node = new TimeSpanNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithTimeSpanAndFormat_InitializesCorrectly()
    {
        // Arrange
        var value = TestTimeSpan;
        const string format = "c";
        var expectedString = value.ToString(format, formatProvider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeSpanNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithTimeSpanFormatAndProvider_InitializesCorrectly()
    {
        // Arrange
        var value = TestTimeSpan;
        const string format = "g";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeSpanNode(value, format, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("c")] // Constant format
    [InlineData("g")] // General short format
    [InlineData("G")] // General long format
    [InlineData(@"hh\:mm\:ss")] // Custom format
    [InlineData(@"dd\.hh\:mm\:ss")] // Days, hours, minutes, seconds
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        var value = TestTimeSpan;
        var expectedString = value.ToString(format, formatProvider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new TimeSpanNode(value, format, provider: CultureInfo.InvariantCulture);

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
        var value = TestTimeSpan;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        const string format = "g"; // TimeSpan needs a format when using CultureInfo
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new TimeSpanNode(value, format, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void Constructor_WithMinValue_InitializesCorrectly()
    {
        // Arrange
        var value = TimeSpan.MinValue;
        var expectedString = value.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeSpanNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMaxValue_InitializesCorrectly()
    {
        // Arrange
        var value = TimeSpan.MaxValue;
        var expectedString = value.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeSpanNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithZero_InitializesCorrectly()
    {
        // Arrange
        var value = TimeSpan.Zero;
        var expectedString = value.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeSpanNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(1, 0, 0, 0)] // 1 day
    [InlineData(0, 1, 0, 0)] // 1 hour
    [InlineData(0, 0, 1, 0)] // 1 minute
    [InlineData(0, 0, 0, 1)] // 1 second
    [InlineData(-1, 0, 0, 0)] // Negative 1 day
    public void TotalLength_WithVariousTimeSpans_ReturnsCorrectLength(int days, int hours, int minutes, int seconds)
    {
        // Arrange
        var value = new TimeSpan(days, hours, minutes, seconds);
        var node = new TimeSpanNode(value, provider: CultureInfo.InvariantCulture);
        var expectedLength = value.ToString(null, CultureInfo.InvariantCulture).Length;

        // Act & Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithTimeSpan_CopiesCorrectly()
    {
        // Arrange
        var value = TestTimeSpan;
        var node = new TimeSpanNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[30];
        var expectedString = value.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithFormat_CopiesCorrectly()
    {
        // Arrange
        var value = TestTimeSpan;
        const string format = "c";
        var node = new TimeSpanNode(value, format, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[30];
        var expectedString = value.ToString(format, formatProvider: CultureInfo.InvariantCulture);
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
        var value = TestTimeSpan;
        const string format = "g";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new TimeSpanNode(value, format, provider);
        Span<char> destination = stackalloc char[30];
        var expectedString = value.ToString(format, provider);
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
        var value = TestTimeSpan;
        var node = new TimeSpanNode(value, provider: CultureInfo.InvariantCulture);

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
        var value = TestTimeSpan;
        var node = new TimeSpanNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);

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
    [InlineData(30)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new TimeSpanNode(TestTimeSpan, provider: CultureInfo.InvariantCulture);

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
        var value = TestTimeSpan;
        const string format = @"hh\:mm\:ss";
        var node = new TimeSpanNode(value, format, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(format, formatProvider: CultureInfo.InvariantCulture);

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
        var value = TestTimeSpan;
        const string format = "g";
        var provider = CultureInfo.GetCultureInfo("de-DE");
        var node = new TimeSpanNode(value, format, provider);
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
    public void ImplicitConversion_FromTimeSpan_CreatesTimeSpanNode()
    {
        // Arrange
        var value = TestTimeSpan;
        var expectedString = value.ToString(format: null, formatProvider: CultureInfo.CurrentCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        TimeSpanNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithTimeSpan_ReturnsCorrectString()
    {
        // Arrange
        var value = TestTimeSpan;
        var node = new TimeSpanNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithFormat_ReturnsFormattedString()
    {
        // Arrange
        var value = TestTimeSpan;
        const string format = "G";
        var node = new TimeSpanNode(value, format, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(format, formatProvider: CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithFormatAndProvider_ReturnsFormattedString()
    {
        // Arrange
        var value = TestTimeSpan;
        const string format = "c";
        var provider = CultureInfo.GetCultureInfo("fr-FR");
        var node = new TimeSpanNode(value, format, provider);
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
        Assert.Equal(0, new TimeSpanNode(TimeSpan.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new TimeSpanNode(TimeSpan.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new TimeSpanNode(TestTimeSpan, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new TimeSpanNode(TimeSpan.Zero, provider: CultureInfo.InvariantCulture).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        var value = TestTimeSpan;
        var node = new TimeSpanNode(value, provider: CultureInfo.InvariantCulture);
        var expectedString = value.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[expectedString.Length]; // Exact size

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedString.Length, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }

    [Fact]
    public void Constructor_WithNegativeTimeSpan_InitializesCorrectly()
    {
        // Arrange
        var value = new TimeSpan(-1, -2, -3, -4);
        var expectedString = value.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeSpanNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMilliseconds_InitializesCorrectly()
    {
        // Arrange
        var value = new TimeSpan(0, 1, 2, 3, 456);
        var expectedString = value.ToString(format: null, formatProvider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new TimeSpanNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }
}