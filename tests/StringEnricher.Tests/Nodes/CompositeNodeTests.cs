using StringEnricher.Helpers;
using StringEnricher.Telegram.Helpers.MarkdownV2;

namespace StringEnricher.Tests.Nodes;

public class CompositeNodeTests
{
    [Fact]
    public void Test_ExplicitExtensionMethod()
    {
        // Arrange
        var leftBold = BoldMarkdownV2.Apply("left");
        var rightBold = BoldMarkdownV2.Apply("right");
        var expectedTotalLength = rightBold.TotalLength + leftBold.TotalLength;
        var expectedSyntaxLength = rightBold.SyntaxLength + leftBold.SyntaxLength;
        var expectedString = leftBold.ToString() + rightBold.ToString();

        // Act
        var composite = CompositeNodeExtensions.CombineWith(leftBold, rightBold);

        // Assert
        Assert.Equal(expectedTotalLength, composite.TotalLength);
        Assert.Equal(expectedSyntaxLength, composite.SyntaxLength);
        Assert.Equal(expectedString, composite.ToString());
    }

    [Fact]
    public void Test_ImplicitExtensionMethod()
    {
        // Arrange
        var leftBold = BoldMarkdownV2.Apply("left");
        var rightBold = BoldMarkdownV2.Apply("right");
        var expectedTotalLength = rightBold.TotalLength + leftBold.TotalLength;
        var expectedSyntaxLength = rightBold.SyntaxLength + leftBold.SyntaxLength;
        var expectedString = leftBold.ToString() + rightBold.ToString();

        // Act
        var composite = leftBold.CombineWith(rightBold);

        // Assert
        Assert.Equal(expectedTotalLength, composite.TotalLength);
        Assert.Equal(expectedSyntaxLength, composite.SyntaxLength);
        Assert.Equal(expectedString, composite.ToString());
    }
}