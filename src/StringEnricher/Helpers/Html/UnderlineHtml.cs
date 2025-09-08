using StringEnricher.Nodes;
using StringEnricher.Nodes.Html.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Html;

/// <summary>
/// Provides methods to apply underline styling in HTML format.
/// Example: "&lt;u&gt;underlined text&lt;/u&gt;"
/// </summary>
public static class UnderlineHtml
{
    /// <summary>
    /// Applies underline style to the given style.
    /// </summary>
    /// <param name="style">The inner style to be wrapped with underline HTML tags.</param>
    /// <typeparam name="T">The type of the inner style that implements <see cref="INode"/>.</typeparam>
    /// <returns>A new instance of <see cref="UnderlineNode{TInner}"/> wrapping the provided inner style.</returns>
    public static UnderlineNode<T> Apply<T>(T style) where T : INode =>
        UnderlineNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies underline style to the given text.
    /// </summary>
    /// <param name="text">The text to be wrapped with underline HTML tags.</param>
    /// <returns>A new instance of <see cref="UnderlineNode{TInner}"/> wrapping the provided text.</returns>
    public static UnderlineNode<PlainTextNode> Apply(string text) =>
        UnderlineNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies underline style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as underlined.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static UnderlineNode<IntegerNode> Apply(int integer) =>
        UnderlineNode<IntegerNode>.Apply(integer);

    /// <summary>
    /// Applies underline style to the given long integer.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as underlined.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static UnderlineNode<LongNode> Apply(long @long) =>
        UnderlineNode<LongNode>.Apply(@long);

    #endregion
}