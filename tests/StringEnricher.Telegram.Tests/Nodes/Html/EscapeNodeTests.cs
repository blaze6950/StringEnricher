using StringEnricher.Telegram.Helpers.Html;

namespace StringEnricher.Telegram.Tests.Nodes.Html;

public class EscapeNodeTests
{
    [Fact]
    public void Test_NothingToEscape()
    {
        // Arrange
        const string expectedString = "string to escape";

        // Act
        var styledEscape = EscapeHtml.Apply("string to escape").ToString();

        // Assert
        Assert.NotNull(styledEscape);
        Assert.NotEmpty(styledEscape);
        Assert.Equal(expectedString, styledEscape);
    }

    [Fact]
    public void Test_Escape_AllEscapableCharacters()
    {
        // Arrange
        const string input = "<>&";
        const string expected = "&lt;&gt;&amp;";

        // Act
        var result = EscapeHtml.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_MixedCharacters()
    {
        // Arrange
        const string input = "Hello<World>&Friends!";
        const string expected = "Hello&lt;World&gt;&amp;Friends!";

        // Act
        var result = EscapeHtml.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_RepeatedEscapableCharacters()
    {
        // Arrange
        const string input = "<<>>&&&";
        const string expected = "&lt;&lt;&gt;&gt;&amp;&amp;&amp;";

        // Act
        var result = EscapeHtml.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_EmptyString()
    {
        // Arrange
        const string input = "";
        const string expected = "";

        // Act
        var result = EscapeHtml.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_NullInput()
    {
        // Arrange
        string input = null;

        // Act
        var exception = Record.Exception(() => EscapeHtml.Apply(input).ToString());

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Test_Escape_CharactersAtStartMiddleEnd()
    {
        // Arrange
        const string input = "<start middle> end&";
        const string expected = "&lt;start middle&gt; end&amp;";

        // Act
        var result = EscapeHtml.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_ConsecutiveEscapableCharacters()
    {
        // Arrange
        const string input = ">>>";
        const string expected = "&gt;&gt;&gt;";

        // Act
        var result = EscapeHtml.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_NonEscapableUnicodeCharacters()
    {
        // Arrange
        const string input = "Привет мир & друзья!";
        const string expected = "Привет мир &amp; друзья!";

        // Act
        var result = EscapeHtml.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_HtmlTagLikeContent()
    {
        // Arrange
        const string input = "<div>Hello & welcome</div>";
        const string expected = "&lt;div&gt;Hello &amp; welcome&lt;/div&gt;";

        // Act
        var result = EscapeHtml.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_OnlyLessThan()
    {
        // Arrange
        const string input = "Hello < World";
        const string expected = "Hello &lt; World";

        // Act
        var result = EscapeHtml.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_OnlyGreaterThan()
    {
        // Arrange
        const string input = "Hello > World";
        const string expected = "Hello &gt; World";

        // Act
        var result = EscapeHtml.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_OnlyAmpersand()
    {
        // Arrange
        const string input = "Hello & World";
        const string expected = "Hello &amp; World";

        // Act
        var result = EscapeHtml.Apply(input).ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_TryGetChar_WithinValidRange_ShouldReturnTrueAndCurrentChar()
    {
        // Arrange
        var escapeStyle = EscapeHtml.Apply("Hello<World>&");
        const string expectedString = "Hello&lt;World&gt;&amp;";
        var totalLength = expectedString.Length;

        // Act & Assert
        for (var i = 0; i < totalLength; i++)
        {
            var result = escapeStyle.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expectedString[i], actualChar);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(23)]
    public void Test_TryGetChar_OutOfRange_ShouldReturnFalse(int index)
    {
        // Arrange
        var escapeStyle = EscapeHtml.Apply("Hello<World>&");
        const char expectedChar = '\0';

        // Act
        var result = escapeStyle.TryGetChar(index, out var actualChar);

        // Assert
        Assert.False(result);
        Assert.Equal(expectedChar, actualChar);
    }

    [Fact]
    public void Test_TryGetChar_AllEscapableCharacters_ShouldReturnCorrectEscapedSequence()
    {
        // Arrange
        var escapeStyle = EscapeHtml.Apply("<>&");
        const string expectedString = "&lt;&gt;&amp;";
        var totalLength = expectedString.Length;

        // Act & Assert
        for (var i = 0; i < totalLength; i++)
        {
            var result = escapeStyle.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expectedString[i], actualChar);
        }
    }

    [Fact]
    public void Test_TryGetChar_EmptyString_ShouldHandleCorrectly()
    {
        // Arrange
        var escapeStyle = EscapeHtml.Apply("");

        // Act
        var result = escapeStyle.TryGetChar(0, out var actualChar);

        // Assert
        Assert.False(result);
        Assert.Equal('\0', actualChar);
    }

    [Fact]
    public void Test_TryGetChar_MixedContent_ShouldReturnCorrectSequence()
    {
        // Arrange
        var escapeStyle = EscapeHtml.Apply("start<middle>end&");
        const string expectedString = "start&lt;middle&gt;end&amp;";
        var totalLength = expectedString.Length;

        // Act & Assert
        for (var i = 0; i < totalLength; i++)
        {
            var result = escapeStyle.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expectedString[i], actualChar);
        }
    }

    [Fact]
    public void Test_TryGetChar_SingleCharacterEscaping_LessThan()
    {
        // Arrange
        var escapeStyle = EscapeHtml.Apply("<");
        const string expectedString = "&lt;";

        // Act & Assert
        for (var i = 0; i < expectedString.Length; i++)
        {
            var result = escapeStyle.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expectedString[i], actualChar);
        }
    }

    [Fact]
    public void Test_TryGetChar_SingleCharacterEscaping_GreaterThan()
    {
        // Arrange
        var escapeStyle = EscapeHtml.Apply(">");
        const string expectedString = "&gt;";

        // Act & Assert
        for (var i = 0; i < expectedString.Length; i++)
        {
            var result = escapeStyle.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expectedString[i], actualChar);
        }
    }

    [Fact]
    public void Test_TryGetChar_SingleCharacterEscaping_Ampersand()
    {
        // Arrange
        var escapeStyle = EscapeHtml.Apply("&");
        const string expectedString = "&amp;";

        // Act & Assert
        for (var i = 0; i < expectedString.Length; i++)
        {
            var result = escapeStyle.TryGetChar(i, out var actualChar);

            Assert.True(result);
            Assert.Equal(expectedString[i], actualChar);
        }
    }

    [Fact]
    public void Test_SyntaxLength_NoEscaping_ShouldBeZero()
    {
        // Arrange
        var escapeStyle = EscapeHtml.Apply("Hello World");

        // Act
        var syntaxLength = escapeStyle.SyntaxLength;

        // Assert
        Assert.Equal(0, syntaxLength);
    }

    [Fact]
    public void Test_SyntaxLength_WithEscaping_ShouldCalculateCorrectly()
    {
        // Arrange
        var escapeStyle = EscapeHtml.Apply("<>&");
        // < becomes &lt; (4 chars), > becomes &gt; (4 chars), & becomes &amp; (5 chars)
        const int expectedSyntaxLength = 4 + 4 + 5;

        // Act
        var syntaxLength = escapeStyle.SyntaxLength;

        // Assert
        Assert.Equal(expectedSyntaxLength, syntaxLength);
    }

    [Fact]
    public void Test_TotalLength_ShouldBeSumOfInnerAndSyntaxLength()
    {
        // Arrange
        const string input = "Hello<World>&";
        // Count how many characters are replaced (\<, \>, &)
        var escapedCount = 0;
        foreach (var ch in input)
        {
            if (ch is '<' or '>' or '&')
            {
                escapedCount++;
            }
        }
        var escapeStyle = EscapeHtml.Apply(input);
        
        // Act
        var totalLength = escapeStyle.TotalLength;
        var innerLength = escapeStyle.InnerLength;
        var syntaxLength = escapeStyle.SyntaxLength;

        // Assert
        Assert.Equal(innerLength - escapedCount + syntaxLength, totalLength);
        Assert.Equal(input.Length, innerLength);
    }

    [Fact]
    public void Test_CopyTo_WithSufficientSpace_ShouldCopyCorrectly()
    {
        // Arrange
        var escapeStyle = EscapeHtml.Apply("Hello<World>&");
        const string expected = "Hello&lt;World&gt;&amp;";
        var destination = new char[expected.Length];

        // Act
        var copiedLength = escapeStyle.CopyTo(destination);

        // Assert
        Assert.Equal(expected.Length, copiedLength);
        Assert.Equal(expected, new string(destination));
    }

    [Fact]
    public void Test_CopyTo_WithInsufficientSpace_ShouldThrowException()
    {
        // Arrange
        var escapeStyle = EscapeHtml.Apply("Hello<World>&");
        var destination = new char[5]; // Too small

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => escapeStyle.CopyTo(destination));
        Assert.Contains("Destination span is not large enough to hold the written characters.", exception.Message);
    }
}
