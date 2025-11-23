using StringEnricher.Nodes.Shared;
using System.Globalization;
using StringEnricher.Debug;

namespace StringEnricher.Tests.Nodes.Shared;

public class IntegerNodeTests
{
    private const int TestInteger = 12345;

    [Fact]
    public void Constructor_WithPositiveInteger_InitializesCorrectly()
    {
        // Arrange
        const int value = 123;
        const int expectedTotalLength = 3; // "123" has 3 characters
        const int expectedSyntaxLength = 0; // IntegerNode has no syntax characters

        // Act
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithIntegerAndFormat_InitializesCorrectly()
    {
        // Arrange
        const int value = TestInteger;
        const string format = "N0";
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new IntegerNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithIntegerFormatAndProvider_InitializesCorrectly()
    {
        // Arrange
        const int value = TestInteger;
        const string format = "C";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new IntegerNode(value, format, provider);

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
        const int value = TestInteger;
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new IntegerNode(value, format, provider: CultureInfo.InvariantCulture);

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
        const int value = TestInteger;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = value.ToString(provider);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new IntegerNode(value, null, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void TotalLength_WithVariousIntegers_ReturnsCorrectLength()
    {
        // Arrange & Act
        var node = new IntegerNode(TestInteger, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(5, node.TotalLength); // "12345" has 5 characters
    }

    [Fact]
    public void CopyTo_WithSufficientSpace_CopiesCorrectly()
    {
        // Arrange
        const int value = 123;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 3; // "123" has 3 characters
        var expectedString = value.ToString(provider: CultureInfo.InvariantCulture);

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithNegativeInteger_CopiesCorrectly()
    {
        // Arrange
        const int value = -456;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
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
    public void CopyTo_WithZero_CopiesCorrectly()
    {
        // Arrange
        const int value = 0;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 1; // "0" has 1 character
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
        const int value = 123;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[2]; // Too small for "123"
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Contains("The destination span is too small to hold the integer value.", exception.Message);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        const int value = 789;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[3]; // Exact size
        const int expectedBytesWritten = 3; // "789" has 3 characters
        var expectedString = value.ToString(CultureInfo.InvariantCulture);

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }

    [Fact]
    public void CopyTo_WithFormat_CopiesCorrectly()
    {
        // Arrange
        const int value = TestInteger;
        const string format = "N0";
        var node = new IntegerNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const int value = TestInteger;
        const string format = "C";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new IntegerNode(value, format, provider);
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
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const int value = 123;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
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
    public void TryGetChar_NegativeInteger_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const int value = -456;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
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
    [InlineData(3)] // "123" has indices 0, 1, 2
    [InlineData(10)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new IntegerNode(123, provider: CultureInfo.InvariantCulture);

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
        var node = new IntegerNode(123, provider: CultureInfo.InvariantCulture);
        var totalLength = node.TotalLength;
        DebugCounters.ResetAllCounters();

        // Act
        var result = node.TryGetChar(totalLength, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
        Assert.Equal(1, DebugCounters.IntegerNode_TryGetChar_CachedTotalLengthEvaluation);
    }

    [Fact]
    public void TryGetChar_ZeroValue_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const int value = 0;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        var expectedChar = value.ToString(CultureInfo.InvariantCulture)[0];

        // Act
        var result = node.TryGetChar(0, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedChar, ch);
    }

    [Theory]
    [InlineData(0, '0')]
    [InlineData(1, '1')]
    [InlineData(9, '9')]
    public void TryGetChar_SingleDigitNumbers_ReturnsTrueAndCorrectChar(int value, char expectedChar)
    {
        // Arrange
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);

        // Act
        var result = node.TryGetChar(0, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedChar, ch);
    }

    [Fact]
    public void ImplicitConversion_FromInt_CreatesIntegerNode()
    {
        // Arrange
        const int value = 42;
        var expectedString = value.ToString(CultureInfo.CurrentCulture);
        var expectedTotalLength = expectedString.Length; // "42" has 2 characters
        const int expectedSyntaxLength = 0; // IntegerNode has no syntax characters

        // Act
        IntegerNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ImplicitConversion_FromNegativeInt_CreatesIntegerNode()
    {
        // Arrange
        const int value = -99;
        var expectedString = value.ToString(CultureInfo.CurrentCulture);
        var expectedTotalLength = expectedString.Length; // "-99" has 3 characters
        const int expectedSyntaxLength = 0; // IntegerNode has no syntax characters

        // Act
        IntegerNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(1000000)]
    [InlineData(-1000000)]
    public void CopyTo_WithLargeNumbers_CopiesCorrectly(int value)
    {
        // Arrange
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[20]; // Large enough buffer

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
    }

    [Theory]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(1234567890)]
    [InlineData(-987654321)]
    public void TryGetChar_WithLargeNumbers_ReturnsCorrectChars(int value)
    {
        // Arrange
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
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
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new IntegerNode(0, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new IntegerNode(123, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new IntegerNode(-456, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new IntegerNode(int.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new IntegerNode(int.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
    }

    [Fact]
    public void ToString_WithFormat_ReturnsFormattedString()
    {
        // Arrange
        const int value = TestInteger;
        const string format = "D8";
        var node = new IntegerNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const int value = TestInteger;
        const string format = "N0";
        var provider = CultureInfo.GetCultureInfo("fr-FR");
        var node = new IntegerNode(value, format, provider);
        var expected = value.ToString(format, provider);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    #region ISpanFormattable Tests - ToString with Format Override

    [Fact]
    public void ToString_WithFormatParameter_OverridesNodeFormat()
    {
        // Arrange
        const int value = TestInteger;
        var node = new IntegerNode(value, "D", CultureInfo.InvariantCulture); // Node has "D" format
        var expected = value.ToString("X", CultureInfo.InvariantCulture); // Expect hex format

        // Act
        var result = node.ToString("X", CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithProviderParameter_OverridesNodeProvider()
    {
        // Arrange
        const int value = TestInteger;
        var nodeProvider = CultureInfo.GetCultureInfo("en-US");
        var overrideProvider = CultureInfo.GetCultureInfo("fr-FR");
        var node = new IntegerNode(value, "N0", nodeProvider);
        var expected = value.ToString("N0", overrideProvider);

        // Act
        var result = node.ToString(null, overrideProvider);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithBothFormatAndProvider_OverridesBothNodeValues()
    {
        // Arrange
        const int value = TestInteger;
        var node = new IntegerNode(value, "D", CultureInfo.GetCultureInfo("en-US"));
        var overrideProvider = CultureInfo.GetCultureInfo("de-DE");
        var expected = value.ToString("N2", overrideProvider);

        // Act
        var result = node.ToString("N2", overrideProvider);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithNullFormat_UsesNodeFormat()
    {
        // Arrange
        const int value = TestInteger;
        const string nodeFormat = "X8";
        var node = new IntegerNode(value, nodeFormat, CultureInfo.InvariantCulture);
        var expected = value.ToString(nodeFormat, CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString(null, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithEmptyFormat_UsesNodeFormat()
    {
        // Arrange
        const int value = TestInteger;
        const string nodeFormat = "D10";
        var node = new IntegerNode(value, nodeFormat, CultureInfo.InvariantCulture);
        var expected = value.ToString(nodeFormat, CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString(string.Empty, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region ISpanFormattable Tests - TryFormat

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const int value = 12345;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
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
        const int value = 255;
        var node = new IntegerNode(value, "D", CultureInfo.InvariantCulture); // Node has "D" format
        Span<char> destination = stackalloc char[20];
        var expected = value.ToString("X", CultureInfo.InvariantCulture); // Expect hex format

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "X".AsSpan(), CultureInfo.InvariantCulture);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithProviderOverride_UsesOverrideProvider()
    {
        // Arrange
        const int value = TestInteger;
        var nodeProvider = CultureInfo.GetCultureInfo("en-US");
        var overrideProvider = CultureInfo.GetCultureInfo("fr-FR");
        var node = new IntegerNode(value, "N0", nodeProvider);
        Span<char> destination = stackalloc char[50];
        var expected = value.ToString("N0", overrideProvider);

        // Act
        var success = node.TryFormat(destination, out var charsWritten, default, overrideProvider);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithEmptyFormat_UsesNodeFormat()
    {
        // Arrange
        const int value = TestInteger;
        const string nodeFormat = "N2";
        var node = new IntegerNode(value, nodeFormat, CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[50];
        var expected = value.ToString(nodeFormat, CultureInfo.InvariantCulture);

        // Act
        var success = node.TryFormat(destination, out var charsWritten, ReadOnlySpan<char>.Empty, CultureInfo.InvariantCulture);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        const int value = TestInteger; // "12345" is 5 characters
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[3]; // Too small

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_WithExactSpace_FormatsCorrectly()
    {
        // Arrange
        const int value = 123;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        var expected = value.ToString(CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[expected.Length]; // Exact size

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination.ToString());
    }

    [Theory]
    [InlineData("D8", "00012345")]
    [InlineData("X", "3039")]
    [InlineData("x", "3039")]
    [InlineData("N0", "12,345")]
    public void TryFormat_WithVariousFormats_FormatsCorrectly(string format, string expected)
    {
        // Arrange
        const int value = TestInteger;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
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
        var node = new IntegerNode(TestInteger, "N0", CultureInfo.InvariantCulture);
        
        // Act - First access computes and caches the length
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;
        var length3 = node.TotalLength;

        // Assert - All accesses should return the same value
        Assert.Equal(length1, length2);
        Assert.Equal(length2, length3);
        Assert.Equal(TestInteger.ToString("N0", CultureInfo.InvariantCulture).Length, length1);
    }

    [Fact]
    public void TotalLength_WithNoFormat_IsCached()
    {
        // Arrange
        var node = new IntegerNode(TestInteger, provider: CultureInfo.InvariantCulture);
        
        // Act
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;

        // Assert
        Assert.Equal(length1, length2);
        Assert.Equal(5, length1); // "12345" is 5 characters
    }

    [Fact]
    public void TotalLength_WithComplexFormat_IsCached()
    {
        // Arrange
        var node = new IntegerNode(TestInteger, "C2", CultureInfo.GetCultureInfo("en-US"));
        
        // Act
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;
        var expected = TestInteger.ToString("C2", CultureInfo.GetCultureInfo("en-US"));

        // Assert
        Assert.Equal(length1, length2);
        Assert.Equal(expected.Length, length1);
    }

    #endregion

    #region ToString vs TryFormat Behavior Tests

    [Fact]
    public void ToString_CallsStringCreate_WhichInternallyUsesTryFormat()
    {
        // Arrange
        const int value = TestInteger;
        var node = new IntegerNode(value, "D", CultureInfo.InvariantCulture);
        var expected = value.ToString("D", CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString();

        // Assert - ToString creates string using string.Create which internally uses CopyTo/TryFormat
        Assert.Equal(expected, result);
        Assert.Equal(expected.Length, node.TotalLength);
    }

    [Fact]
    public void TryFormat_DirectlyWritesToSpan_MoreEfficientThanToString()
    {
        // Arrange
        const int value = TestInteger;
        var node = new IntegerNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> buffer = stackalloc char[20];

        // Act - TryFormat writes directly to span (single operation)
        var success = node.TryFormat(buffer, out var charsWritten);
        
        // ToString would: 1) access TotalLength, 2) create string via TryFormat internally
        var stringResult = node.ToString();

        // Assert
        Assert.True(success);
        Assert.Equal(stringResult, buffer[..charsWritten].ToString());
    }

    #endregion
}