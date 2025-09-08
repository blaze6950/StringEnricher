using StringEnricher.Nodes;
using StringEnricher.Nodes.MarkdownV2.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to create inline code styles in MarkdownV2 format.
/// Example: "`inline code`"
/// </summary>
public static class InlineCodeMarkdownV2
{
    /// <summary>
    /// Applies inline code style to the given style
    /// </summary>
    /// <param name="style">
    /// The style to be styled as inline code.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// An instance of <see cref="InlineCodeNode{TInner}"/> containing the provided style wrapped with inline code syntax.
    /// </returns>
    public static InlineCodeNode<T> Apply<T>(T style) where T : INode =>
        InlineCodeNode<T>.Apply(style);

    #region Overloads for Common Types

    /// <summary>
    /// Applies inline code style to the given text
    /// </summary>
    /// <param name="text">
    /// The text to be styled as inline code.
    /// </param>
    /// <returns>
    /// An instance of <see cref="InlineCodeNode{TInner}"/> containing the provided text styled as inline code.
    /// </returns>
    public static InlineCodeNode<PlainTextNode> Apply(string text) =>
        InlineCodeNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies inline code style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as inline code.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static InlineCodeNode<IntegerNode> Apply(int integer) =>
        InlineCodeNode<IntegerNode>.Apply(integer);

    /// <summary>
    /// Applies inline code style to the given long integer.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as inline code.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="InlineCodeNode{TInner}"/> containing the styled long integer.
    /// </returns>
    public static InlineCodeNode<LongNode> Apply(long @long) =>
        InlineCodeNode<LongNode>.Apply(@long);

    #endregion
}