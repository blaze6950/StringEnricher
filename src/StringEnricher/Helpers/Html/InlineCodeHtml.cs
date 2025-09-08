using StringEnricher.Nodes;
using StringEnricher.Nodes.Html.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Html;

/// <summary>
/// Provides methods to apply inline code styling in HTML format.
/// Example: "&lt;code&gt;inline code&lt;/code&gt;"
/// </summary>
public static class InlineCodeHtml
{
    /// <summary>
    /// Applies inline code style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with inline code HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static InlineCodeNode<T> Apply<T>(T style) where T : INode =>
        InlineCodeNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies inline code style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with inline code HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static InlineCodeNode<PlainTextNode> Apply(string text) =>
        InlineCodeNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies inline code style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as inline code.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static InlineCodeNode<IntegerNode> Apply(int integer) =>
        InlineCodeNode<IntegerNode>.Apply(integer);

    /// <summary>
    /// Applies inline code style to the given long integer.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as inline code.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static InlineCodeNode<LongNode> Apply(long @long) =>
        InlineCodeNode<LongNode>.Apply(@long);

    #endregion
}