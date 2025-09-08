using StringEnricher.Nodes;
using StringEnricher.Nodes.Html.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Html;

/// <summary>
/// Provides methods to apply inline link styling in HTML format.
/// Example: "&lt;a href="url"&gt;title&lt;/a&gt;"
/// </summary>
public static class InlineLinkHtml
{
    /// <summary>
    /// Applies inline link style to the given styled title and URL.
    /// </summary>
    /// <param name="linkTitle">The styled link title to be wrapped with anchor HTML tags.</param>
    /// <param name="linkUrl">The link URL to be used in the anchor tag.</param>
    /// <typeparam name="T">The type of the inner style that implements <see cref="INode"/>.</typeparam>
    /// <returns>A new instance of <see cref="InlineLinkNode{TInner}"/> wrapping the provided styled title and URL.</returns>
    public static InlineLinkNode<T> Apply<T>(T linkTitle, string linkUrl) where T : INode =>
        InlineLinkNode<T>.Apply(linkTitle, linkUrl);

    #region Overloads for Common Types

    /// <summary>
    /// Applies inline link style to the given title and URL.
    /// </summary>
    /// <param name="linkTitle">The link title to be wrapped with anchor HTML tags.</param>
    /// <param name="linkUrl">The link URL to be used in the anchor tag.</param>
    /// <returns>A new instance of <see cref="InlineLinkNode{TInner}"/> wrapping the provided title and URL.</returns>
    public static InlineLinkNode<PlainTextNode> Apply(string linkTitle, string linkUrl) =>
        InlineLinkNode<PlainTextNode>.Apply(linkTitle, linkUrl);

    /// <summary>
    /// Applies inline link style to the given integer title and URL.
    /// </summary>
    /// <param name="linkTitle">The integer to be used as link title.</param>
    /// <param name="linkUrl">The link URL to be used in the anchor tag.</param>
    /// <returns>A new instance of <see cref="InlineLinkNode{TInner}"/> containing the integer title and URL.</returns>
    public static InlineLinkNode<IntegerNode> Apply(int linkTitle, string linkUrl) =>
        InlineLinkNode<IntegerNode>.Apply(linkTitle, linkUrl);

    #endregion
}