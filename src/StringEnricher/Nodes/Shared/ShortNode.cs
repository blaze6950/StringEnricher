namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a short.
/// </summary>
public readonly struct ShortNode : INode
{
    private readonly short _short;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShortNode"/> struct.
    /// </summary>
    /// <param name="short">
    /// The short value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string that defines the format of the short.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    public ShortNode(short @short, string? format = null, IFormatProvider? provider = null)
    {
        _short = @short;
        _format = format;
        _provider = provider;
        TotalLength = GetFloatLength(@short, _format, _provider);
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    public int TotalLength { get; }

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var textLength = TotalLength;
        if (destination.Length < textLength)
        {
            throw new ArgumentException("Destination span too small.");
        }

        _short.TryFormat(destination, out _, _format, _provider);

        return textLength;
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        Span<char> buffer = stackalloc char[TotalLength];
        _short.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a short to a <see cref="ShortNode"/>.
    /// </summary>
    /// <param name="short">Source short</param>
    /// <returns><see cref="ShortNode"/></returns>
    public static implicit operator ShortNode(short @short) => new(@short);

    /// <summary>
    /// Calculates the length of the short when represented as a string.
    /// </summary>
    /// <param name="value">
    /// The short value to be measured.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string that defines the format of the short.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// The length of the short when formatted as a string.
    /// </returns>
    private static int GetFloatLength(short value, string? format = null, IFormatProvider? provider = null)
    {
        var bufferSize = 8;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var shortLength))
            {
                return shortLength;
            }

            bufferSize *= 2;
            if (bufferSize > 64)
            {
                throw new InvalidOperationException("short format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a short.
    /// </summary>
    /// <param name="value">
    /// The short value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the short to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the short to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the short.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the short.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(short value, string? format, IFormatProvider? provider, int bufferSize,
        out int length)
    {
        length = 0;
        Span<char> buffer = stackalloc char[bufferSize];

        if (!value.TryFormat(buffer, out var charsWritten, format, provider))
        {
            return false;
        }

        length = charsWritten;
        return true;
    }
}