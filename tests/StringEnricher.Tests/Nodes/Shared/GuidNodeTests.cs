﻿﻿using StringEnricher.Nodes.Shared;

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
    [InlineData("D")]     // Default format with hyphens
    [InlineData("N")]     // No hyphens
    [InlineData("B")]     // With braces
    [InlineData("P")]     // With parentheses
    [InlineData("X")]     // Hexadecimal format
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
}
