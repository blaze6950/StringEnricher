using StringEnricher.Nodes;
using StringEnricher.Nodes.MarkdownV2.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to create inline link styles in MarkdownV2 format.
/// Example: "[test](https://example.com)"
/// </summary>
public static class InlineLinkMarkdownV2
{
    /// <summary>
    /// Applies the inline link style to the given link title and link URL using plain text style.
    /// </summary>
    /// <param name="linkTitle">
    /// The link title as a plain text string.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a plain text string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified link title and link URL.
    /// </returns>
    public static InlineLinkNode<PlainTextNode> Apply(string linkTitle, string linkUrl) =>
        InlineLinkNode<PlainTextNode>.Apply(linkTitle, linkUrl);

    /// <summary>
    /// Applies the inline link style to the given link title and link URL using the specified inner style.
    /// </summary>
    /// <param name="linkTitle">
    /// The inner style representing the link title.
    /// </param>
    /// <param name="linkUrl">
    /// The inner style representing the link URL.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified link title and link URL.
    /// </returns>
    public static InlineLinkNode<T> Apply<T>(T linkTitle, string linkUrl) where T : INode =>
        InlineLinkNode<T>.Apply(linkTitle, linkUrl);

    /// <summary>
    /// Applies the inline link style to the given integer link title and URL.
    /// </summary>
    /// <param name="linkTitle">
    /// The integer to be used as link title.
    /// </param>
    /// <param name="linkUrl">
    /// The link URL as a string.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="InlineLinkNode{TInner}"/> struct
    /// with the specified integer link title and link URL.
    /// </returns>
    public static InlineLinkNode<IntegerNode> Apply(int linkTitle, string linkUrl) =>
        InlineLinkNode<IntegerNode>.Apply(linkTitle, linkUrl);
}