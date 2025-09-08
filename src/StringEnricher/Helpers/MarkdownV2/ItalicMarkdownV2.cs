using StringEnricher.Nodes;
using StringEnricher.Nodes.MarkdownV2.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to apply italic style to text in MarkdownV2 format.
/// Example: "_italic text_"
/// </summary>
public static class ItalicMarkdownV2
{
    /// <summary>
    /// Applies italic style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be styled in italic.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static ItalicNode<PlainTextNode> Apply(string text) =>
        ItalicNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies italic style to the given style.
    /// </summary>
    /// <param name="style">
    /// The style to be wrapped with italic syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> wrapping the provided style.
    /// </returns>
    public static ItalicNode<T> Apply<T>(T style) where T : INode =>
        ItalicNode<T>.Apply(style);

    /// <summary>
    /// Applies italic style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as italic.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static ItalicNode<IntegerNode> Apply(int integer) =>
        ItalicNode<IntegerNode>.Apply(integer);
}