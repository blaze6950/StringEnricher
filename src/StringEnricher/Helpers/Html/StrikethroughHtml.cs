using StringEnricher.Nodes;
using StringEnricher.Nodes.Html.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Html;

/// <summary>
/// Provides methods to apply strikethrough styling in HTML format.
/// Example: "&lt;s&gt;strikethrough text&lt;/s&gt;"
/// </summary>
public static class StrikethroughHtml
{
    /// <summary>
    /// Applies strikethrough style to the given style.
    /// </summary>
    /// <param name="style">The inner style to be wrapped with strikethrough HTML tags.</param>
    /// <typeparam name="T">The type of the inner style that implements <see cref="INode"/>.</typeparam>
    /// <returns>A new instance of <see cref="StrikethroughNode{TInner}"/> wrapping the provided inner style.</returns>
    public static StrikethroughNode<T> Apply<T>(T style) where T : INode =>
        StrikethroughNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies strikethrough style to the given text.
    /// </summary>
    /// <param name="text">The text to be wrapped with strikethrough HTML tags.</param>
    /// <returns>A new instance of <see cref="StrikethroughNode{TInner}"/> wrapping the provided text.</returns>
    public static StrikethroughNode<PlainTextNode> Apply(string text) =>
        StrikethroughNode<PlainTextNode>.Apply(text);

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

    /// <summary>
    /// Applies strikethrough style to the given long integer.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled with strikethrough.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static StrikethroughNode<LongNode> Apply(long @long) =>
        StrikethroughNode<LongNode>.Apply(@long);

    #endregion
}