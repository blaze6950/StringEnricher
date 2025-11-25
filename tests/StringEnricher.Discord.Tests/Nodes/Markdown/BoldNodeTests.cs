using StringEnricher.Discord.Helpers.Markdown;

namespace StringEnricher.Discord.Tests.Nodes.Markdown;

public class BoldNodeTests
{
    [Fact]
    public void Test_SingleApply()
    {
        // Arrange
        const string expectedBold = "**bold text**";

        // Act
        var styledBold = BoldMarkdown.Apply("bold text").ToString();

        // Assert
        Assert.NotNull(styledBold);
        Assert.NotEmpty(styledBold);
        Assert.Equal(expectedBold, styledBold);
    }

    [Fact]
    public void Test_DoubleApply()
    {
        // Arrange
        const string expectedBold = "****bold text****";

        // Act
        var styledBold = BoldMarkdown.Apply(
            BoldMarkdown.Apply("bold text")
        ).ToString();

        // Assert
        Assert.NotNull(styledBold);
        Assert.NotEmpty(styledBold);
        Assert.Equal(expectedBold, styledBold);
    }

    [Fact]
    public void Test_TryGetChar_WithinValidRange_ShouldReturnTrueAndCurrentChar()
    {
        // Arrange
        var boldStyle = BoldMarkdown.Apply("bold text");
        const string expectedString = "**bold text**";
        var totalLength = expectedString.Length;

        // Act & Assert
        for (var i = 0; i < totalLength; i++)
        {
            var result = boldStyle.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expectedString[i], actualChar);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(13)]
    public void Test_TryGetChar_OutOfRange_ShouldReturnFalse(int index)
    {
        // Arrange
        var boldStyle = BoldMarkdown.Apply("bold text");
        const char expectedChar = '\0';

        // Act
        var resultNegative = boldStyle.TryGetChar(index, out var actualChar);

        // Assert
        Assert.False(resultNegative);
        Assert.Equal(expectedChar, actualChar);
    }

    [Fact]
    public void TotalLength_ReturnsCorrectLength()
    {
        // Arrange
        const string innerText = "test";
        var node = BoldMarkdown.Apply(innerText);
        const int expectedLength = 8; // "**test**"

        // Act
        var actualLength = node.TotalLength;

        // Assert
        Assert.Equal(expectedLength, actualLength);
    }

    [Fact]
    public void SyntaxLength_ReturnsCorrectLength()
    {
        // Arrange
        var node = BoldMarkdown.Apply("test");
        const int expectedSyntaxLength = 4; // "**" + "**"

        // Act
        var actualSyntaxLength = node.SyntaxLength;

        // Assert
        Assert.Equal(expectedSyntaxLength, actualSyntaxLength);
    }

    [Fact]
    public void InnerLength_ReturnsCorrectLength()
    {
        // Arrange
        const string innerText = "test";
        var node = BoldMarkdown.Apply(innerText);
        var expectedInnerLength = innerText.Length;

        // Act
        var actualInnerLength = node.InnerLength;

        // Assert
        Assert.Equal(expectedInnerLength, actualInnerLength);
    }

