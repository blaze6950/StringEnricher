using StringEnricher.Nodes.Shared;
using System.Globalization;
using StringEnricher.Debug;

namespace StringEnricher.Tests.Nodes.Shared;

public class ShortNodeTests
{
    private const short TestShort = 12345;

    [Fact]
    public void Constructor_WithPositiveShort_InitializesCorrectly()
    {
        // Arrange
        const short value = 123;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "123" has 3 characters
        const int expectedSyntaxLength = 0; // ShortNode has no syntax characters

        // Act
        var node = new ShortNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithShortAndFormat_InitializesCorrectly()
    {
        // Arrange
        const short value = TestShort;
        const string format = "N0";
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new ShortNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithShortFormatAndProvider_InitializesCorrectly()
    {
        // Arrange
        const short value = TestShort;
        const string format = "C";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new ShortNode(value, format, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("D")] // Decimal
    [InlineData("N")] // Number with thousands separator
    [InlineData("X")] // Hexadecimal uppercase
    [InlineData("x")] // Hexadecimal lowercase
    [InlineData("C")] // Currency
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        const short value = TestShort;
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new ShortNode(value, format, provider: CultureInfo.InvariantCulture);

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
        const short value = TestShort;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = value.ToString(provider);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new ShortNode(value, null, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void Constructor_WithNegativeShort_InitializesCorrectly()
    {
        // Arrange
        const short value = -456;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "-456" has 4 characters
        const int expectedSyntaxLength = 0; // ShortNode has no syntax characters

        // Act
        var node = new ShortNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithZero_InitializesCorrectly()
    {
        // Arrange
        const short value = 0;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "0" has 1 character
        const int expectedSyntaxLength = 0; // ShortNode has no syntax characters

        // Act
        var node = new ShortNode(value);

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
    [InlineData(short.MaxValue, 5)] // 32767
    [InlineData(short.MinValue, 6)] // -32768
    public void TotalLength_WithVariousShorts_ReturnsCorrectLength(short value, int expectedLength)
    {
        // Arrange & Act
        var node = new ShortNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithFormat_CopiesCorrectly()
    {
        // Arrange
        const short value = TestShort;
        const string format = "N0";
        var node = new ShortNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const short value = TestShort;
        const string format = "C";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new ShortNode(value, format, provider);
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
    public void CopyTo_WithNegativeShort_CopiesCorrectly()
    {
        // Arrange
        const short value = -456;
        var node = new ShortNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 4; // "-456" has 4 characters
        var expectedString = value.ToString(CultureInfo.InvariantCulture);

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
        const short value = 123;
        var node = new ShortNode(value, provider: CultureInfo.InvariantCulture);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[2]; // Too small for "123"
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Contains("The destination span is too small to hold the short value.", exception.Message);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const short value = 123;
        var node = new ShortNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);

        // Act & Assert
        for (var i = 0; i < expected.Length; i++)
        {
            var result = node.TryGetChar(i, out var ch);
            Assert.True(result);
            Assert.Equal(expected[i], ch);
        }
    }

    [Fact]
    public void TryGetChar_WithFormat_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const short value = TestShort;
        const string format = "X";
        var node = new ShortNode(value, format, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(format, provider: CultureInfo.InvariantCulture);

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
        var node = new ShortNode(123, provider: CultureInfo.InvariantCulture);

        // Act
        var result = node.TryGetChar(index, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
    }

    [Fact]
    public void TryGetChar_OutOfRightRangeIndexWhenTotalLengthWasCalculated_ReturnsFalseAndNullChar()
    {
        // Arrange
        var node = new ShortNode(123, provider: CultureInfo.InvariantCulture);
        var totalLength = node.TotalLength;
        DebugCounters.ResetAllCounters();

        // Act
        var result = node.TryGetChar(totalLength, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
        Assert.Equal(1, DebugCounters.ShortNode_TryGetChar_CachedTotalLengthEvaluation);
    }

    [Fact]
    public void ImplicitConversion_FromShort_CreatesShortNode()
    {
        // Arrange
        const short value = 42;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "42" has 2 characters
        const int expectedSyntaxLength = 0; // ShortNode has no syntax characters

        // Act
        ShortNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(short.MaxValue)] // 32767
    [InlineData(short.MinValue)] // -32768
    [InlineData(10000)]
    [InlineData(-10000)]
    public void CopyTo_WithEdgeValues_CopiesCorrectly(short value)
    {
        // Arrange
        var node = new ShortNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10]; // Large enough buffer

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new ShortNode(0, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new ShortNode(123, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new ShortNode(-456, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new ShortNode(short.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new ShortNode(short.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithShortMaxValue_CopiesCorrectly()
    {
        // Arrange
        const short value = short.MaxValue; // 32767
        var node = new ShortNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
        Assert.Equal(5, bytesWritten); // short.MaxValue has 5 digits
    }

    [Fact]
    public void CopyTo_WithShortMinValue_CopiesCorrectly()
    {
        // Arrange
        const short value = short.MinValue; // -32768
        var node = new ShortNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
        Assert.Equal(6, bytesWritten); // short.MinValue has 6 characters (including minus sign)
    }

    [Theory]
    [InlineData(32766, 5)] // 5 digits
    [InlineData(10000, 5)] // 5 digits  
    [InlineData(1000, 4)] // 4 digits
    [InlineData(100, 3)] // 3 digits
    [InlineData(-32767, 6)] // 6 characters including minus
    [InlineData(-10000, 6)] // 6 characters including minus
    [InlineData(-1000, 5)] // 5 characters including minus
    [InlineData(-100, 4)] // 4 characters including minus
    public void TotalLength_WithSpecificShortValues_ReturnsCorrectLength(short value, int expectedLength)
    {
        // Arrange & Act
        var node = new ShortNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    #region ISpanFormattable Tests - ToString with Format Override

    [Fact]
    public void ToString_WithFormatParameter_OverridesNodeFormat()
    {
        // Arrange
        const short value = 1234;
        var node = new ShortNode(value, "D", CultureInfo.InvariantCulture);
        var expected = value.ToString("X", CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString("X", CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithProviderParameter_OverridesNodeProvider()
    {
        // Arrange
        const short value = 1234;
        var nodeProvider = CultureInfo.GetCultureInfo("en-US");
        var overrideProvider = CultureInfo.GetCultureInfo("fr-FR");
        var node = new ShortNode(value, "N0", nodeProvider);
        var expected = value.ToString("N0", overrideProvider);

        // Act
        var result = node.ToString(null, overrideProvider);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithNullFormat_UsesNodeFormat()
    {
        // Arrange
        const short value = 1234;
        const string nodeFormat = "X4";
        var node = new ShortNode(value, nodeFormat, CultureInfo.InvariantCulture);
        var expected = value.ToString(nodeFormat, CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString(null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region ISpanFormattable Tests - TryFormat

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const short value = 1234;
        var node = new ShortNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];
        var expected = value.ToString(CultureInfo.InvariantCulture);

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithFormatOverride_UsesOverrideFormat()
    {
        // Arrange
        const short value = 255;
        var node = new ShortNode(value, "D", CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];
        var expected = value.ToString("X", CultureInfo.InvariantCulture);

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "X".AsSpan(), CultureInfo.InvariantCulture);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        const short value = 1234;
        var node = new ShortNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[2];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Theory]
    [InlineData("D6", "001234")]
    [InlineData("X", "4D2")]
    [InlineData("N0", "1,234")]
    public void TryFormat_WithVariousFormats_FormatsCorrectly(string format, string expected)
    {
        // Arrange
        const short value = 1234;
        var node = new ShortNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[20];

        // Act
        var success = node.TryFormat(destination, out var charsWritten, format.AsSpan(), CultureInfo.InvariantCulture);

        // Assert
        Assert.True(success);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    #endregion

    #region Length Caching Tests

    [Fact]
    public void TotalLength_IsCachedAfterFirstAccess()
    {
        // Arrange
        var node = new ShortNode(1234, "N0", CultureInfo.InvariantCulture);
        
        // Act
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;

        // Assert
        Assert.Equal(length1, length2);
    }

    #endregion
}
