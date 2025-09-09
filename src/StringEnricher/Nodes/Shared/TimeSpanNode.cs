namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a timeSpan.
/// </summary>
public readonly struct TimeSpanNode : INode
{
    private readonly TimeSpan _timeSpan;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeSpanNode"/> struct.
    /// </summary>
    /// <param name="timeSpan">
    /// The timeSpan value.
    /// </param>
    /// <param name="format">
    /// The format to use. If null, the default format is used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use. If null, the current culture is used.
    /// </param>
    public TimeSpanNode(TimeSpan timeSpan, string? format = null, IFormatProvider? provider = null)
    {
        _timeSpan = timeSpan;
        _format = format;
        _provider = provider;
        TotalLength = GetTimeSpanLength(_timeSpan, _format, _provider);
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

        _timeSpan.TryFormat(destination, out _, _format, _provider);

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
        _timeSpan.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a timeSpan to a <see cref="TimeSpanNode"/>.
    /// </summary>
    /// <param name="timeSpan">Source timeSpan</param>
    /// <returns><see cref="TimeSpanNode"/></returns>
    public static implicit operator TimeSpanNode(TimeSpan timeSpan) => new(timeSpan);

    /// <summary>
    /// Gets the length of the string representation of a timeSpan.
    /// </summary>
    /// <param name="value">
    /// The timeSpan value.
    /// </param>
    /// <param name="format">
    /// The format to use. If null, the default format is used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use. If null, the current culture is used.
    /// </param>
    /// <returns>
    /// The length of the string representation of the timeSpan.
    /// </returns>
    private static int GetTimeSpanLength(TimeSpan value, string? format = null, IFormatProvider? provider = null)
    {
        var bufferSize = 32;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var dateOnlyLength))
            {
                return dateOnlyLength;
            }

            bufferSize *= 2;
            if (bufferSize > 128)
            {
                throw new InvalidOperationException("TimeSpan format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a TimeSpan.
    /// </summary>
    /// <param name="value">
    /// The TimeSpan value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the TimeSpan.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the TimeSpan.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(TimeSpan value, string? format, IFormatProvider? provider,
        int bufferSize, out int length)
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