namespace StringEnricher.StringStyles.MarkdownV2;

/// <summary>
/// Provides methods to create specific code block styles in MarkdownV2 format.
/// Example: "```language\ncode block\n```"
/// </summary>
public static class SpecificCodeBlockMarkdownV2
{
    /// <summary>
    /// Applies specific code block style to the given code block and language using plain text style.
    /// </summary>
    /// <param name="codeBlock">
    /// The code block content.
    /// </param>
    /// <param name="language">
    /// The programming language of the code block.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockStyle{PlainTextStyle}"/> struct.
    /// </returns>
    public static SpecificCodeBlockStyle<PlainTextStyle> Apply(string codeBlock, string language) =>
        SpecificCodeBlockStyle<PlainTextStyle>.Apply(codeBlock, language);

    /// <summary>
    /// Applies specific code block style to the given code block and language using the specified style type.
    /// </summary>
    /// <param name="codeBlock">
    /// The inner style representing the code block content.
    /// </param>
    /// <param name="language">
    /// The inner style representing the programming language of the code block.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that will be wrapped with specific code block syntax.
    /// </typeparam>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockStyle{T}"/> struct.
    /// </returns>
    public static SpecificCodeBlockStyle<T> Apply<T>(T codeBlock, T language) where T : IStyle =>
        SpecificCodeBlockStyle<T>.Apply(codeBlock, language);
}

/// <summary>
/// Represents specific code block text in MarkdownV2 format.
/// Example: "```language\ncode block\n```"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with specific code block syntax.
/// </typeparam>
public readonly struct SpecificCodeBlockStyle<TInner> : IStyle
    where TInner : IStyle
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
    private readonly TInner _language;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificCodeBlockStyle{TInner}"/> struct.
    /// </summary>
    /// <param name="codeBlock">
    /// The inner style representing the code block content.
    /// </param>
    /// <param name="language">
    /// The inner style representing the programming language of the code block.
    /// </param>
    public SpecificCodeBlockStyle(TInner codeBlock, TInner language)
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
    public int InnerLength => _innerCodeBlock.TotalLength + _language.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length + Separator.Length + Suffix.Length;

    /// <inheritdoc />
    public int TotalLength => SyntaxLength + InnerLength;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var totalLength = TotalLength;
        if (destination.Length < totalLength)
        {
            throw new ArgumentException("The destination span is too small to hold the formatted text.");
        }

        var pos = 0;

        // Copy prefix
        Prefix.AsSpan().CopyTo(destination.Slice(pos, Prefix.Length));
        pos += Prefix.Length;

        // Copy language
        _language.CopyTo(destination.Slice(pos, _language.TotalLength));
        pos += _language.TotalLength;

        // Copy separator
        Separator.AsSpan().CopyTo(destination.Slice(pos, Separator.Length));
        pos += Separator.Length;

        // Copy code block
        _innerCodeBlock.CopyTo(destination.Slice(pos, _innerCodeBlock.TotalLength));
        pos += _innerCodeBlock.TotalLength;

        // Copy suffix
        Suffix.AsSpan().CopyTo(destination.Slice(pos, Suffix.Length));

        return totalLength;
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

        if (index < _language.TotalLength)
        {
            return _language.TryGetChar(index, out character);
        }

        index -= _language.TotalLength;

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

        // At this point, index must be within the suffix
        character = Suffix[index];
        return true;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SpecificCodeBlockStyle{TInner}"/> struct.
    /// </summary>
    /// <param name="codeBlock">
    /// The inner style representing the code block content.
    /// </param>
    /// <param name="language">
    /// The inner style representing the programming language of the code block.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="SpecificCodeBlockStyle{TInner}"/> struct.
    /// </returns>
    public static SpecificCodeBlockStyle<TInner> Apply(TInner codeBlock, TInner language) => new(codeBlock, language);
}