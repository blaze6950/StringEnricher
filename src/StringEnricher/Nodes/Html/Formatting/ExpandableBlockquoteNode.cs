using StringEnricher.Nodes.Shared;

namespace StringEnricher.Nodes.Html.Formatting;

/// <summary>
/// Provides methods to apply expandable blockquote styling in HTML format.
/// Example: "&lt;blockquote expandable&gt;quoted text&lt;/blockquote&gt;"
/// </summary>
public static class ExpandableBlockquoteHtml
{
    /// <summary>
    /// Applies expandable blockquote style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with expandable blockquote HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ExpandableBlockquoteNode{TInner}"/> wrapping the provided text.
    /// </returns>
    public static ExpandableBlockquoteNode<PlainTextNode> Apply(string text) =>
        ExpandableBlockquoteNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies expandable blockquote style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with expandable blockquote HTML tags.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="ExpandableBlockquoteNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static ExpandableBlockquoteNode<T> Apply<T>(T style) where T : INode =>
        ExpandableBlockquoteNode<T>.Apply(style);

    /// <summary>
    /// Applies expandable blockquote style to the given integer.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as expandable blockquote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ExpandableBlockquoteNode{TInner}"/> containing the styled integer.
    /// </returns>
    public static ExpandableBlockquoteNode<IntegerNode> Apply(int integer) =>
        ExpandableBlockquoteNode<IntegerNode>.Apply(integer);
}

/// <summary>
/// Represents expandable blockquote text in HTML format.
/// Example: "&lt;blockquote expandable&gt;quoted text&lt;/blockquote&gt;"
/// </summary>
public readonly struct ExpandableBlockquoteNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The opening expandable blockquote tag.
    /// </summary>
    public const string Prefix = "<blockquote expandable>";
    /// <summary>
    /// The closing expandable blockquote tag.
    /// </summary>
    public const string Suffix = "</blockquote>";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandableBlockquoteNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style to be wrapped with expandable blockquote HTML tags.</param>
    public ExpandableBlockquoteNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <inheritdoc/>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text.
    /// </summary>
    public int InnerLength => _innerText.TotalLength;
    /// <summary>
    /// Gets the total length of the HTML expandable blockquote syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Suffix.Length;
    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted expandable blockquote text to the provided span.
    /// </summary>
    /// <param name="destination">The span to copy the formatted text into.</param>
    /// <returns>The total length of the formatted text.</returns>
    /// <exception cref="ArgumentException">Thrown if the destination span is too small.</exception>
    public int CopyTo(Span<char> destination)
    {
        var totalLength = TotalLength;
        if (destination.Length < totalLength)
        {
            throw new ArgumentException("The destination span is too small to hold the formatted text.");
        }

        var pos = 0;
        Prefix.AsSpan().CopyTo(destination.Slice(pos, Prefix.Length));
        pos += Prefix.Length;

        _innerText.CopyTo(destination.Slice(pos, InnerLength));
        pos += InnerLength;

        Suffix.AsSpan().CopyTo(destination.Slice(pos, Suffix.Length));

        return totalLength;
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        if (index < Prefix.Length)
        {
            character = Prefix[index];
            return true;
        }

        index -= Prefix.Length;

        if (index < InnerLength)
        {
            return _innerText.TryGetChar(index, out character);
        }

        index -= InnerLength;

        character = Suffix[index];
        return true;
    }

    public static ExpandableBlockquoteNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}