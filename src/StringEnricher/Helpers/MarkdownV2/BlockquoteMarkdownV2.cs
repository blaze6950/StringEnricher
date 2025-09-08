using StringEnricher.Nodes;
using StringEnricher.Nodes.MarkdownV2.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to apply blockquote style in MarkdownV2 format.
/// Example: "&gt;blockquote text example\n&gt;new blockquote line"
/// </summary>
public static class BlockquoteMarkdownV2
{
    /// <summary>
    /// Applies blockquote style to the given style.
    /// </summary>
    /// <param name="style">
    /// The style to be wrapped with blockquote syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled text.
    /// </returns>
    public static BlockquoteNode<T> Apply<T>(T style) where T : INode =>
        BlockquoteNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies blockquote style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be styled as a blockquote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled text.
    /// </returns>
    public static BlockquoteNode<PlainTextNode> Apply(string text) =>
        BlockquoteNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies blockquote style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as a blockquote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static BlockquoteNode<IntegerNode> Apply(int integer) =>
        BlockquoteNode<IntegerNode>.Apply(integer);

    /// <summary>
    /// Applies blockquote style to the given long integer.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as a blockquote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static BlockquoteNode<LongNode> Apply(long @long) =>
        BlockquoteNode<LongNode>.Apply(@long);

    #endregion
}