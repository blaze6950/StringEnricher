using StringEnricher.Nodes;
using StringEnricher.Nodes.MarkdownV2.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to apply spoiler styling in MarkdownV2 format.
/// Example: "||spoiler text||"
/// </summary>
public static class SpoilerMarkdownV2
{
    /// <summary>
    /// Applies spoiler styling to the given text using plain text style as the inner style.
    /// </summary>
    /// <param name="text">
    /// The text to be styled as a spoiler.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpoilerNode{TInner}"/> containing the styled text.
    /// </returns>
    public static SpoilerNode<PlainTextNode> Apply(string text) =>
        SpoilerNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies spoiler styling to the given style.
    /// </summary>
    /// <param name="style">
    /// The style to be wrapped with spoiler syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="SpoilerNode{TInner}"/> containing the styled text.
    /// </returns>
    public static SpoilerNode<T> Apply<T>(T style) where T : INode =>
        SpoilerNode<T>.Apply(style);

    /// <summary>
    /// Applies spoiler styling to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as spoiler.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpoilerNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static SpoilerNode<IntegerNode> Apply(int integer) =>
        SpoilerNode<IntegerNode>.Apply(integer);
}