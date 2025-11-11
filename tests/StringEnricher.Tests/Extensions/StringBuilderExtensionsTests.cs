using System.Text;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes.Shared;
using StringEnricher.Telegram.Nodes.Html.Formatting;

namespace StringEnricher.Tests.Extensions;

public class StringBuilderExtensionsTests
{
    [Fact]
    public void AppendNode_WithSmallNode_UsesStackAllocation()
    {
        // Arrange
        var sb = new StringBuilder();
        var node = new IntegerNode(123);
        var expectedContent = "123";

        // Act
        var result = sb.AppendNode(node);

        // Assert
        Assert.Same(sb, result); // Should return the same StringBuilder for chaining
        Assert.Equal(expectedContent, sb.ToString());
        Assert.True(node.TotalLength <= StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
    }

    [Fact]
    public void AppendNode_WithBoldNode_FormatsCorrectly()
    {
        // Arrange
        var sb = new StringBuilder();
        var plainText = new PlainTextNode("Hello World");
        var boldNode = new BoldNode<PlainTextNode>(plainText);
        var expectedContent = "<b>Hello World</b>";

        // Act
        var result = sb.AppendNode(boldNode);

        // Assert
        Assert.Same(sb, result);
        Assert.Equal(expectedContent, sb.ToString());
        Assert.True(boldNode.TotalLength <= StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
    }

    [Fact]
    public void AppendNode_WithItalicNode_FormatsCorrectly()
    {
        // Arrange
        var sb = new StringBuilder();
        var plainText = new PlainTextNode("Italic text");
        var italicNode = new ItalicNode<PlainTextNode>(plainText);
        var expectedContent = "<i>Italic text</i>";

        // Act
        var result = sb.AppendNode(italicNode);

        // Assert
        Assert.Same(sb, result);
        Assert.Equal(expectedContent, sb.ToString());
    }

    [Fact]
    public void AppendNode_WithNestedBoldAndItalic_FormatsCorrectly()
    {
        // Arrange
        var sb = new StringBuilder();
        var plainText = new PlainTextNode("Bold and Italic");
        var italicNode = new ItalicNode<PlainTextNode>(plainText);
        var boldItalicNode = new BoldNode<ItalicNode<PlainTextNode>>(italicNode);
        var expectedContent = "<b><i>Bold and Italic</i></b>";

        // Act
        var result = sb.AppendNode(boldItalicNode);

        // Assert
        Assert.Same(sb, result);
        Assert.Equal(expectedContent, sb.ToString());
    }

    [Fact]
    public void AppendNode_WithDeeplyNestedNodes_FormatsCorrectly()
    {
        // Arrange
        var sb = new StringBuilder();
        var plainText = new PlainTextNode("Nested");
        var level1 = new ItalicNode<PlainTextNode>(plainText);
        var level2 = new BoldNode<ItalicNode<PlainTextNode>>(level1);
        var level3 = new ItalicNode<BoldNode<ItalicNode<PlainTextNode>>>(level2);
        var expectedContent = "<i><b><i>Nested</i></b></i>";

        // Act
        var result = sb.AppendNode(level3);

        // Assert
        Assert.Same(sb, result);
        Assert.Equal(expectedContent, sb.ToString());
    }

    [Fact]
    public void AppendNode_WithMediumSizedBoldNode_UsesArrayPooling()
    {
        // Arrange
        var sb = new StringBuilder();
        
        // Create a large text that will exceed stack alloc threshold when wrapped in bold tags
        var largeText = new string('A', StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength + 50);
        var plainTextNode = new PlainTextNode(largeText);
        var boldNode = new BoldNode<PlainTextNode>(plainTextNode);

        // Act
        var result = sb.AppendNode(boldNode);

        // Assert
        Assert.Same(sb, result);
        Assert.Equal($"<b>{largeText}</b>", sb.ToString());
        Assert.True(boldNode.TotalLength > StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength);
        Assert.True(boldNode.TotalLength <= StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength);
    }

    [Fact]
    public void AppendNode_WithLargeBoldNode_UsesHeapAllocation()
    {
        // Arrange
        var sb = new StringBuilder();
        
        // Create a very large text that will exceed both thresholds when wrapped in bold tags
        var maxPooledLength = StringEnricherSettings.Extensions.StringBuilder.MaxPooledArrayLength;
        var largeSize = maxPooledLength - BoldNode<PlainTextNode>.Prefix.Length - BoldNode<PlainTextNode>.Suffix.Length + 1000;
        var veryLargeText = new string('B', largeSize);
        var plainTextNode = new PlainTextNode(veryLargeText);
        var boldNode = new BoldNode<PlainTextNode>(plainTextNode);

        // Act
        var result = sb.AppendNode(boldNode);

        // Assert
        Assert.Same(sb, result);
        Assert.Equal($"<b>{veryLargeText}</b>", sb.ToString());
        Assert.True(boldNode.TotalLength > maxPooledLength, 
            $"Bold node length {boldNode.TotalLength} should be greater than MaxPooledArrayLength {maxPooledLength}");
    }

    [Fact]
    public void AppendNode_WithEmptyPlainTextNode_ReturnsOriginalStringBuilder()
    {
        // Arrange
        var sb = new StringBuilder("initial");
        var emptyNode = new PlainTextNode("");

        // Act
        var result = sb.AppendNode(emptyNode);

        // Assert
        Assert.Same(sb, result);
        Assert.Equal("initial", sb.ToString());
    }

    [Fact]
    public void AppendNode_WithExistingContent_AppendNodesNodeCorrectly()
    {
        // Arrange
        var sb = new StringBuilder("Hello ");
        var plainText = new PlainTextNode("World!");
        var boldNode = new BoldNode<PlainTextNode>(plainText);

        // Act
        sb.AppendNode(boldNode);

        // Assert
        Assert.Equal("Hello <b>World!</b>", sb.ToString());
    }

    [Fact]
    public void AppendNode_MultipleFormattedNodes_AppendsNodeAllCorrectly()
    {
        // Arrange
        var sb = new StringBuilder();
        var number = new IntegerNode(42);
        var separator = new PlainTextNode(" - ");
        var boldText = new BoldNode<PlainTextNode>(new PlainTextNode("Bold"));
        var space = new PlainTextNode(" ");
        var italicText = new ItalicNode<PlainTextNode>(new PlainTextNode("Italic"));

        // Act
        sb.AppendNode(number).AppendNode(separator).AppendNode(boldText).AppendNode(space).AppendNode(italicText);

        // Assert
        Assert.Equal("42 - <b>Bold</b> <i>Italic</i>", sb.ToString());
    }

    [Fact]
    public void AppendNode_WithSpecialCharactersInFormattedNodes_HandlesCorrectly()
    {
        // Arrange
        var sb = new StringBuilder();
        var specialChars = "Hello\nWorld\t!🚀";
        var plainText = new PlainTextNode(specialChars);
        var boldNode = new BoldNode<PlainTextNode>(plainText);

        // Act
        sb.AppendNode(boldNode);

        // Assert
        Assert.Equal($"<b>{specialChars}</b>", sb.ToString());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(400)] // Smaller than default MaxStackAllocLength to account for bold formatting overhead
    public void AppendNode_WithVariousFormattedSizes_WorksCorrectly(int textSize)
    {
        // Arrange
        var sb = new StringBuilder();
        var content = new string('X', textSize);
        var plainText = new PlainTextNode(content);
        var boldNode = new BoldNode<PlainTextNode>(plainText);

        // Act
        sb.AppendNode(boldNode);

        // Assert
        Assert.Equal($"<b>{content}</b>", sb.ToString());
        Assert.Equal(textSize + BoldNode<PlainTextNode>.Prefix.Length + BoldNode<PlainTextNode>.Suffix.Length, sb.Length);
    }

    [Fact]
    public void AppendNode_WithNullStringBuilder_ThrowsNullReferenceException()
    {
        // Arrange
        StringBuilder? sb = null;
        var plainText = new PlainTextNode("test");
        var boldNode = new BoldNode<PlainTextNode>(plainText);

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => sb!.AppendNode(boldNode));
    }

    [Fact]
    public void AppendNode_BoldNodeAtExactStackAllocThreshold_UsesStackAllocation()
    {
        // Arrange
        var sb = new StringBuilder();
        var maxStackAlloc = StringEnricherSettings.Extensions.StringBuilder.MaxStackAllocLength;
        var textSize = maxStackAlloc - BoldNode<PlainTextNode>.Prefix.Length - BoldNode<PlainTextNode>.Suffix.Length;
        var content = new string('T', textSize);
        var plainText = new PlainTextNode(content);
        var boldNode = new BoldNode<PlainTextNode>(plainText);

        // Act
        sb.AppendNode(boldNode);

        // Assert
        Assert.Equal($"<b>{content}</b>", sb.ToString());
        Assert.Equal(maxStackAlloc, boldNode.TotalLength);
    }

    [Fact]
    public void AppendNode_MultipleNestedFormattingStyles_WorksCorrectly()
    {
        // Arrange
        var sb = new StringBuilder();
        
        // Create different nesting patterns
        var text1 = new BoldNode<PlainTextNode>(new PlainTextNode("Bold"));
        var text2 = new ItalicNode<BoldNode<PlainTextNode>>(new BoldNode<PlainTextNode>(new PlainTextNode("Bold Italic")));
        var text3 = new BoldNode<ItalicNode<PlainTextNode>>(new ItalicNode<PlainTextNode>(new PlainTextNode("Italic Bold")));

        // Act
        sb.AppendNode(text1).Append(' ').AppendNode(text2).Append(' ').AppendNode(text3);

        // Assert
        Assert.Equal("<b>Bold</b> <i><b>Bold Italic</b></i> <b><i>Italic Bold</i></b>", sb.ToString());
    }
}
