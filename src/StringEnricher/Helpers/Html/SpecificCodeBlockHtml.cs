using StringEnricher.Nodes;
using StringEnricher.Nodes.Html.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Html;

/// <summary>
/// Provides methods to apply specific code block styling in HTML format with language class.
/// Example: "&lt;pre&gt;&lt;code class="language-csharp"&gt;code block&lt;/code&gt;&lt;/pre&gt;"
/// </summary>
public static class SpecificCodeBlockHtml
{
    /// <summary>
    /// Applies specific code block style to the given styled code block and language.
    /// </summary>
    /// <param name="codeBlock">The styled code block text.</param>
    /// <param name="language">The language to be used in the class attribute.</param>
    /// <typeparam name="T">The type of the inner style that implements <see cref="INode"/>.</typeparam>
    /// <returns>A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> wrapping the provided styled code block and language.</returns>
    public static SpecificCodeBlockNode<T> Apply<T>(T codeBlock, string language) where T : INode =>
        SpecificCodeBlockNode<T>.Apply(codeBlock, language);

    #region Overloads for Common Types

    /// <summary>
    /// Applies specific code block style to the given text and language.
    /// </summary>
    /// <param name="codeBlock">The code block text to be wrapped with code block HTML tags.</param>
    /// <param name="language">The language to be used in the class attribute.</param>
    /// <returns>A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> wrapping the provided code block and language.</returns>
    public static SpecificCodeBlockNode<PlainTextNode> Apply(string codeBlock, string language) =>
        SpecificCodeBlockNode<PlainTextNode>.Apply(codeBlock, language);

    /// <summary>
    /// Applies specific code block style to the given integer code block.
    /// </summary>
    /// <param name="codeBlock">The integer to be styled as a code block.</param>
    /// <param name="language">The language to be used in the class attribute.</param>
    /// <returns>A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> containing the styled integer and language.</returns>
    public static SpecificCodeBlockNode<IntegerNode> Apply(int codeBlock, string language) =>
        SpecificCodeBlockNode<IntegerNode>.Apply(codeBlock, language);

    #endregion
}