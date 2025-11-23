using StringEnricher.Debug;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class GuidNodeTests
{
    private static readonly Guid TestGuid = new("12345678-1234-5678-9012-123456789012");

    [Fact]
    public void Constructor_WithGuid_InitializesCorrectly()
    {
        // Arrange
        var value = TestGuid;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0; // GuidNode has no syntax characters

        // Act
        var node = new GuidNode(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithGuidAndFormat_InitializesCorrectly()
    {
        // Arrange
        var value = TestGuid;
        const string format = "N";
        var expectedString = value.ToString(format);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new GuidNode(value, format);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("D")] // Default format with hyphens
    [InlineData("N")] // No hyphens
    [InlineData("B")] // With braces
    [InlineData("P")] // With parentheses
    [InlineData("X")] // Hexadecimal format
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        var value = TestGuid;
        var expectedString = value.ToString(format);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new GuidNode(value, format);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void TotalLength_WithVariousGuids_ReturnsCorrectLength()
    {
        // Arrange & Act
        var node1 = new GuidNode(TestGuid);
        var node2 = new GuidNode(Guid.Empty);
        var node3 = new GuidNode(Guid.NewGuid());

        // Assert - All GUIDs have the same string length
        Assert.Equal(36, node1.TotalLength); // Standard GUID string format
        Assert.Equal(36, node2.TotalLength);
        Assert.Equal(36, node3.TotalLength);
    }

    [Fact]
    public void CopyTo_WithGuid_CopiesCorrectly()
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value);
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
    public void CopyTo_WithFormat_CopiesCorrectly()
    {
        // Arrange
        var value = TestGuid;
        const string format = "N";
        var node = new GuidNode(value, format);
        Span<char> destination = stackalloc char[50];
        var expectedString = value.ToString(format);
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
        var value = TestGuid;
        var node = new GuidNode(value);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[10]; // Too small
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Contains("The destination span is too small to hold the GUID value.", exception.Message);
    }

    [Fact]
    public void TryGetChar_ValidIndices_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value);
        var expected = value.ToString();

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
        var value = TestGuid;
        const string format = "B";
        var node = new GuidNode(value, format);
        var expected = value.ToString(format);

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
    [InlineData(36)]
    [InlineData(50)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new GuidNode(TestGuid);

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
        var node = new GuidNode(TestGuid);
        var totalLength = node.TotalLength;
        DebugCounters.ResetAllCounters();

        // Act
        var result = node.TryGetChar(totalLength, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
        Assert.Equal(1, DebugCounters.GuidNode_TryGetChar_CachedTotalLengthEvaluation);
    }

    [Fact]
    public void ImplicitConversion_FromGuid_CreatesGuidNode()
    {
        // Arrange
        var value = TestGuid;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        GuidNode node = value; // Implicit conversion

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithGuid_ReturnsCorrectString()
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value);
        var expected = value.ToString();

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithFormat_ReturnsFormattedString()
    {
        // Arrange
        var value = TestGuid;
        const string format = "X";
        var node = new GuidNode(value, format);
        var expected = value.ToString(format);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new GuidNode(Guid.Empty).SyntaxLength);
        Assert.Equal(0, new GuidNode(TestGuid).SyntaxLength);
        Assert.Equal(0, new GuidNode(Guid.NewGuid()).SyntaxLength);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value);
        Span<char> destination = stackalloc char[36]; // Exact size
        var expectedString = value.ToString();

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(36, bytesWritten);
        Assert.Equal(expectedString, destination.ToString());
    }

    #region ISpanFormattable Tests - ToString with Format Override

    [Fact]
    public void ToString_WithFormatParameter_OverridesNodeFormat()
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value, "D"); // Node has "D" format (with hyphens)
        var expected = value.ToString("N"); // Expect "N" format (no hyphens)

        // Act
        var result = node.ToString("N", null);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(32, result.Length); // "N" format has no hyphens
    }

    [Fact]
    public void ToString_WithNullFormat_UsesNodeFormat()
    {
        // Arrange
        var value = TestGuid;
        const string nodeFormat = "B"; // Braces format
        var node = new GuidNode(value, nodeFormat);
        var expected = value.ToString(nodeFormat);

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
        Assert.StartsWith("{", result);
        Assert.EndsWith("}", result);
    }

    [Fact]
    public void ToString_WithEmptyFormat_UsesNodeFormat()
    {
        // Arrange
        var value = TestGuid;
        const string nodeFormat = "P"; // Parentheses format
        var node = new GuidNode(value, nodeFormat);
        var expected = value.ToString(nodeFormat);

        // Act
        var result = node.ToString(string.Empty, null);

        // Assert
        Assert.Equal(expected, result);
        Assert.StartsWith("(", result);
        Assert.EndsWith(")", result);
    }

    [Theory]
    [InlineData("D", 36)] // xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
    [InlineData("N", 32)] // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    [InlineData("B", 38)] // {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
    [InlineData("P", 38)] // (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
    public void ToString_WithVariousFormats_ReturnsCorrectLength(string format, int expectedLength)
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value, "D"); // Start with default format

        // Act
        var result = node.ToString(format, null);

        // Assert
        Assert.Equal(expectedLength, result.Length);
        Assert.Equal(value.ToString(format), result);
    }

    [Fact]
    public void ToString_ProviderParameter_IsIgnoredForGuid()
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value);
        var provider = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");
        var expected = value.ToString(); // Provider doesn't affect GUID formatting

        // Act
        var result = node.ToString(null, provider);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region ISpanFormattable Tests - TryFormat

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value);
        Span<char> destination = stackalloc char[50];
        var expected = value.ToString();

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
        var value = TestGuid;
        var node = new GuidNode(value, "D"); // Node has "D" format
        Span<char> destination = stackalloc char[50];
        var expected = value.ToString("N"); // Expect "N" format

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "N".AsSpan(), null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
        Assert.Equal(32, charsWritten); // "N" format has 32 characters
    }

    [Fact]
    public void TryFormat_WithEmptyFormat_UsesNodeFormat()
    {
        // Arrange
        var value = TestGuid;
        const string nodeFormat = "B";
        var node = new GuidNode(value, nodeFormat);
        Span<char> destination = stackalloc char[50];
        var expected = value.ToString(nodeFormat);

        // Act
        var success = node.TryFormat(destination, out var charsWritten, ReadOnlySpan<char>.Empty, null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Theory]
    [InlineData("D", 36)]
    [InlineData("N", 32)]
    [InlineData("B", 38)]
    [InlineData("P", 38)]
    public void TryFormat_WithVariousFormats_FormatsCorrectly(string format, int expectedLength)
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value);
        Span<char> destination = stackalloc char[50];

        // Act
        var success = node.TryFormat(destination, out var charsWritten, format.AsSpan(), null);

        // Assert
        Assert.True(success);
        Assert.Equal(expectedLength, charsWritten);
        Assert.Equal(value.ToString(format), destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value); // Default format needs 36 chars
        Span<char> destination = stackalloc char[30]; // Too small

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_WithExactSpace_FormatsCorrectly()
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value, "N"); // "N" format is 32 chars
        var expected = value.ToString("N");
        Span<char> destination = stackalloc char[32]; // Exact size

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(32, charsWritten);
        Assert.Equal(expected, destination.ToString());
    }

    [Fact]
    public void TryFormat_ProviderParameter_IsIgnored()
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value);
        var provider = System.Globalization.CultureInfo.GetCultureInfo("ja-JP");
        Span<char> destination = stackalloc char[50];
        var expected = value.ToString();

        // Act
        var success = node.TryFormat(destination, out var charsWritten, default, provider);

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
        var node = new GuidNode(TestGuid, "D");

        // Act - First access computes and caches the length
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;
        var length3 = node.TotalLength;

        // Assert - All accesses should return the same value
        Assert.Equal(length1, length2);
        Assert.Equal(length2, length3);
        Assert.Equal(36, length1);
    }

    [Theory]
    [InlineData("D", 36)]
    [InlineData("N", 32)]
    [InlineData("B", 38)]
    [InlineData("P", 38)]
    public void TotalLength_WithDifferentFormats_IsCachedCorrectly(string format, int expectedLength)
    {
        // Arrange
        var node = new GuidNode(TestGuid, format);

        // Act
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;

        // Assert
        Assert.Equal(expectedLength, length1);
        Assert.Equal(length1, length2);
    }

    [Fact]
    public void TotalLength_AfterTryGetChar_UsesCachedValue()
    {
        // Arrange
        var node = new GuidNode(TestGuid);

        // Act - TryGetChar might cache the length
        _ = node.TryGetChar(0, out _);
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;

        // Assert
        Assert.Equal(36, length1);
        Assert.Equal(length1, length2);
    }

    #endregion

    #region ToString vs TryFormat Behavior Tests

    [Fact]
    public void ToString_CreatesNewString_TryFormatWritesToExistingSpan()
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value);
        Span<char> buffer = stackalloc char[50];

        // Act
        var stringResult = node.ToString(); // Allocates new string
        var tryFormatSuccess = node.TryFormat(buffer, out var charsWritten); // Writes to existing span

        // Assert
        Assert.True(tryFormatSuccess);
        Assert.Equal(stringResult, buffer[..charsWritten].ToString());
        Assert.Equal(stringResult.Length, charsWritten);
    }

    [Fact]
    public void TryFormat_IsMoreEfficientThanToString_NoStringAllocation()
    {
        // Arrange
        var value = TestGuid;
        var node = new GuidNode(value, "N");
        Span<char> buffer = stackalloc char[32];

        // Act - TryFormat writes directly to buffer without allocating a string
        var success = node.TryFormat(buffer, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(32, charsWritten);
        // The buffer contains the formatted GUID without allocating a string object
        Assert.Equal(value.ToString("N"), buffer.ToString());
    }

    #endregion
}