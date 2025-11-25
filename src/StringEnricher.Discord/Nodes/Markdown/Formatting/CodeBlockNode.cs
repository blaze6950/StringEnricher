using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;

namespace StringEnricher.Discord.Nodes.Markdown.Formatting;

/// <summary>
/// Represents code block text in Discord markdown format.
/// Example: "```code block```"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with code block syntax.
/// </typeparam>
[DebuggerDisplay("{typeof(CodeBlockNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
public readonly struct CodeBlockNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix for code block in Discord markdown format.
    /// </summary>
    public const string Prefix = "```";

    /// <summary>
    /// The suffix for code block in Discord markdown format.
    /// </summary>
    public const string Suffix = "```";

    private readonly TInner _innerCodeBlock;

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeBlockNode{TInner}"/> struct.
    /// </summary>
    /// <param name="innerCodeBlock">
    /// The inner style to be wrapped with code block syntax.
    /// </param>
    public CodeBlockNode(TInner innerCodeBlock)
    {
        _innerCodeBlock = innerCodeBlock;
    }

    /// <summary>
    /// Returns the string representation of the code block style in Discord markdown format.
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
    /// Gets the length of the inner code block content.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _innerCodeBlock.TotalLength;

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
        Prefix.AsSpan().CopyTo(destination);
        writtenChars += Prefix.Length;

        // Copy inner code block
        writtenChars += _innerCodeBlock.CopyTo(destination[writtenChars..]);

        // Copy suffix
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

    /// <summary>
    /// Applies the code block style to the given inner code block.
    /// </summary>
    /// <param name="innerCodeBlock">
    /// The inner style to be wrapped with code block syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static CodeBlockNode<TInner> Apply(TInner innerCodeBlock) => new(innerCodeBlock);
}