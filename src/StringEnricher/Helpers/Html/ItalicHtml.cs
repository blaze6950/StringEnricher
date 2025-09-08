using StringEnricher.Nodes;
using StringEnricher.Nodes.Html.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Html;

/// <summary>
/// Provides methods to apply italic styling in HTML format.
/// Example: "&lt;i&gt;italic text&lt;/i&gt;"
/// </summary>
public static class ItalicHtml
{
    /// <summary>
    /// Applies italic style to the given style.
    /// </summary>
    /// <param name="style">The inner style to be wrapped with italic HTML tags.</param>
    /// <typeparam name="T">The type of the inner style that implements <see cref="INode"/>.</typeparam>
    /// <returns>A new instance of <see cref="ItalicNode{TInner}"/> wrapping the provided inner style.</returns>
    public static ItalicNode<T> Apply<T>(T style) where T : INode =>
        ItalicNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies italic style to the given text.
    /// </summary>
    /// <param name="text">The text to be wrapped with italic HTML tags.</param>
    /// <returns>A new instance of <see cref="ItalicNode{TInner}"/> wrapping the provided text.</returns>
    public static ItalicNode<PlainTextNode> Apply(string text) =>
        ItalicNode<PlainTextNode>.Apply(text);

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

    #endregion
}