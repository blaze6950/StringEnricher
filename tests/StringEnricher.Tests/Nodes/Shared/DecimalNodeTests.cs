using StringEnricher.Nodes.Shared;
using System.Globalization;
using StringEnricher.Debug;

namespace StringEnricher.Tests.Nodes.Shared;

public class DecimalNodeTests
{
    private const decimal TestDecimal = 123.45m;

    [Fact]
    public void Constructor_WithPositiveDecimal_InitializesCorrectly()
    {
        // Arrange
        const decimal value = 123.45m;
        var expectedString = value.ToString(CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // DecimalNode has no syntax characters

        // Act
        var node = new DecimalNode(value, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithDecimalAndFormat_InitializesCorrectly()
    {
        // Arrange
        const decimal value = TestDecimal;
        const string format = "C";
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new DecimalNode(value, format, provider: CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithDecimalFormatAndProvider_InitializesCorrectly()
    {
        // Arrange
        const decimal value = TestDecimal;
        const string format = "C";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var expectedString = value.ToString(format, provider);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new DecimalNode(value, format, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("C")] // Currency
    [InlineData("N")] // Number
    [InlineData("F2")] // Fixed-point with 2 decimals
    [InlineData("P")] // Percent
    [InlineData("E")] // Exponential
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        const decimal value = TestDecimal;
        var expectedString = value.ToString(format, provider: CultureInfo.InvariantCulture);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new DecimalNode(value, format, provider: CultureInfo.InvariantCulture);

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
        const decimal value = TestDecimal;
        var provider = CultureInfo.GetCultureInfo(cultureName);
        var expectedString = value.ToString(provider);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new DecimalNode(value, null, provider);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void TotalLength_WithVariousDecimals_ReturnsCorrectLength()
    {
        // Arrange & Act
        var node = new DecimalNode(TestDecimal, provider: CultureInfo.InvariantCulture);
        var expectedLength = TestDecimal.ToString(CultureInfo.InvariantCulture).Length;

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void CopyTo_WithDecimal_CopiesCorrectly()
    {
        // Arrange
        const decimal value = 123.45m;
        var node = new DecimalNode(value, provider: CultureInfo.InvariantCulture);
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
    public void CopyTo_WithNegativeDecimal_CopiesCorrectly()
    {
        // Arrange
        const decimal value = -456.78m;
        var node = new DecimalNode(value, provider: CultureInfo.InvariantCulture);
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
    public void CopyTo_WithFormat_CopiesCorrectly()
    {
        // Arrange
        const decimal value = TestDecimal;
        const string format = "N2";
        var node = new DecimalNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const decimal value = TestDecimal;
        const string format = "C";
        var provider = CultureInfo.GetCultureInfo("en-GB");
        var node = new DecimalNode(value, format, provider);
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
        const decimal value = 123.45m;
        var node = new DecimalNode(value, provider: CultureInfo.InvariantCulture);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[2]; // Too small
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Contains("The destination span is too small to hold the decimal value.", exception.Message);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const decimal value = 123.45m;
        var node = new DecimalNode(value, provider: CultureInfo.InvariantCulture);
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
    [InlineData(10)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new DecimalNode(123.45m, provider: CultureInfo.InvariantCulture);

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
        var node = new DecimalNode(123.45m, provider: CultureInfo.InvariantCulture);
        var totalLength = node.TotalLength;
        DebugCounters.ResetAllCounters();

        // Act
        var result = node.TryGetChar(totalLength, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
        Assert.Equal(1, DebugCounters.DecimalNode_TryGetChar_CachedTotalLengthEvaluation);
    }

    [Fact]
    public void TryGetChar_WithFormat_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const decimal value = TestDecimal;
        const string format = "F2";
        var node = new DecimalNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const decimal value = TestDecimal;
        const string format = "N";
        var provider = CultureInfo.GetCultureInfo("de-DE");
        var node = new DecimalNode(value, format, provider);
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
    public void ImplicitConversion_FromDecimal_CreatesDecimalNode()
    {
        // Arrange
        const decimal value = 42.5m;
        var expectedString = value.ToString(CultureInfo.CurrentCulture);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        DecimalNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithDecimal_ReturnsCorrectString()
    {
        // Arrange
        const decimal value = 789.123m;
        var node = new DecimalNode(value, provider: CultureInfo.InvariantCulture);
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
        const decimal value = TestDecimal;
        const string format = "C";
        var node = new DecimalNode(value, format, provider: CultureInfo.InvariantCulture);
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
        const decimal value = TestDecimal;
        const string format = "N2";
        var provider = CultureInfo.GetCultureInfo("fr-FR");
        var node = new DecimalNode(value, format, provider);
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
        Assert.Equal(0, new DecimalNode(0m, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new DecimalNode(123.45m, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new DecimalNode(-456.78m, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new DecimalNode(decimal.MaxValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
        Assert.Equal(0, new DecimalNode(decimal.MinValue, provider: CultureInfo.InvariantCulture).SyntaxLength);
    }

    #region ISpanFormattable Tests - ToString with Format Override

    [Fact]
    public void ToString_WithFormatParameter_OverridesNodeFormat()
    {
        // Arrange
        const decimal value = TestDecimal;
        var node = new DecimalNode(value, "F2", CultureInfo.InvariantCulture);
        var expected = value.ToString("F4", CultureInfo.InvariantCulture);

        // Act
        var result = node.ToString("F4", CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithProviderParameter_OverridesNodeProvider()
    {
        // Arrange
        const decimal value = TestDecimal;
        var nodeProvider = CultureInfo.GetCultureInfo("en-US");
        var overrideProvider = CultureInfo.GetCultureInfo("fr-FR");
        var node = new DecimalNode(value, "N2", nodeProvider);
        var expected = value.ToString("N2", overrideProvider);

        // Act
        var result = node.ToString(null, overrideProvider);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithNullFormat_UsesNodeFormat()
    {
        // Arrange
        const decimal value = TestDecimal;
        const string nodeFormat = "C2";
        var node = new DecimalNode(value, nodeFormat, CultureInfo.InvariantCulture);
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
        const decimal value = TestDecimal;
        var node = new DecimalNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[50];
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
        const decimal value = TestDecimal;
        var node = new DecimalNode(value, "F2", CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[50];
        var expected = value.ToString("F4", CultureInfo.InvariantCulture);

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "F4".AsSpan(), CultureInfo.InvariantCulture);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        const decimal value = TestDecimal;
        var node = new DecimalNode(value, provider: CultureInfo.InvariantCulture);
        Span<char> destination = stackalloc char[3];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Theory]
    [InlineData("F2", "123.45")]
    [InlineData("F4", "123.4500")]
    [InlineData("N2", "123.45")]
    [InlineData("C2", "¤123.45")]
    public void TryFormat_WithVariousFormats_FormatsCorrectly(string format, string expected)
    {
        // Arrange
        const decimal value = TestDecimal;
        var node = new DecimalNode(value, provider: CultureInfo.InvariantCulture);
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
        var node = new DecimalNode(TestDecimal, "F2", CultureInfo.InvariantCulture);
        
        // Act
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;

        // Assert
        Assert.Equal(length1, length2);
    }

    #endregion
}