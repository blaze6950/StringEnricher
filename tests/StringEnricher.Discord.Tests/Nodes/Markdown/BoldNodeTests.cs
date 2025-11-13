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
}
