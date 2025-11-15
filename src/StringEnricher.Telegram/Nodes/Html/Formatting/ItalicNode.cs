using System.Diagnostics;
using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.Html.Formatting;

/// <summary>
/// Represents italic text in HTML format.
/// Example: "&lt;i&gt;italic text&lt;/i&gt;"
/// </summary>
[DebuggerDisplay("{typeof(ItalicNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
public readonly struct ItalicNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The opening italic tag.
    /// </summary>
    public const string Prefix = "<i>";

    /// <summary>
    /// The closing italic tag.
    /// </summary>
    public const string Suffix = "</i>";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItalicNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style to be wrapped with italic HTML tags.</param>
    public ItalicNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <inheritdoc/>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _innerText.TotalLength;

    /// <summary>
    /// Gets the total length of the HTML italic syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Suffix.Length;

    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted italic text to the provided span.
    /// </summary>
    /// <param name="destination">The span to copy the formatted text into.</param>
    /// <returns>The total length of the formatted text.</returns>
    /// <exception cref="ArgumentException">Thrown if the destination span is too small.</exception>
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;
        Prefix.AsSpan().CopyTo(destination.Slice(writtenChars, Prefix.Length));
        writtenChars += Prefix.Length;

        writtenChars += _innerText.CopyTo(destination[writtenChars..]);

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

        if (index < Prefix.Length + InnerLength)
        {
            return _innerText.TryGetChar(index - Prefix.Length, out character);
        }

        character = Suffix[index - Prefix.Length - InnerLength];
        return true;
    }

    /// <summary>
    /// Applies italic style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with italic HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static ItalicNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}