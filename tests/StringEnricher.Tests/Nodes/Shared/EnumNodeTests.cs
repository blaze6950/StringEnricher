using StringEnricher.Debug;
 using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class EnumNodeTests
{
    public enum TestEnum
    {
        First,
        Second,
        Third
    }

    public enum TestEnumWithValues
    {
        Low = 1,
        Medium = 5,
        High = 10
    }

    [Flags]
    public enum TestFlagsEnum
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4,
        ReadWrite = Read | Write,
        All = Read | Write | Execute
    }

    private const TestEnum TestEnumValue = TestEnum.Second;

    [Fact]
    public void Constructor_WithEnum_InitializesCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.First;
        const int expectedTotalLength = 5; // "First" has 5 characters
        const int expectedSyntaxLength = 0; // EnumNode has no syntax characters

        // Act
        var node = new EnumNode<TestEnum>(value);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithEnumAndFormat_InitializesCorrectly()
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.High;
        const string format = "D";
        var expectedString = value.ToString(format);
        var expectedTotalLength = expectedString.Length;
        const int expectedSyntaxLength = 0;

        // Act
        var node = new EnumNode<TestEnumWithValues>(value, format);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedSyntaxLength, node.SyntaxLength);
    }

    [Theory]
    [InlineData("D")] // Decimal
    [InlineData("G")] // General
    [InlineData("F")] // Format
    [InlineData("X")] // Hexadecimal uppercase
    [InlineData("x")] // Hexadecimal lowercase
    public void Constructor_WithVariousFormats_InitializesCorrectly(string format)
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.Medium;
        var expectedString = value.ToString(format);
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new EnumNode<TestEnumWithValues>(value, format);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void TotalLength_WithVariousEnums_ReturnsCorrectLength()
    {
        // Arrange & Act
        var node = new EnumNode<TestEnum>(TestEnumValue);

        // Assert
        Assert.Equal(6, node.TotalLength); // "Second" has 6 characters
    }

    [Fact]
    public void CopyTo_WithSufficientSpace_CopiesCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.First;
        var node = new EnumNode<TestEnum>(value);
        Span<char> destination = stackalloc char[10];
        const int expectedBytesWritten = 5; // "First" has 5 characters
        var expectedString = value.ToString();

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithFlagsEnum_CopiesCorrectly()
    {
        // Arrange
        const TestFlagsEnum value = TestFlagsEnum.ReadWrite;
        var node = new EnumNode<TestFlagsEnum>(value);
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
    public void CopyTo_WithEnumValue_CopiesCorrectly()
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.Low;
        var node = new EnumNode<TestEnumWithValues>(value);
        Span<char> destination = stackalloc char[10];
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
        const TestEnum value = TestEnum.First;
        var node = new EnumNode<TestEnum>(value);

        // Act
        var exception = Record.Exception(() =>
        {
            Span<char> destination = stackalloc char[2]; // Too small for "First"
            return node.CopyTo(destination);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Contains("The destination span is too small to hold the enum value.", exception.Message);
    }

    [Fact]
    public void CopyTo_WithExactSpace_CopiesCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.Third;
        var node = new EnumNode<TestEnum>(value);
        Span<char> destination = stackalloc char[5]; // Exact size for "Third"
        const int expectedBytesWritten = 5; // "Third" has 5 characters
        var expectedString = value.ToString();

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
        const TestEnumWithValues value = TestEnumWithValues.High;
        const string format = "D";
        var node = new EnumNode<TestEnumWithValues>(value, format);
        Span<char> destination = stackalloc char[10];
        var expectedString = value.ToString(format);
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Fact]
    public void CopyTo_WithHexFormat_CopiesCorrectly()
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.Medium;
        const string format = "X";
        var node = new EnumNode<TestEnumWithValues>(value, format);
        Span<char> destination = stackalloc char[10];
        var expectedString = value.ToString(format);
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
        const TestEnum value = TestEnum.First;
        var node = new EnumNode<TestEnum>(value);
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
        const TestEnumWithValues value = TestEnumWithValues.Low;
        const string format = "D";
        var node = new EnumNode<TestEnumWithValues>(value, format);
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
    [InlineData(5)] // "First" has indices 0, 1, 2, 3, 4
    [InlineData(10)]
    public void TryGetChar_OutOfRangeIndices_ReturnsFalseAndNullChar(int index)
    {
        // Arrange
        var node = new EnumNode<TestEnum>(TestEnum.First);

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
        var node = new EnumNode<TestEnum>(TestEnum.First);
        var totalLength = node.TotalLength;
        DebugCounters.ResetAllCounters();

        // Act
        var result = node.TryGetChar(totalLength, out var ch);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', ch);
        Assert.Equal(1, DebugCounters.EnumNode_TryGetChar_CachedTotalLengthEvaluation);
    }

    [Fact]
    public void TryGetChar_WithFlagsEnum_ReturnsTrueAndCorrectChar()
    {
        // Arrange
        const TestFlagsEnum value = TestFlagsEnum.Read;
        var node = new EnumNode<TestFlagsEnum>(value);
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
    [InlineData(TestEnum.First, 'F')]
    [InlineData(TestEnum.Second, 'S')]
    [InlineData(TestEnum.Third, 'T')]
    public void TryGetChar_FirstCharacter_ReturnsCorrectChar(TestEnum value, char expectedChar)
    {
        // Arrange
        var node = new EnumNode<TestEnum>(value);

        // Act
        var result = node.TryGetChar(0, out var ch);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedChar, ch);
    }

    [Theory]
    [InlineData(TestEnum.First)]
    [InlineData(TestEnum.Second)]
    [InlineData(TestEnum.Third)]
    public void CopyTo_WithDifferentEnumValues_CopiesCorrectly(TestEnum value)
    {
        // Arrange
        var node = new EnumNode<TestEnum>(value);
        var expected = value.ToString();
        Span<char> destination = stackalloc char[20]; // Large enough buffer

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, bytesWritten);
        Assert.Equal(expected, destination[..bytesWritten].ToString());
    }

    [Theory]
    [InlineData(TestEnumWithValues.Low)]
    [InlineData(TestEnumWithValues.Medium)]
    [InlineData(TestEnumWithValues.High)]
    public void TryGetChar_WithEnumValues_ReturnsCorrectChars(TestEnumWithValues value)
    {
        // Arrange
        var node = new EnumNode<TestEnumWithValues>(value);
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
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange & Act & Assert
        Assert.Equal(0, new EnumNode<TestEnum>(TestEnum.First).SyntaxLength);
        Assert.Equal(0, new EnumNode<TestEnum>(TestEnum.Second).SyntaxLength);
        Assert.Equal(0, new EnumNode<TestEnum>(TestEnum.Third).SyntaxLength);
        Assert.Equal(0, new EnumNode<TestEnumWithValues>(TestEnumWithValues.Low).SyntaxLength);
        Assert.Equal(0, new EnumNode<TestFlagsEnum>(TestFlagsEnum.Read).SyntaxLength);
    }

    [Fact]
    public void ToString_WithFormat_ReturnsFormattedString()
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.High;
        const string format = "D";
        var node = new EnumNode<TestEnumWithValues>(value, format);
        var expected = value.ToString(format);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithGeneralFormat_ReturnsFormattedString()
    {
        // Arrange
        const TestEnum value = TestEnum.Second;
        const string format = "G";
        var node = new EnumNode<TestEnum>(value, format);
        var expected = value.ToString(format);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithFormatFormat_ReturnsFormattedString()
    {
        // Arrange
        const TestEnum value = TestEnum.Third;
        const string format = "F";
        var node = new EnumNode<TestEnum>(value, format);
        var expected = value.ToString(format);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithHexFormat_ReturnsFormattedString()
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.Medium;
        const string format = "X";
        var node = new EnumNode<TestEnumWithValues>(value, format);
        var expected = value.ToString(format);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithNullFormat_ReturnsDefaultString()
    {
        // Arrange
        const TestEnum value = TestEnum.Second;
        var node = new EnumNode<TestEnum>(value, null);
        var expected = value.ToString();

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Constructor_WithNullFormat_InitializesCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.First;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new EnumNode<TestEnum>(value, null);

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void Constructor_WithEmptyFormat_InitializesCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.Second;
        var expectedString = value.ToString();
        var expectedTotalLength = expectedString.Length;

        // Act
        var node = new EnumNode<TestEnum>(value, "");

        // Assert
        Assert.Equal(expectedTotalLength, node.TotalLength);
        Assert.Equal(expectedString, node.ToString());
    }

    [Fact]
    public void GetEnumLength_WithLongFormat_HandlesCorrectly()
    {
        // Arrange
        const TestFlagsEnum value = TestFlagsEnum.All;
        const string format = "F";

        // Act
        var node = new EnumNode<TestFlagsEnum>(value, format);
        var expected = value.ToString(format);

        // Assert
        Assert.Equal(expected.Length, node.TotalLength);
        Assert.Equal(expected, node.ToString());
    }

    [Fact]
    public void CopyTo_WithComplexFlagsEnum_CopiesCorrectly()
    {
        // Arrange
        const TestFlagsEnum value = TestFlagsEnum.All;
        var node = new EnumNode<TestFlagsEnum>(value);
        Span<char> destination = stackalloc char[50]; // Large buffer for complex enum
        var expectedString = value.ToString();
        var expectedBytesWritten = expectedString.Length;

        // Act
        var bytesWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expectedBytesWritten, bytesWritten);
        Assert.Equal(expectedString, destination[..bytesWritten].ToString());
    }

    [Theory]
    [InlineData(TestFlagsEnum.None)]
    [InlineData(TestFlagsEnum.Read)]
    [InlineData(TestFlagsEnum.Write)]
    [InlineData(TestFlagsEnum.Execute)]
    [InlineData(TestFlagsEnum.ReadWrite)]
    [InlineData(TestFlagsEnum.All)]
    public void ToString_WithDifferentFlagsValues_ReturnsCorrectString(TestFlagsEnum value)
    {
        // Arrange
        var node = new EnumNode<TestFlagsEnum>(value);
        var expected = value.ToString();

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("D")]
    [InlineData("G")]
    [InlineData("F")]
    [InlineData("X")]
    [InlineData("x")]
    public void TotalLength_WithDifferentFormats_ReturnsCorrectLength(string format)
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.Medium;
        var expectedString = value.ToString(format);
        var expectedLength = expectedString.Length;

        // Act
        var node = new EnumNode<TestEnumWithValues>(value, format);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    #region ISpanFormattable Tests

    [Fact]
    public void ToString_WithFormatParameter_OverridesNodeFormat()
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.Medium;
        var node = new EnumNode<TestEnumWithValues>(value, "G");
        var expected = value.ToString("X");

        // Act
        var result = node.ToString("X", null);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const TestEnum value = TestEnum.Second;
        var node = new EnumNode<TestEnum>(value);
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
        const TestEnumWithValues value = TestEnumWithValues.High;
        var node = new EnumNode<TestEnumWithValues>(value, "G");
        Span<char> destination = stackalloc char[50];
        var expected = value.ToString("D");

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "D".AsSpan(), null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        const TestEnum value = TestEnum.Second;
        var node = new EnumNode<TestEnum>(value);
        Span<char> destination = stackalloc char[2];

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Theory]
    [InlineData("G")]
    [InlineData("F")]
    [InlineData("D")]
    [InlineData("X")]
    public void TryFormat_WithVariousFormats_FormatsCorrectly(string format)
    {
        // Arrange
        const TestEnumWithValues value = TestEnumWithValues.Medium;
        var node = new EnumNode<TestEnumWithValues>(value);
        Span<char> destination = stackalloc char[50];
        var expected = value.ToString(format);

        // Act
        var success = node.TryFormat(destination, out var charsWritten, format.AsSpan(), null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TotalLength_IsCachedAfterFirstAccess()
    {
        // Arrange
        var node = new EnumNode<TestEnum>(TestEnum.Second, "G");
        
        // Act
        var length1 = node.TotalLength;
        var length2 = node.TotalLength;

        // Assert
        Assert.Equal(length1, length2);
    }

    #endregion
}