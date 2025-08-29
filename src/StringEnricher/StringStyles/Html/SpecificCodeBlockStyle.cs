namespace StringEnricher.StringStyles.Html;

/// <summary>
/// Provides methods to apply specific code block styling in HTML format with language class.
/// Example: "<pre><code class=\"language-csharp\">code block</code></pre>"
/// </summary>
public static class SpecificCodeBlockHtml
{
    /// <summary>
    /// Applies specific code block style to the given text and language.
    /// </summary>
    /// <param name="codeBlock">The code block text to be wrapped with code block HTML tags.</param>
    /// <param name="language">The language to be used in the class attribute.</param>
    /// <returns>A new instance of <see cref="SpecificCodeBlockStyle{PlainTextStyle}"/> wrapping the provided code block and language.</returns>
    public static SpecificCodeBlockStyle<PlainTextStyle> Apply(string codeBlock, string language) =>
        SpecificCodeBlockStyle<PlainTextStyle>.Apply(codeBlock, language);

    /// <summary>
    /// Applies specific code block style to the given styled code block and language.
    /// </summary>
    /// <param name="codeBlock">The styled code block text.</param>
    /// <param name="language">The styled language to be used in the class attribute.</param>
    /// <typeparam name="T">The type of the inner style that implements <see cref="IStyle"/>.</typeparam>
    /// <returns>A new instance of <see cref="SpecificCodeBlockStyle{T}"/> wrapping the provided styled code block and language.</returns>
    public static SpecificCodeBlockStyle<T> Apply<T>(T codeBlock, T language) where T : IStyle =>
        SpecificCodeBlockStyle<T>.Apply(codeBlock, language);
}

/// <summary>
/// Represents a specific code block in HTML format with language class.
/// Example: "<pre><code class=\"language-csharp\">code block</code></pre>"
/// </summary>
public readonly struct SpecificCodeBlockStyle<TInner> : IStyle
    where TInner : IStyle
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
    private readonly TInner _language;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificCodeBlockStyle{TInner}"/> struct.
    /// </summary>
    /// <param name="codeBlock">The styled code block text.</param>
    /// <param name="language">The styled language to be used in the class attribute.</param>
    public SpecificCodeBlockStyle(TInner codeBlock, TInner language)
    {
        _innerCodeBlock = codeBlock;
        _language = language;
    }

    /// <inheritdoc/>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner code block and language.
    /// </summary>
    public int InnerLength => _innerCodeBlock.TotalLength + _language.TotalLength;
    /// <summary>
    /// Gets the total length of the HTML code block syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Separator.Length + Suffix.Length;
    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted specific code block text to the provided span.
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

        _language.CopyTo(destination.Slice(pos, _language.TotalLength));
        pos += _language.TotalLength;

        Separator.AsSpan().CopyTo(destination.Slice(pos, Separator.Length));
        pos += Separator.Length;

        _innerCodeBlock.CopyTo(destination.Slice(pos, _innerCodeBlock.TotalLength));
        pos += _innerCodeBlock.TotalLength;

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

        // Remaining part is in the Suffix
        character = Suffix[index];
        return true;
    }

    /// <summary>
    /// Applies specific code block style to the given code block and language.
    /// </summary>
    /// <param name="codeBlock">The code block text.</param>
    /// <param name="language">The language to be used in the class attribute.</param>
    /// <returns>A new instance of <see cref="SpecificCodeBlockStyle{TInner}"/> wrapping the provided code block and language.</returns>
    public static SpecificCodeBlockStyle<TInner> Apply(TInner codeBlock, TInner language) => new(codeBlock, language);
}