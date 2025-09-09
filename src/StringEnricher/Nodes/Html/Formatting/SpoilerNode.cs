namespace StringEnricher.Nodes.Html.Formatting;

/// <summary>
/// Represents spoiler text in HTML format.
/// Example: "&lt;tg-spoiler&gt;spoiler text&lt;/tg-spoiler&gt;"
/// </summary>
public readonly struct SpoilerNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The opening spoiler tag.
    /// </summary>
    public const string Prefix = "<tg-spoiler>";

    /// <summary>
    /// The closing spoiler tag.
    /// </summary>
    public const string Suffix = "</tg-spoiler>";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpoilerNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style to be wrapped with spoiler HTML tags.</param>
    public SpoilerNode(TInner inner)
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
    /// Gets the total length of the HTML spoiler syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Suffix.Length;

    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted spoiler text to the provided span.
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
        var totalLength = TotalLength;
        if (index < 0 || index >= totalLength)
        {
            character = '\0';
            return false;
        }

        if (index < Prefix.Length)
        {
            character = Prefix[index];
            return true;
        }

        if (index >= Prefix.Length + InnerLength)
        {
            character = Suffix[index - Prefix.Length - InnerLength];
            return true;
        }

        return _innerText.TryGetChar(index - Prefix.Length, out character);
    }

    public static SpoilerNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}