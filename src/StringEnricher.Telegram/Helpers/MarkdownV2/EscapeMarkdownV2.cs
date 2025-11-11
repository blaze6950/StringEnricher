using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;
using StringEnricher.Telegram.Nodes.MarkdownV2;

namespace StringEnricher.Telegram.Helpers.MarkdownV2;

/// <summary>
/// Escapes MarkdownV2 reserved characters for the given text.
/// Example: "_string_ *with* ~MarkdownV2~ `reserved` !characters!" => "\_string\_ \*with\* \~MarkdownV2\~ \`reserved\` \!characters\!"
/// </summary>
public static class EscapeMarkdownV2
{
    /// <summary>
    /// Escapes MarkdownV2 reserved characters for the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be escaped.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="EscapeNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static EscapeNode<PlainTextNode> Apply(string text) =>
        EscapeNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Escapes MarkdownV2 reserved characters for the given text.
    /// </summary>
    /// <param name="style">
    /// The inner style to be escaped.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="EscapeNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static EscapeNode<T> Apply<T>(T style) where T : INode =>
        EscapeNode<T>.Apply(style);
}