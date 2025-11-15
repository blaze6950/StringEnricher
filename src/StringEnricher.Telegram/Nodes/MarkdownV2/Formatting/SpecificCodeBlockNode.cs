using System.Diagnostics;
using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.MarkdownV2.Formatting;

/// <summary>
/// Represents specific code block text in MarkdownV2 format.
/// Example: "```language\ncode block\n```"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with specific code block syntax.
/// </typeparam>
[DebuggerDisplay("{typeof(SpecificCodeBlockNode).Name,nq} Prefix={Prefix} Language={_language} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
public readonly struct SpecificCodeBlockNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix for specific code block style in MarkdownV2 format.
    /// </summary>
    public const string Prefix = "```";

    /// <summary>
    /// The separator between language and code block in MarkdownV2 format.
    /// </summary>
    public const string Separator = "\n";

    /// <summary>
    /// The suffix for specific code block style in MarkdownV2 format.
    /// </summary>
    public const string Suffix = "\n```";

    private readonly TInner _innerCodeBlock;
    private readonly string _language;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </summary>
    /// <param name="codeBlock">
    /// The inner style representing the code block content.
    /// </param>
    /// <param name="language">
    /// The inner style representing the programming language of the code block.
    /// </param>
    public SpecificCodeBlockNode(TInner codeBlock, string language)
    {
        _innerCodeBlock = codeBlock;
        _language = language;
    }

    /// <summary>
    /// Returns the string representation of the specific code block style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner content (language + code block).
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _innerCodeBlock.TotalLength + _language.Length;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length + Separator.Length + Suffix.Length;

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

        // Copy language
        _language.CopyTo(destination.Slice(writtenChars, _language.Length));
        writtenChars += _language.Length;

        // Copy separator
        Separator.AsSpan().CopyTo(destination.Slice(writtenChars, Separator.Length));
        writtenChars += Separator.Length;

        // Copy code block
        writtenChars += _innerCodeBlock.CopyTo(destination[writtenChars..]);

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

        // At this point, index must be within the Suffix
        character = Suffix[index];
        return true;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </summary>
    /// <param name="codeBlock">
    /// The inner style representing the code block content.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="SpecificCodeBlockNode{TInner}"/> struct.
    /// </returns>
    public static SpecificCodeBlockNode<TInner> Apply(TInner codeBlock, string language) =>
        new(codeBlock, language);
}