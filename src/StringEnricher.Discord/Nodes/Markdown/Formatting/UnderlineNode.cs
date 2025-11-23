using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;

namespace StringEnricher.Discord.Nodes.Markdown.Formatting;

/// <summary>
/// Represents underline text in Discord markdown format.
/// Example: "__underline text__"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with underline syntax.
/// </typeparam>
[DebuggerDisplay("{typeof(UnderlineNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
public readonly struct UnderlineNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix and suffix used for underline style in Discord markdown format.
    /// </summary>
    public const string Prefix = "__";

    /// <summary>
    /// The prefix and suffix used for underline style in Discord markdown format.
    /// </summary>
    public const string Suffix = "__";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnderlineNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style that will be wrapped with underline syntax.
    /// </param>
    public UnderlineNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the string representation of the underline style in Discord markdown format.
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
        if (!Prefix.AsSpan().TryCopyTo(destination.SliceSafe(charsWritten, Prefix.Length)))
        {
            return false;
        }

        charsWritten += Prefix.Length;

        // Copy inner text
        var isInnerTextFormatSuccess = _innerText.TryFormat(
            destination.SliceSafe(charsWritten),
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
        if (!Suffix.AsSpan().TryCopyTo(destination.SliceSafe(charsWritten, Suffix.Length)))
        {
            return false;
        }

        charsWritten += Suffix.Length;

        return true;
    }

    /// <summary>
    /// Gets the length of the inner text excluding the underline syntax.
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
        ;

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

        var innerIndex = index - Prefix.Length;
        if (innerIndex < InnerLength)
        {
            return _innerText.TryGetChar(innerIndex, out character);
        }

        var suffixIndex = innerIndex - InnerLength;
        character = Suffix[suffixIndex];
        return true;
    }

    /// <summary>
    /// Applies the underline style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with underline syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="UnderlineNode{TInner}"/> that wraps the provided inner style.
    /// </returns>
    public static UnderlineNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}