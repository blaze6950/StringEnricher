using StringEnricher.Nodes;
using StringEnricher.Nodes.MarkdownV2.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to create specific code block styles in MarkdownV2 format.
/// Example: "```language\ncode block\n```"
/// </summary>
public static class SpecificCodeBlockMarkdownV2
{
    /// <summary>
    /// Applies specific code block style to the given code block and language using the specified style type.
    /// </summary>
    /// <param name="codeBlock">
    /// The inner style representing the code block content.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that will be wrapped with specific code block syntax.
    /// </typeparam>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    public static SpecificCodeBlockNode<T> Apply<T>(T codeBlock, string language) where T : INode =>
        SpecificCodeBlockNode<T>.Apply(codeBlock, language);

    #region Overloads for Common Typ

    /// <summary>
    /// Applies specific code block style to the given code block and language using plain text style.
    /// </summary>
    /// <param name="codeBlock">
    /// The code block content.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    public static SpecificCodeBlockNode<PlainTextNode> Apply(string codeBlock, string language) =>
        SpecificCodeBlockNode<PlainTextNode>.Apply(codeBlock, language);

    /// <summary>
    /// Applies specific code block style to the given integer code block and language.
    /// </summary>
    /// <param name="codeBlock">
    /// The integer to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    public static SpecificCodeBlockNode<IntegerNode> Apply(int codeBlock, string language) =>
        SpecificCodeBlockNode<IntegerNode>.Apply(codeBlock, language);

    /// <summary>
    /// Applies specific code block style to the given long integer code block and language.
    /// </summary>
    /// <param name="codeBlock">
    /// The long integer to be styled as a code block.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    public static SpecificCodeBlockNode<LongNode> Apply(long codeBlock, string language) =>
        SpecificCodeBlockNode<LongNode>.Apply(codeBlock, language);

    #endregion
}