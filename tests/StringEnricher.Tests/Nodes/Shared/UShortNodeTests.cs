using StringEnricher.Nodes.Shared;
using System.Globalization;
using StringEnricher.Debug;

namespace StringEnricher.Tests.Nodes.Shared;

public class UShortNodeTests
{
    private const ushort TestUShort = 12345;

    [Fact]
    public void Constructor_WithPositiveUShort_InitializesCorrectly()
    {
        // Arrange
        const ushort value = 123;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "123" has 3 characters
        const int expectedSyntaxLength = 0; // UShortNode has no syntax characters

        // Act
        var node = new UShortNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithUShortAndFormat_InitializesCorrectly()
    {
        // Arrange
        const ushort value = TestUShort;
        const string format = "N0";
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new UShortNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithUShortFormatAndProvider_InitializesCorrectly()
    {
        // Arrange
        const ushort value = TestUShort;
        const string format = "X4";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new UShortNode(value, format, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("D")] // Decimal
    [InlineData("N")] // Number with thousands separator
    [InlineData("X")] // Hexadecimal uppercase
    [InlineData("x")] // Hexadecimal lowercase
    [InlineData("D5")] // Decimal with padding
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        const ushort value = TestUShort;
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new UShortNode(value, format, provider: CultureInfo.InvariantCulture);

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
        const ushort value = TestUShort;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = value.ToString(provider);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new UShortNode(value, null, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void Constructor_WithZero_InitializesCorrectly()
    {
        // Arrange
        const ushort value = 0;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "0" has 1 character
        const int expectedSyntaxLength = 0; // UShortNode has no syntax characters

        // Act
        var node = new UShortNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(12, 2)]
    [InlineData(123, 3)]
    [InlineData(1234, 4)]
    [InlineData(12345, 5)]
    [InlineData(ushort.MaxValue, 5)] // 65535
    [InlineData(ushort.MinValue, 1)] // 0
    public void TotalLength_WithVariousUShorts_ReturnsCorrectLength(ushort value, int expectedLength)
    {
        // Arrange & Act
        var node = new UShortNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithFormat_CopiesCorrectly()
    {
        // Arrange
        const ushort value = TestUShort;
        const string format = "N0";
        var node = new UShortNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const ushort value = TestUShort;
        const string format = "X4";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new UShortNode(value, format, provider);
        Span<char> destination = stackalloc char[10];
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
        const ushort value = 12345;
        var node = new UShortNode(value, provider: CultureInfo.InvariantCulture);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[4]; // Too small for "12345"
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Contains("The destination span is too small to hold the ushort value.", exception.Message);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const ushort value = 123;
        var node = new UShortNode(value, provider: CultureInfo.InvariantCulture);
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
        const ushort value = TestUShort;
        const string format = "X";
        var node = new UShortNode(value, format, provider: CultureInfo.InvariantCulture);
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
        var node = new UShortNode(123, provider: CultureInfo.InvariantCulture);

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
        var node = new UShortNode(123, provider: CultureInfo.InvariantCulture);
        var totalLength = node.TotalLength;
        DebugCounters.ResetAllCounters();

        // Act
        var result = node.TryGetChar(totalLength, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
        Assert.Equal(1, DebugCounters.UShortNode_TryGetChar_CachedTotalLengthEvaluation);
    }

    [Fact]
    public void ImplicitConversion_FromUShort_CreatesUShortNode()
    {
        // Arrange
        const ushort value = 42;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length; // "42" has 2 characters
        const int expectedSyntaxLength = 0; // UShortNode has no syntax characters

        // Act
        UShortNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(ushort.MaxValue)] // 65535
    [InlineData(ushort.MinValue)] // 0
    [InlineData(32768)]
    [InlineData(50000)]
    public void CopyTo_WithEdgeValues_CopiesCorrectly(ushort value)
    {
        // Arrange
        var node = new UShortNode(value, provider: CultureInfo.InvariantCulture);
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
        Assert.Equal(0, new UShortNode(0, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new UShortNode(123, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new UShortNode(ushort.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new UShortNode(ushort.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithUShortMaxValue_CopiesCorrectly()
    {
        // Arrange
        const ushort value = ushort.MaxValue; // 65535
        var node = new UShortNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
        Assert.Equal(5, bytesWritten); // ushort.MaxValue has 5 digits
    }

    [Fact]
    public void CopyTo_WithUShortMinValue_CopiesCorrectly()
    {
        // Arrange
        const ushort value = ushort.MinValue; // 0
        var node = new UShortNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
        Assert.Equal(1, bytesWritten); // ushort.MinValue "0" has 1 character
    }

    [Theory]
    [InlineData(65534, 5)] // 5 digits
    [InlineData(50000, 5)] // 5 digits  
    [InlineData(10000, 5)] // 5 digits
    [InlineData(1000, 4)] // 4 digits
    [InlineData(100, 3)] // 3 digits
    [InlineData(10, 2)] // 2 digits
    [InlineData(1, 1)] // 1 digit
    public void TotalLength_WithSpecificUShortValues_ReturnsCorrectLength(ushort value, int expectedLength)
    {
        // Arrange & Act
        var node = new UShortNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    #region ISpanFormattable Tests

    [Fact]
    public void ToString_WithFormatParameter_OverridesNodeFormat()
    {
        // Arrange
        const ushort value = 1234;
        var node = new UShortNode(value, "D", CultureInfo.InvariantCulture);
        var expected = value.ToString("X", CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString("X", CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const ushort value = 1234;
        var node = new UShortNode(value, provider: CultureInfo.InvariantCulture);
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
        const ushort value = 255;
        var node = new UShortNode(value, "D", CultureInfo.InvariantCulture);
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
        const ushort value = 1234;
        var node = new UShortNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[2];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TotalLength_IsCachedAfterFirstAccess()
    {
        // Arrange
        var node = new UShortNode(1234, "N0", CultureInfo.InvariantCulture);
        
        // Act
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;

        // Assert
        Assert.Equal(length1, length2);
    }

    #endregion
}
