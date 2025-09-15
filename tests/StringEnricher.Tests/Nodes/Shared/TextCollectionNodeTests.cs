using StringEnricher.Nodes.Shared;

namespace StringEnricher.Tests.Nodes.Shared;

public class TextCollectionNodeTests
{
    [Fact]
    public void Constructor_WithNullTexts_ThrowsArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TextCollectionNode<List<string>>(null!));
    }

    [Fact]
    public void Constructor_WithEmptyCollection_InitializesCorrectly()
    {
        // Arrange
        var texts = new List<string>();

        // Act
        var node = new TextCollectionNode<List<string>>(texts);

        // Assert
        Assert.Equal(0, node.TotalLength);
        Assert.Equal(0, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithSingleText_InitializesCorrectly()
    {
        // Arrange
        var texts = new List<string> { "Hello" };

        // Act
        var node = new TextCollectionNode<List<string>>(texts);

        // Assert
        Assert.Equal(5, node.TotalLength);
        Assert.Equal(0, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMultipleTexts_NoSeparator_InitializesCorrectly()
    {
        // Arrange
        var texts = new List<string> { "Hello", "World", "Test" };

        // Act
        var node = new TextCollectionNode<List<string>>(texts);

        // Assert
        Assert.Equal(14, node.TotalLength); // "HelloWorldTest"
        Assert.Equal(0, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithMultipleTexts_WithSeparator_InitializesCorrectly()
    {
        // Arrange
        var texts = new List<string> { "Hello", "World", "Test" };
        const string separator = ", ";

        // Act
        var node = new TextCollectionNode<List<string>>(texts, separator);

        // Assert
        Assert.Equal(18, node.TotalLength); // "Hello, World, Test"
        Assert.Equal(0, node.SyntaxLength);
    }

    [Fact]
    public void Constructor_WithNullTextInCollection_SkipsNullValues()
    {
        // Arrange
        var texts = new List<string?> { "Hello", null, "World" };

        // Act
        var node = new TextCollectionNode<List<string?>>(texts);

        // Assert
        Assert.Equal(10, node.TotalLength); // "HelloWorld"
        Assert.Equal(0, node.SyntaxLength);
    }

    [Theory]
    [InlineData(new[] { "A" }, null, 1)]
    [InlineData(new[] { "Hello", "World" }, null, 10)]
    [InlineData(new[] { "Hello", "World" }, " ", 11)]
    [InlineData(new[] { "Hello", "World" }, ", ", 12)]
    [InlineData(new[] { "A", "B", "C" }, "-", 5)]
    [InlineData(new string[0], null, 0)]
    [InlineData(new string[0], ", ", 0)]
    public void TotalLength_WithVariousInputs_ReturnsCorrectLength(string[] texts, string? separator, int expectedLength)
    {
        // Arrange & Act
        var node = new TextCollectionNode<string[]>(texts, separator);

        // Assert
        Assert.Equal(expectedLength, node.TotalLength);
    }

    [Fact]
    public void SyntaxLength_AlwaysReturnsZero()
    {
        // Arrange
        var texts = new List<string> { "Hello", "World" };

        // Act
        var node = new TextCollectionNode<List<string>>(texts, ", ");

        // Assert
        Assert.Equal(0, node.SyntaxLength);
    }

    [Fact]
    public void ToString_WithNoSeparator_ReturnsCorrectString()
    {
        // Arrange
        var texts = new List<string> { "Hello", "World", "Test" };
        var node = new TextCollectionNode<List<string>>(texts);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal("HelloWorldTest", result);
    }

    [Fact]
    public void ToString_WithSeparator_ReturnsCorrectString()
    {
        // Arrange
        var texts = new List<string> { "Hello", "World", "Test" };
        var node = new TextCollectionNode<List<string>>(texts, ", ");

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal("Hello, World, Test", result);
    }

    [Fact]
    public void ToString_WithEmptyCollection_ReturnsEmptyString()
    {
        // Arrange
        var texts = new List<string>();
        var node = new TextCollectionNode<List<string>>(texts);

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToString_WithNullValuesInCollection_SkipsNulls()
    {
        // Arrange
        var texts = new List<string?> { "Hello", null, "World" };
        var node = new TextCollectionNode<List<string?>>(texts, " ");

        // Act
        var result = node.ToString();

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void CopyTo_WithNoSeparator_CopiesCorrectly()
    {
        // Arrange
        var texts = new List<string> { "Hello", "World" };
        var node = new TextCollectionNode<List<string>>(texts);
        var destination = new char[10];

        // Act
        var copiedLength = node.CopyTo(destination);

        // Assert
        Assert.Equal(10, copiedLength);
        Assert.Equal("HelloWorld", new string(destination));
    }

    [Fact]
    public void CopyTo_WithSeparator_CopiesCorrectly()
    {
        // Arrange
        var texts = new List<string> { "Hello", "World" };
        var node = new TextCollectionNode<List<string>>(texts, " ");
        var destination = new char[11];

        // Act
        var copiedLength = node.CopyTo(destination);

        // Assert
        Assert.Equal(11, copiedLength);
        Assert.Equal("Hello World", new string(destination));
    }

    [Fact]
    public void CopyTo_WithTooSmallDestination_ThrowsArgumentException()
    {
        // Arrange
        var texts = new List<string> { "Hello", "World" };
        var node = new TextCollectionNode<List<string>>(texts);
        var destination = new char[5]; // Too small

        // Act & Assert
        Assert.Throws<ArgumentException>(() => node.CopyTo(destination));
    }

    [Fact]
    public void CopyTo_WithNullValuesInCollection_SkipsNulls()
    {
        // Arrange
        var texts = new List<string?> { "Hello", null, "World" };
        var node = new TextCollectionNode<List<string?>>(texts, " ");
        var destination = new char[11];

        // Act
        var copiedLength = node.CopyTo(destination);

        // Assert
        Assert.Equal(11, copiedLength);
        Assert.Equal("Hello World", new string(destination));
    }

    [Theory]
    [InlineData(0, 'H')]
    [InlineData(4, 'o')]
    [InlineData(5, 'W')]
    [InlineData(9, 'd')]
    public void TryGetChar_WithNoSeparator_ReturnsCorrectCharacter(int index, char expectedChar)
    {
        // Arrange
        var texts = new List<string> { "Hello", "World" };
        var node = new TextCollectionNode<List<string>>(texts);

        // Act
        var success = node.TryGetChar(index, out var character);

        // Assert
        Assert.True(success);
        Assert.Equal(expectedChar, character);
    }

    [Theory]
    [InlineData(0, 'H')]
    [InlineData(4, 'o')]
    [InlineData(5, ' ')]
    [InlineData(6, 'W')]
    [InlineData(10, 'd')]
    public void TryGetChar_WithSeparator_ReturnsCorrectCharacter(int index, char expectedChar)
    {
        // Arrange
        var texts = new List<string> { "Hello", "World" };
        var node = new TextCollectionNode<List<string>>(texts, " ");

        // Act
        var success = node.TryGetChar(index, out var character);

        // Assert
        Assert.True(success);
        Assert.Equal(expectedChar, character);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(10)]
    [InlineData(100)]
    public void TryGetChar_WithInvalidIndex_NoSeparator_ReturnsFalse(int index)
    {
        // Arrange
        var texts = new List<string> { "Hello", "World" };
        var node = new TextCollectionNode<List<string>>(texts);

        // Act
        var success = node.TryGetChar(index, out var character);

        // Assert
        Assert.False(success);
        Assert.Equal('\0', character);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(11)]
    [InlineData(100)]
    public void TryGetChar_WithInvalidIndex_WithSeparator_ReturnsFalse(int index)
    {
        // Arrange
        var texts = new List<string> { "Hello", "World" };
        var node = new TextCollectionNode<List<string>>(texts, " ");

        // Act
        var success = node.TryGetChar(index, out var character);

        // Assert
        Assert.False(success);
        Assert.Equal('\0', character);
    }

    [Fact]
    public void TryGetChar_WithNullValuesInCollection_SkipsNulls()
    {
        // Arrange
        var texts = new List<string?> { "Hello", null, "World" };
        var node = new TextCollectionNode<List<string?>>(texts, " ");

        // Act & Assert
        Assert.True(node.TryGetChar(0, out var char0));
        Assert.Equal('H', char0);

        Assert.True(node.TryGetChar(5, out var char5));
        Assert.Equal(' ', char5);

        Assert.True(node.TryGetChar(6, out var char6));
        Assert.Equal('W', char6);

        Assert.True(node.TryGetChar(10, out var char10));
        Assert.Equal('d', char10);
    }

    [Fact]
    public void TryGetChar_WithEmptyCollection_ReturnsFalse()
    {
        // Arrange
        var texts = new List<string>();
        var node = new TextCollectionNode<List<string>>(texts);

        // Act
        var success = node.TryGetChar(0, out var character);

        // Assert
        Assert.False(success);
        Assert.Equal('\0', character);
    }

    [Fact]
    public void TryGetChar_WithMultiCharacterSeparator_ReturnsCorrectCharacters()
    {
        // Arrange
        var texts = new List<string> { "A", "B", "C" };
        var node = new TextCollectionNode<List<string>>(texts, " -> ");

        // Act & Assert
        Assert.True(node.TryGetChar(0, out var char0));
        Assert.Equal('A', char0);

        Assert.True(node.TryGetChar(1, out var char1));
        Assert.Equal(' ', char1);

        Assert.True(node.TryGetChar(2, out var char2));
        Assert.Equal('-', char2);

        Assert.True(node.TryGetChar(3, out var char3));
        Assert.Equal('>', char3);

        Assert.True(node.TryGetChar(4, out var char4));
        Assert.Equal(' ', char4);

        Assert.True(node.TryGetChar(5, out var char5));
        Assert.Equal('B', char5);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(", ")]
    [InlineData(" -> ")]
    [InlineData("|||")]
    public void Constructor_WithVariousSeparators_HandlesCorrectly(string separator)
    {
        // Arrange
        var texts = new List<string> { "A", "B" };

        // Act
        var node = new TextCollectionNode<List<string>>(texts, separator);

        // Assert
        var expected = string.IsNullOrEmpty(separator) ? "AB" : $"A{separator}B";
        Assert.Equal(expected, node.ToString());
        Assert.Equal(expected.Length, node.TotalLength);
    }

    [Fact]
    public void Constructor_WithReadOnlyList_WorksCorrectly()
    {
        // Arrange
        IReadOnlyList<string> texts = new[] { "Hello", "World" };

        // Act
        var node = new TextCollectionNode<IReadOnlyList<string>>(texts, " ");

        // Assert
        Assert.Equal("Hello World", node.ToString());
        Assert.Equal(11, node.TotalLength);
    }

    [Fact]
    public void Constructor_WithArray_WorksCorrectly()
    {
        // Arrange
        var texts = new[] { "Hello", "World" };

        // Act
        var node = new TextCollectionNode<string[]>(texts, " ");

        // Assert
        Assert.Equal("Hello World", node.ToString());
        Assert.Equal(11, node.TotalLength);
    }
}
