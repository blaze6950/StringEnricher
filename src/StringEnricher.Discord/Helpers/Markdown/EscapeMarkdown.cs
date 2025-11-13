using StringEnricher.Discord.Nodes.Markdown;
using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Discord.Helpers.Markdown;

/// <summary>
/// Escapes Discord markdown reserved characters for the given text.
/// Example: "_string_ *with* ~Markdown~ `reserved` |characters|" => "\_string\_ \*with\* \~Markdown\~ \`reserved\` \|characters\|"
/// </summary>
public static class EscapeMarkdown
{
    /// <summary>
    /// Escapes Discord markdown reserved characters for the given text.
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
    /// Escapes Discord markdown reserved characters for the given style.
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