using StringEnricher.Nodes;
using StringEnricher.Nodes.MarkdownV2.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to apply strikethrough style to text in MarkdownV2 format.
/// Example: "~strikethrough text~"
/// </summary>
public static class StrikethroughMarkdownV2
{
    /// <summary>
    /// Applies strikethrough style to the given text using plain text style as inner style.
    /// </summary>
    /// <param name="text">
    /// The text to be styled with strikethrough syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the specified text.
    /// </returns>
    public static StrikethroughNode<PlainTextNode> Apply(string text) =>
        StrikethroughNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies strikethrough style to the given inner style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with strikethrough syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the specified inner style.
    /// </returns>
    public static StrikethroughNode<T> Apply<T>(T style) where T : INode =>
        StrikethroughNode<T>.Apply(style);

    /// <summary>
    /// Applies strikethrough style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled with strikethrough.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static StrikethroughNode<IntegerNode> Apply(int integer) =>
        StrikethroughNode<IntegerNode>.Apply(integer);
}