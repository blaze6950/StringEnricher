using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.Html.Formatting;

/// <summary>
/// Represents a specific code block in HTML format with language class.
/// Example: "&lt;pre&gt;&lt;code class="language-csharp"&gt;code block&lt;/code&gt;&lt;/pre&gt;"
/// </summary>
[DebuggerDisplay(
    "{typeof(SpecificCodeBlockNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
public readonly struct SpecificCodeBlockNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The opening code block tag and language class.
    /// </summary>
    public const string Prefix = "<pre><code class=\"language-";

    /// <summary>
    /// The separator between the language and the code block.
    /// </summary>
    public const string Separator = "\">";

    /// <summary>
    /// The closing code block and pre tags.
    /// </summary>
    public const string Suffix = "</code></pre>";

    private readonly TInner _innerCodeBlock;
    private readonly string _language;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </summary>
    /// <param name="codeBlock">The styled code block text.</param>
    /// <param name="language">The styled language to be used in the class attribute.</param>
    public SpecificCodeBlockNode(TInner codeBlock, string language)
    {
        _innerCodeBlock = codeBlock;
        _language = language;
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
        if (!Prefix.AsSpan().TryCopyTo(destination.SliceSafe(charsWritten, Prefix.Length)))
        {
            return false;
        }

        charsWritten += Prefix.Length;

        // Copy language
        if (!_language.AsSpan().TryCopyTo(destination.SliceSafe(charsWritten, _language.Length)))
        {
            return false;
        }

        charsWritten += _language.Length;

        // Copy separator
        if (!Separator.AsSpan().TryCopyTo(destination.SliceSafe(charsWritten, Separator.Length)))
        {
            return false;
        }

        charsWritten += Separator.Length;

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
        if (!Suffix.AsSpan().TryCopyTo(destination.SliceSafe(charsWritten, Suffix.Length)))
        {
            return false;
        }

        charsWritten += Suffix.Length;

        return true;
    }

    /// <summary>
    /// Gets the length of the inner code block and language.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _innerCodeBlock.TotalLength + _language.Length;

    /// <summary>
    /// Gets the total length of the HTML code block syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Separator.Length + Suffix.Length;

    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted specific code block text to the provided span.
    /// </summary>
    /// <param name="destination">The span to copy the formatted text into.</param>
    /// <returns>The total length of the formatted text.</returns>
    /// <exception cref="ArgumentException">Thrown if the destination span is too small.</exception>
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;
        Prefix.AsSpan().CopyTo(destination.Slice(writtenChars, Prefix.Length));
        writtenChars += Prefix.Length;

        _language.CopyTo(destination.Slice(writtenChars, _language.Length));
        writtenChars += _language.Length;

        Separator.AsSpan().CopyTo(destination.Slice(writtenChars, Separator.Length));
        writtenChars += Separator.Length;

        writtenChars += _innerCodeBlock.CopyTo(destination[writtenChars..]);

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

        if (index < _language.Length)
        {
            character = _language[index];
            return true;
        }

        index -= _language.Length;

        if (index < Separator.Length)
        {
            character = Separator[index];
            return true;
        }

        index -= Separator.Length;

        if (index < _innerCodeBlock.TotalLength)
        {
            return _innerCodeBlock.TryGetChar(index, out character);
        }

        index -= _innerCodeBlock.TotalLength;

        // Remaining part is in the Suffix
        character = Suffix[index];
        return true;
    }

    /// <summary>
    /// Applies specific code block style to the given code block and language.
    /// </summary>
    /// <param name="codeBlock">The code block text.</param>
    /// <param name="language">The language to be used in the class attribute.</param>
    /// <returns>A new instance of <see cref="SpecificCodeBlockNode{TInner}"/> wrapping the provided code block and language.</returns>
    public static SpecificCodeBlockNode<TInner> Apply(TInner codeBlock, string language) => new(codeBlock, language);
}