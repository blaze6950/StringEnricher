using StringEnricher.Nodes;
using StringEnricher.Nodes.Html.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Html;

/// <summary>
/// Provides methods to apply expandable blockquote styling in HTML format.
/// Example: "&lt;blockquote expandable&gt;quoted text&lt;/blockquote&gt;"
/// </summary>
public static class ExpandableBlockquoteHtml
{
    /// <summary>
    /// Applies expandable blockquote style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with expandable blockquote HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="ExpandableBlockquoteNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static ExpandableBlockquoteNode<T> Apply<T>(T style) where T : INode =>
        ExpandableBlockquoteNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies expandable blockquote style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with expandable blockquote HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ExpandableBlockquoteNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static ExpandableBlockquoteNode<PlainTextNode> Apply(string text) =>
        ExpandableBlockquoteNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies expandable blockquote style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as expandable blockquote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ExpandableBlockquoteNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static ExpandableBlockquoteNode<IntegerNode> Apply(int integer) =>
        ExpandableBlockquoteNode<IntegerNode>.Apply(integer);

    /// <summary>
    /// Applies expandable blockquote style to the given long integer.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as expandable blockquote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ExpandableBlockquoteNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static ExpandableBlockquoteNode<LongNode> Apply(long @long) =>
        ExpandableBlockquoteNode<LongNode>.Apply(@long);

    #endregion
}