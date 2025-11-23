using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.MarkdownV2.Formatting;

/// <summary>
/// Represents spoiler text in MarkdownV2 format.
/// Example: "||spoiler text||"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with spoiler syntax.
/// </typeparam>
[DebuggerDisplay("{typeof(SpoilerNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
public readonly struct SpoilerNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix used to denote the start of spoiler text in MarkdownV2.
    /// </summary>
    public const string Prefix = "||";

    /// <summary>
    /// The suffix used to denote the end of spoiler text in MarkdownV2.
    /// </summary>
    public const string Suffix = "||";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpoilerNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style that will be wrapped with spoiler syntax.
    /// </param>
    public SpoilerNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the string representation of the spoiler style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
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
    /// Gets the length of the inner text excluding the spoiler syntax.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _innerText.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length + Suffix.Length;

    /// <inheritdoc />
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => SyntaxLength + InnerLength;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;

        // Copy prefix
        Prefix.AsSpan().CopyTo(destination.Slice(writtenChars, Prefix.Length));
        writtenChars += Prefix.Length;

        // Copy inner text
        writtenChars += _innerText.CopyTo(destination[writtenChars..]);

        // Copy suffix
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

        // Suffix part
        character = Suffix[index - Prefix.Length - InnerLength];
        return true;
    }

    /// <summary>
    /// Creates a new instance of <see cref="SpoilerNode{TInner}"/> with the specified inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with spoiler syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpoilerNode{TInner}"/> containing the specified inner style.
    /// </returns>
    public static SpoilerNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}