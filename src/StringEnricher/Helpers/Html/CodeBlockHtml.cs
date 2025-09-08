using StringEnricher.Nodes;
using StringEnricher.Nodes.Html.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Html;

/// <summary>
/// Provides methods to apply code block styling in HTML format.
/// Example: "&lt;pre&gt;code block&lt;/pre&gt;"
/// </summary>
public static class CodeBlockHtml
{
    /// <summary>
    /// Applies code block style to the given style.
    /// </summary>
    /// <param name="codeBlock">
    /// The inner style to be wrapped with code block HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static CodeBlockNode<T> Apply<T>(T codeBlock) where T : INode =>
        CodeBlockNode<T>.Apply(codeBlock);

    #region Overloads for Common Types

    /// <summary>
    /// Applies code block style to the given text.
    /// </summary>
    /// <param name="codeBlock">
    /// The text to be wrapped with code block HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static CodeBlockNode<PlainTextNode> Apply(string codeBlock) =>
        CodeBlockNode<PlainTextNode>.Apply(codeBlock);

    /// <summary>
    /// Applies code block style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as a code block.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static CodeBlockNode<IntegerNode> Apply(int integer) =>
        CodeBlockNode<IntegerNode>.Apply(integer);

    /// <summary>
    /// Applies code block style to the given long integer.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as a code block.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static CodeBlockNode<LongNode> Apply(long @long) =>
        CodeBlockNode<LongNode>.Apply(@long);

    #endregion
}