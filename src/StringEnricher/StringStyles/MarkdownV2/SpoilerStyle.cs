namespace StringEnricher.StringStyles.MarkdownV2;

/// <summary>
/// Provides methods to apply spoiler styling in MarkdownV2 format.
/// Example: "||spoiler text||"
/// </summary>
public static class SpoilerMarkdownV2
{
    /// <summary>
    /// Applies spoiler styling to the given text using plain text style as the inner style.
    /// </summary>
    /// <param name="text">
    /// The text to be styled as a spoiler.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpoilerStyle{PlainTextStyle}"/> containing the styled text.
    /// </returns>
    public static SpoilerStyle<PlainTextStyle> Apply(string text) =>
        SpoilerStyle<PlainTextStyle>.Apply(text);

    /// <summary>
    /// Applies spoiler styling to the given style.
    /// </summary>
    /// <param name="style">
    /// The style to be wrapped with spoiler syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="IStyle"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="SpoilerStyle{T}"/> containing the styled text.
    /// </returns>
    public static SpoilerStyle<T> Apply<T>(T style) where T : IStyle =>
        SpoilerStyle<T>.Apply(style);
}

/// <summary>
/// Represents spoiler text in MarkdownV2 format.
/// Example: "||spoiler text||"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with spoiler syntax.
/// </typeparam>
public readonly struct SpoilerStyle<TInner> : IStyle
    where TInner : IStyle
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
    /// Initializes a new instance of the <see cref="SpoilerStyle{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style that will be wrapped with spoiler syntax.
    /// </param>
    public SpoilerStyle(TInner inner)
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

    /// <summary>
    /// Gets the length of the inner text excluding the spoiler syntax.
    /// </summary>
    public int InnerLength => _innerText.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length + Suffix.Length;

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

        // Copy inner text
        _innerText.CopyTo(destination.Slice(pos, InnerLength));
        pos += InnerLength;

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

        if (index < Prefix.Length + InnerLength)
        {
            return _innerText.TryGetChar(index - Prefix.Length, out character);
        }

        // Suffix part
        character = Suffix[index - Prefix.Length - InnerLength];
        return true;
    }

    /// <summary>
    /// Creates a new instance of <see cref="SpoilerStyle{TInner}"/> with the specified inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with spoiler syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="SpoilerStyle{TInner}"/> containing the specified inner style.
    /// </returns>
    public static SpoilerStyle<TInner> Apply(TInner innerStyle) => new(innerStyle);
}