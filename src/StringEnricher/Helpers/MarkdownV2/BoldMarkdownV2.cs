using StringEnricher.Nodes;
using StringEnricher.Nodes.MarkdownV2.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to apply bold styling in MarkdownV2 format.
/// Example: "*bold text*"
/// </summary>
public static class BoldMarkdownV2
{
    /// <summary>
    /// Applies bold style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with bold syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static BoldNode<PlainTextNode> Apply(string text) =>
        BoldNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies bold style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with bold syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static BoldNode<T> Apply<T>(T style) where T : INode =>
        BoldNode<T>.Apply(style);

    /// <summary>
    /// Applies bold style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as a bold.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static BoldNode<IntegerNode> Apply(int integer) =>
        BoldNode<IntegerNode>.Apply(integer);
}