using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.Html.Formatting;

/// <summary>
/// Represents code block text in HTML format.
/// Example: "&lt;pre&gt;code block&lt;/pre&gt;"
/// </summary>
[DebuggerDisplay("{typeof(CodeBlockNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
public readonly struct CodeBlockNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The opening code block tag.
    /// </summary>
    public const string Prefix = "<pre>";

    /// <summary>
    /// The closing code block tag.
    /// </summary>
    public const string Suffix = "</pre>";

    private readonly TInner _innerCodeBlock;

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeBlockNode{TInner}"/> struct.
    /// </summary>
    /// <param name="innerCodeBlock">The inner style to be wrapped with code block HTML tags.</param>
    public CodeBlockNode(TInner innerCodeBlock)
    {
        _innerCodeBlock = innerCodeBlock;
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
        if (!Prefix.AsSpan().TryCopyTo(destination))
        {
            return false;
        }

        charsWritten += Prefix.Length;

        // Copy inner text
        var isInnerTextFormatSuccess = _innerCodeBlock.TryFormat(
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
        if (!Suffix.AsSpan().TryCopyTo(destination.SliceSafe(charsWritten)))
        {
            return false;
        }

        charsWritten += Suffix.Length;

        return true;
    }

    /// <summary>
    /// Gets the length of the inner code block.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _innerCodeBlock.TotalLength;

    /// <summary>
    /// Gets the total length of the HTML code block syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Suffix.Length;

    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted code block text to the provided span.
    /// </summary>
    /// <param name="destination">The span to copy the formatted text into.</param>
    /// <returns>The total length of the formatted text.</returns>
    /// <exception cref="ArgumentException">Thrown if the destination span is too small.</exception>
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;
        Prefix.AsSpan().CopyTo(destination);
        writtenChars += Prefix.Length;

        writtenChars += _innerCodeBlock.CopyTo(destination[writtenChars..]);

        Suffix.AsSpan().CopyTo(destination[writtenChars..]);
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
            return _innerCodeBlock.TryGetChar(index, out character);
        }

        index -= InnerLength;

        character = Suffix[index];
        return true;
    }

    public static CodeBlockNode<TInner> Apply(TInner innerCodeBlock) => new(innerCodeBlock);
}