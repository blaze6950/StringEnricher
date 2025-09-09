namespace StringEnricher.Tests.Helpers.Html;

public class HtmlTests
{
    [Fact]
    public void Test_NothingToEscape()
    {
        // Arrange
        const string expectedString = "string to escape";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape("string to escape");

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expectedString, result);
    }

    [Fact]
    public void Test_Escape_AllEscapableCharacters()
    {
        // Arrange
        const string input = "<>&";
        const string expected = "&lt;&gt;&amp;";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_MixedCharacters()
    {
        // Arrange
        const string input = "Hello<World>";
        const string expected = "Hello&lt;World&gt;";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_RepeatedEscapableCharacters()
    {
        // Arrange
        const string input = "<<>>&";
        const string expected = "&lt;&lt;&gt;&gt;&amp;";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

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
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_NullInput()
    {
        // Arrange
        string input = null;
        string expected = null;

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_CharactersAtStartMiddleEnd()
    {
        // Arrange
        const string input = "<start middle> end&";
        const string expected = "&lt;start middle&gt; end&amp;";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_ConsecutiveEscapableCharacters()
    {
        // Arrange
        const string input = "<<<";
        const string expected = "&lt;&lt;&lt;";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_NonEscapableUnicodeCharacters()
    {
        // Arrange
        const string input = "Привет мир<>";
        const string expected = "Привет мир&lt;&gt;";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_LessThanOnly()
    {
        // Arrange
        const string input = "Hello < World";
        const string expected = "Hello &lt; World";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_GreaterThanOnly()
    {
        // Arrange
        const string input = "Hello > World";
        const string expected = "Hello &gt; World";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_AmpersandOnly()
    {
        // Arrange
        const string input = "Tom & Jerry";
        const string expected = "Tom &amp; Jerry";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_HtmlTags()
    {
        // Arrange
        const string input = "<div>Content</div>";
        const string expected = "&lt;div&gt;Content&lt;/div&gt;";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_ComplexHtmlWithAttributes()
    {
        // Arrange
        const string input = "<a href=\"http://example.com\">Link & Text</a>";
        const string expected = "&lt;a href=\"http://example.com\"&gt;Link &amp; Text&lt;/a&gt;";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_OnlyAmpersands()
    {
        // Arrange
        const string input = "&&&";
        const string expected = "&amp;&amp;&amp;";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Escape_MixedWithNormalText()
    {
        // Arrange
        const string input = "This is a test < with & some > special characters.";
        const string expected = "This is a test &lt; with &amp; some &gt; special characters.";

        // Act
        var result = StringEnricher.Helpers.Html.Html.Escape(input);

        // Assert
        Assert.Equal(expected, result);
    }
}
