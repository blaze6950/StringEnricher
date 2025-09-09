using StringEnricher.Nodes;
using StringEnricher.Nodes.Html;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Html;

/// <summary>
/// Escapes HTML reserved characters for the given text.
/// Example: "text with &lt;tags&gt; &amp; entities" => "text with &amp;lt;tags&amp;gt; &amp;amp; entities"
/// </summary>
public static class EscapeHtml
{
    /// <summary>
    /// Escapes HTML reserved characters for the given text.
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
    /// Escapes HTML reserved characters for the given text.
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