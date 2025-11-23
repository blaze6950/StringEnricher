using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.Html.Formatting;

/// <summary>
/// Represents underlined text in HTML format.
/// Example: "&lt;u&gt;underlined text&lt;/u&gt;"
/// </summary>
[DebuggerDisplay("{typeof(UnderlineNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
public readonly struct UnderlineNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The opening underline tag.
    /// </summary>
    public const string Prefix = "<u>";

    /// <summary>
    /// The closing underline tag.
    /// </summary>
    public const string Suffix = "</u>";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnderlineNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style to be wrapped with underline HTML tags.</param>
    public UnderlineNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <inheritdoc/>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? provider)
    {
        var length = this.GetSpanFormattableLength(
            nodeSettings: StringEnricherSettings.Extensions.StringBuilder,
            format: format,
            provider: provider
        );

        return string.Create(
            length: length,
            state: ValueTuple.Create(this, format, provider),
            action: static (span, state) =>
            {
                if (!state.Item1.TryFormat(span, out _, state.Item2, state.Item3))
                {
                    throw new InvalidOperationException("Formatting failed unexpectedly.");
                }
            }
        );
    }

    /// <inheritdoc />
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null
    )
    {
        charsWritten = 0;

        // Copy prefix
        if (!Prefix.AsSpan().TryCopyTo(destination.Slice(charsWritten, Prefix.Length)))
        {
            return false;
        }

        charsWritten += Prefix.Length;

        // Copy inner text
        var isInnerTextFormatSuccess = _innerText.TryFormat(
            destination[charsWritten..],
            out var innerCharsWritten,
            format,
            provider
        );

        if (!isInnerTextFormatSuccess)
        {
            return false;
        }

        charsWritten += innerCharsWritten;

        // Copy suffix
        if (!Suffix.AsSpan().TryCopyTo(destination.Slice(charsWritten, Suffix.Length)))
        {
            return false;
        }

        charsWritten += Suffix.Length;

        return true;
    }

    /// <summary>
    /// Gets the length of the inner text.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _innerText.TotalLength;

    /// <summary>
    /// Gets the total length of the HTML underline syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Suffix.Length;

    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted underlined text to the provided span.
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
    /// Applies underline style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with underline HTML tags.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static UnderlineNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}