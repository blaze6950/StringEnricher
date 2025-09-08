using StringEnricher.Nodes;
using StringEnricher.Nodes.MarkdownV2.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to apply underline style to text in MarkdownV2 format.
/// Example: "__underline text__"
/// </summary>
public static class UnderlineMarkdownV2
{
    /// <summary>
    /// Applies the underline style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be styled with underline syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> that wraps the provided text.
    /// </returns>
    public static UnderlineNode<PlainTextNode> Apply(string text) =>
        UnderlineNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies the underline style to the given style.
    /// </summary>
    /// <param name="style">
    /// The style to be wrapped with underline syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> that wraps the provided style.
    /// </returns>
    public static UnderlineNode<T> Apply<T>(T style) where T : INode =>
        UnderlineNode<T>.Apply(style);

    /// <summary>
    /// Applies underline style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled with underline.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static UnderlineNode<IntegerNode> Apply(int integer) =>
        UnderlineNode<IntegerNode>.Apply(integer);
}