    [Fact]
    public void CopyTo_WithSufficientSpace_CopiesCorrectly()
    {
        // Arrange
        const string innerText = "test";
        var node = BoldMarkdown.Apply(innerText);
        Span<char> destination = stackalloc char[20];
        const string expected = "**test**";

        // Act
        var charsWritten = node.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void ToString_WithFormatAndProvider_ReturnsCorrectString()
    {
        // Arrange
        const int value = 123;
        var node = BoldMarkdown.Apply(value);
        const string expected = "**123**";

        // Act
        var result = node.ToString(null, null);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryFormat_WithSufficientSpace_FormatsCorrectly()
    {
        // Arrange
        const string innerText = "test";
        var node = BoldMarkdown.Apply(innerText);
        Span<char> destination = stackalloc char[20];
        const string expected = "**test**";

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithInsufficientSpace_ReturnsFalse()
    {
        // Arrange
        var node = BoldMarkdown.Apply("test");
        Span<char> destination = stackalloc char[3]; // Too small for "**test**"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_WithExactlyPrefixSpace_ReturnsFalse()
    {
        // Arrange
        var node = BoldMarkdown.Apply("test");
        Span<char> destination = stackalloc char[2]; // Only space for prefix "**"

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
        Assert.Equal(2, charsWritten); // Prefix was written before failure
    }

    [Fact]
    public void TryFormat_WithPrefixPlusPartialInnerSpace_ReturnsFalse()
    {
        // Arrange
        var node = BoldMarkdown.Apply("test");
        Span<char> destination = stackalloc char[4]; // Space for prefix "**" + 2 chars

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_WithoutSpaceForSuffix_ReturnsFalse()
    {
        // Arrange
        var node = BoldMarkdown.Apply("test");
        Span<char> destination = stackalloc char[6]; // Space for prefix "**" + "test" but not suffix

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_EmptyDestination_ReturnsFalse()
    {
        // Arrange
        var node = BoldMarkdown.Apply("text");
        Span<char> destination = Span<char>.Empty;

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryFormat_ExactSizeDestination_ReturnsTrue()
    {
        // Arrange
        const string innerText = "test";
        var node = BoldMarkdown.Apply(innerText);
        const string expected = "**test**";
        Span<char> destination = stackalloc char[expected.Length]; // Exactly the right size

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_OneCharLessThanNeeded_ReturnsFalse()
    {
        // Arrange
        const string innerText = "test";
        var node = BoldMarkdown.Apply(innerText);
        const string expected = "**test**";
        Span<char> destination = stackalloc char[expected.Length - 1]; // One char too small

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.False(success);
    }

    [Fact]
    public void TryFormat_WithInnerInteger_FormatsCorrectly()
    {
        // Arrange
        const int value = 42;
        var node = BoldMarkdown.Apply(value);
        Span<char> destination = stackalloc char[20];
        const string expected = "**42**";

        // Act
        var success = node.TryFormat(destination, out var charsWritten);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void ToString_WithFormatParameter_PassesFormatToInnerNode()
    {
        // Arrange
        const int value = 12345;
        var node = BoldMarkdown.Apply(value);
        var expected = $"**{value:X}**"; // Hex format

        // Act
        var result = node.ToString("X", null);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithProviderParameter_PassesProviderToInnerNode()
    {
        // Arrange
        const int value = 12345;
        var provider = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");
        var node = BoldMarkdown.Apply(value);
        var expected = $"**{value.ToString("N0", provider)}**";

        // Act
        var result = node.ToString("N0", provider);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryFormat_WithFormatParameter_PassesFormatToInnerNode()
    {
        // Arrange
        const int value = 255;
        var node = BoldMarkdown.Apply(value);
        Span<char> destination = stackalloc char[20];
        var expected = $"**{value:X}**"; // Hex format: **FF**

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "X".AsSpan(), null);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithProviderParameter_PassesProviderToInnerNode()
    {
        // Arrange
        const int value = 12345;
        var provider = System.Globalization.CultureInfo.GetCultureInfo("de-DE");
        var node = BoldMarkdown.Apply(value);
        Span<char> destination = stackalloc char[30];
        var expected = $"**{value.ToString("N0", provider)}**";

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "N0".AsSpan(), provider);

        // Assert
        Assert.True(success);
        Assert.Equal(expected.Length, charsWritten);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }

    [Fact]
    public void TryFormat_WithBothFormatAndProvider_PassesBothToInnerNode()
    {
        // Arrange
        const decimal value = 1234.56m;
        var provider = System.Globalization.CultureInfo.GetCultureInfo("en-GB");
        var node = BoldMarkdown.Apply(value);
        Span<char> destination = stackalloc char[30];
        var expected = $"**{value.ToString("C", provider)}**";

        // Act
        var success = node.TryFormat(destination, out var charsWritten, "C".AsSpan(), provider);

        // Assert
        Assert.True(success);
        Assert.Equal(expected, destination[..charsWritten].ToString());
    }
}
