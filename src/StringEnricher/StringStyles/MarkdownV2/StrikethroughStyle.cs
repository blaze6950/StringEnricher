namespace StringEnricher.StringStyles.MarkdownV2;

/// <summary>
/// Provides methods to apply strikethrough style to text in MarkdownV2 format.
/// Example: "~strikethrough text~"
/// </summary>
public static class StrikethroughMarkdownV2
{
    /// <summary>
    /// Applies strikethrough style to the given text using plain text style as inner style.
    /// </summary>
    /// <param name="text">
    /// The text to be styled with strikethrough syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughStyle{PlainTextStyle}"/> containing the specified text.
    /// </returns>
    public static StrikethroughStyle<PlainTextStyle> Apply(string text) =>
        StrikethroughStyle<PlainTextStyle>.Apply(text);

    /// <summary>
    /// Applies strikethrough style to the given inner style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with strikethrough syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="IStyle"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="StrikethroughStyle{T}"/> containing the specified inner style.
    /// </returns>
    public static StrikethroughStyle<T> Apply<T>(T style) where T : IStyle =>
        StrikethroughStyle<T>.Apply(style);
}

/// <summary>
/// Represents strikethrough text in MarkdownV2 format.
/// Example: "~strikethrough text~"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with strikethrough syntax.
/// </typeparam>
public readonly struct StrikethroughStyle<TInner> : IStyle
    where TInner : IStyle
{
    /// <summary>
    /// The prefix and suffix used to apply strikethrough style in MarkdownV2 format.
    /// </summary>
    public const string Prefix = "~";

    /// <summary>
    /// The suffix used to apply strikethrough style in MarkdownV2 format.
    /// </summary>
    public const string Suffix = "~";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="StrikethroughStyle{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style to be wrapped with strikethrough syntax.
    /// </param>
    public StrikethroughStyle(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the string representation of the strikethrough style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text.
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

    /// <summary>
    /// Applies strikethrough style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with strikethrough syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="StrikethroughStyle{TInner}"/> containing the specified inner style.
    /// </returns>
    public static StrikethroughStyle<TInner> Apply(TInner innerStyle) => new(innerStyle);
}