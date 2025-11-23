using StringEnricher.Nodes.Shared;
using System.Globalization;
using StringEnricher.Debug;

namespace StringEnricher.Tests.Nodes.Shared;

public class UIntegerNodeTests
{
    private const uint TestUInteger = 1234567890U;

    [Fact]
    public void Constructor_WithPositiveUInteger_InitializesCorrectly()
    {
        // Arrange
        const uint value = 123U;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "123" has 3 characters
        const int expectedSyntaxLength = 0; // UIntegerNode has no syntax characters

        // Act
        var node = new UIntegerNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithUIntegerAndFormat_InitializesCorrectly()
    {
        // Arrange
        const uint value = TestUInteger;
        const string format = "N0";
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new UIntegerNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithUIntegerFormatAndProvider_InitializesCorrectly()
    {
        // Arrange
        const uint value = TestUInteger;
        const string format = "X8";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new UIntegerNode(value, format, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("D")] // Decimal
    [InlineData("N")] // Number with thousands separator
    [InlineData("X")] // Hexadecimal uppercase
    [InlineData("x")] // Hexadecimal lowercase
    [InlineData("D10")] // Decimal with padding
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        const uint value = TestUInteger;
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new UIntegerNode(value, format, provider: CultureInfo.InvariantCulture);

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
        const uint value = TestUInteger;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = value.ToString(provider);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new UIntegerNode(value, null, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void Constructor_WithZero_InitializesCorrectly()
    {
        // Arrange
        const uint value = 0U;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "0" has 1 character
        const int expectedSyntaxLength = 0; // UIntegerNode has no syntax characters

        // Act
        var node = new UIntegerNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(1U, 1)]
    [InlineData(12U, 2)]
    [InlineData(123U, 3)]
    [InlineData(1234U, 4)]
    [InlineData(12345U, 5)]
    [InlineData(123456U, 6)]
    [InlineData(1234567U, 7)]
    [InlineData(12345678U, 8)]
    [InlineData(123456789U, 9)]
    [InlineData(1234567890U, 10)]
    [InlineData(uint.MaxValue, 10)] // 4294967295
    [InlineData(uint.MinValue, 1)] // 0
    public void TotalLength_WithVariousUIntegers_ReturnsCorrectLength(uint value, int expectedLength)
    {
        // Arrange & Act
        var node = new UIntegerNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithFormat_CopiesCorrectly()
    {
        // Arrange
        const uint value = TestUInteger;
        const string format = "N0";
        var node = new UIntegerNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const uint value = TestUInteger;
        const string format = "X8";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new UIntegerNode(value, format, provider);
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
    public void CopyTo_WithInsufficientSpace_ThrowsArgumentException()
    {
        // Arrange
        const uint value = 1234567890U;
        var node = new UIntegerNode(value, provider: CultureInfo.InvariantCulture);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[9]; // Too small for "1234567890"
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Contains("The destination span is too small to hold the unsigned integer value.", exception.Message);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const uint value = 123U;
        var node = new UIntegerNode(value, provider: CultureInfo.InvariantCulture);
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
        const uint value = TestUInteger;
        const string format = "X";
        var node = new UIntegerNode(value, format, provider: CultureInfo.InvariantCulture);
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
        var node = new UIntegerNode(123U, provider: CultureInfo.InvariantCulture);

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
        var node = new UIntegerNode(123U, provider: CultureInfo.InvariantCulture);
        var totalLength = node.TotalLength;
        DebugCounters.ResetAllCounters();

        // Act
        var result = node.TryGetChar(totalLength, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
        Assert.Equal(1, DebugCounters.UIntegerNode_TryGetChar_CachedTotalLengthEvaluation);
    }

    [Fact]
    public void ImplicitConversion_FromUInteger_CreatesUIntegerNode()
    {
        // Arrange
        const uint value = 42U;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "42" has 2 characters
        const int expectedSyntaxLength = 0; // UIntegerNode has no syntax characters

        // Act
        UIntegerNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(uint.MaxValue)] // 4294967295
    [InlineData(uint.MinValue)] // 0
    [InlineData(2147483648U)] // 2^31
    [InlineData(3000000000U)]
    public void CopyTo_WithEdgeValues_CopiesCorrectly(uint value)
    {
        // Arrange
        var node = new UIntegerNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[15]; // Large enough buffer

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
        Assert.Equal(0, new UIntegerNode(0U, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new UIntegerNode(123U, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new UIntegerNode(uint.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new UIntegerNode(uint.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithUIntegerMaxValue_CopiesCorrectly()
    {
        // Arrange
        const uint value = uint.MaxValue; // 4294967295
        var node = new UIntegerNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[15];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
        Assert.Equal(10, bytesWritten); // uint.MaxValue has 10 digits
    }

    [Fact]
    public void CopyTo_WithUIntegerMinValue_CopiesCorrectly()
    {
        // Arrange
        const uint value = uint.MinValue; // 0
        var node = new UIntegerNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
        Assert.Equal(1, bytesWritten); // uint.MinValue "0" has 1 character
    }

    [Theory]
    [InlineData(4294967294U, 10)] // 10 digits
    [InlineData(1000000000U, 10)] // 10 digits  
    [InlineData(100000000U, 9)] // 9 digits
    [InlineData(10000000U, 8)] // 8 digits
    [InlineData(1000000U, 7)] // 7 digits
    [InlineData(100000U, 6)] // 6 digits
    [InlineData(10000U, 5)] // 5 digits
    [InlineData(1000U, 4)] // 4 digits
    [InlineData(100U, 3)] // 3 digits
    [InlineData(10U, 2)] // 2 digits
    [InlineData(1U, 1)] // 1 digit
    public void TotalLength_WithSpecificUIntegerValues_ReturnsCorrectLength(uint value, int expectedLength)
    {
        // Arrange & Act
        var node = new UIntegerNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    #region ISpanFormattable Tests - ToString with Format Override

    [Fact]
    public void ToString_WithFormatParameter_OverridesNodeFormat()
    {
        // Arrange
        const uint value = 12345U;
        var node = new UIntegerNode(value, "D", CultureInfo.InvariantCulture);
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
        const uint value = 12345U;
        var nodeProvider = CultureInfo.GetCultureInfo("en-US");
        var overrideProvider = CultureInfo.GetCultureInfo("fr-FR");
        var node = new UIntegerNode(value, "N0", nodeProvider);
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
        const uint value = 12345U;
        const string nodeFormat = "X8";
        var node = new UIntegerNode(value, nodeFormat, CultureInfo.InvariantCulture);
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
        const uint value = 12345U;
        var node = new UIntegerNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[20];
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
        const uint value = 255U;
        var node = new UIntegerNode(value, "D", CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[20];
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
        const uint value = 12345U;
        var node = new UIntegerNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[3];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Theory]
    [InlineData("D8", "00012345")]
    [InlineData("X", "3039")]
    [InlineData("N0", "12,345")]
    public void TryFormat_WithVariousFormats_FormatsCorrectly(string format, string expected)
    {
        // Arrange
        const uint value = 12345U;
        var node = new UIntegerNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[50];

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
        var node = new UIntegerNode(12345U, "N0", CultureInfo.InvariantCulture);
        
        // Act
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;

        // Assert
        Assert.Equal(length1, length2);
    }

    #endregion
}