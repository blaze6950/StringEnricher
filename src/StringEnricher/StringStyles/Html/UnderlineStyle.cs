namespace StringEnricher.StringStyles.Html;

/// <summary>
/// Provides methods to apply underline styling in HTML format.
/// Example: "<u>underlined text</u>"
/// </summary>
public static class UnderlineHtml
{
    /// <summary>
    /// Applies underline style to the given text.
    /// </summary>
    /// <param name="text">The text to be wrapped with underline HTML tags.</param>
    /// <returns>A new instance of <see cref="UnderlineStyle{PlainTextStyle}"/> wrapping the provided text.</returns>
    public static UnderlineStyle<PlainTextStyle> Apply(string text) =>
        UnderlineStyle<PlainTextStyle>.Apply(text);

    /// <summary>
    /// Applies underline style to the given style.
    /// </summary>
    /// <param name="style">The inner style to be wrapped with underline HTML tags.</param>
    /// <typeparam name="T">The type of the inner style that implements <see cref="IStyle"/>.</typeparam>
    /// <returns>A new instance of <see cref="UnderlineStyle{T}"/> wrapping the provided inner style.</returns>
    public static UnderlineStyle<T> Apply<T>(T style) where T : IStyle =>
        UnderlineStyle<T>.Apply(style);
}

/// <summary>
/// Represents underlined text in HTML format.
/// Example: "<u>underlined text</u>"
/// </summary>
public readonly struct UnderlineStyle<TInner> : IStyle
    where TInner : IStyle
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
    /// Initializes a new instance of the <see cref="UnderlineStyle{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style to be wrapped with underline HTML tags.</param>
    public UnderlineStyle(TInner inner)
    {
        _innerText = inner;
    }

    /// <inheritdoc/>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text.
    /// </summary>
    public int InnerLength => _innerText.TotalLength;
    /// <summary>
    /// Gets the total length of the HTML underline syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Suffix.Length;
    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted underlined text to the provided span.
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

        _innerText.CopyTo(destination.Slice(pos, InnerLength));
        pos += InnerLength;

        Suffix.AsSpan().CopyTo(destination.Slice(pos, Suffix.Length));

        return totalLength;
    }

    public static UnderlineStyle<TInner> Apply(TInner innerStyle) => new(innerStyle);
}