using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.Html.Formatting;

/// <summary>
/// Represents strikethrough text in HTML format.
/// Example: "&lt;s&gt;strikethrough text&lt;/s&gt;"
/// </summary>
public readonly struct StrikethroughNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The opening strikethrough tag.
    /// </summary>
    public const string Prefix = "<s>";

    /// <summary>
    /// The closing strikethrough tag.
    /// </summary>
    public const string Suffix = "</s>";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="StrikethroughNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style to be wrapped with strikethrough HTML tags.</param>
    public StrikethroughNode(TInner inner)
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
    /// Gets the total length of the HTML strikethrough syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Suffix.Length;

    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted strikethrough text to the provided span.
    /// </summary>
    /// <param name="destination">The span to copy the formatted text into.</param>
    /// <returns>The total length of the formatted text.</returns>
    /// <exception cref="ArgumentException">Thrown if the destination span is too small.</exception>
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;
        Prefix.AsSpan().CopyTo(destination.Slice(writtenChars, Prefix.Length));
        writtenChars += Prefix.Length;

        writtenChars += _innerText.CopyTo(destination[writtenChars..]);;

        Suffix.AsSpan().CopyTo(destination.Slice(writtenChars, Suffix.Length));
        writtenChars += Suffix.Length;

        return writtenChars;
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

    /// <summary>
    /// Applies strikethrough style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with strikethrough HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static StrikethroughNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}