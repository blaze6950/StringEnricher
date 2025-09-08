using StringEnricher.Nodes;
using StringEnricher.Nodes.Html.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Html;

/// <summary>
/// Provides methods to apply bold styling in HTML format.
/// Example: "&lt;b&gt;bold text&lt;/b&gt;"
/// </summary>
public static class BoldHtml
{
    /// <summary>
    /// Applies bold style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with bold HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static BoldNode<T> Apply<T>(T style) where T : INode =>
        BoldNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies bold style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with bold HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static BoldNode<PlainTextNode> Apply(string text) =>
        BoldNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies bold style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as bold.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static BoldNode<IntegerNode> Apply(int integer) =>
        BoldNode<IntegerNode>.Apply(integer);

    /// <summary>
    /// Applies bold style to the given long integer.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as bold.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BoldNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static BoldNode<LongNode> Apply(long @long) =>
        BoldNode<LongNode>.Apply(@long);

    #endregion
}