using StringEnricher.Nodes;
using StringEnricher.Nodes.MarkdownV2.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to apply code block styling in MarkdownV2 format.
/// Example: "```\ncode block\n```"
/// </summary>
public static class CodeBlockMarkdownV2
{
    /// <summary>
    /// Applies the code block style to the given plain text code block.
    /// </summary>
    /// <param name="codeBlock">
    /// The plain text code block to be wrapped with code block syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> wrapping the provided plain text.
    /// </returns>
    public static CodeBlockNode<PlainTextNode> Apply(string codeBlock) =>
        CodeBlockNode<PlainTextNode>.Apply(codeBlock);

    /// <summary>
    /// Applies the code block style to the given styled code block.
    /// </summary>
    /// <param name="codeBlock">
    /// The styled code block to be wrapped with code block syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> wrapping the provided style.
    /// </returns>
    public static CodeBlockNode<T> Apply<T>(T codeBlock) where T : INode =>
        CodeBlockNode<T>.Apply(codeBlock);

    /// <summary>
    /// Applies code block style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as a code block.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static CodeBlockNode<IntegerNode> Apply(int integer) =>
        CodeBlockNode<IntegerNode>.Apply(integer);
}