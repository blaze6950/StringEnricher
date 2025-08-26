namespace StringEnricher.StringStyles;

/// <summary>
/// A style that represents plain text without any special formatting.
/// </summary>
public readonly struct PlainTextStyle : IStyle
{
    private readonly string _text;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlainTextStyle"/> struct.
    /// </summary>
    /// <param name="text"></param>
    public PlainTextStyle(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        _text = text;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;
    /// <inheritdoc />
    public int TotalLength => _text.Length;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var textLength = _text.Length;
        if (destination.Length < textLength)
        {
            throw new ArgumentException("Destination span too small.");
        }

        _text.AsSpan().CopyTo(destination);

        return textLength;
    }

    /// <summary>
    /// Implicitly converts a string to a <see cref="PlainTextStyle"/>.
    /// </summary>
    /// <param name="text">Source string</param>
    /// <returns><see cref="PlainTextStyle"/></returns>
    public static implicit operator PlainTextStyle(string text) => new(text);
}