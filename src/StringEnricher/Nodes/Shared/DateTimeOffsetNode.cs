namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a dateTimeOffset.
/// </summary>
public readonly struct DateTimeOffsetNode : INode
{
    private readonly DateTimeOffset _dateTimeOffset;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeOffsetNode"/> struct.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The dateTimeOffset value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateTimeOffset to a string.
    /// </param>
    public DateTimeOffsetNode(DateTimeOffset dateTimeOffset, string? format = null, IFormatProvider? provider = null)
    {
        _dateTimeOffset = dateTimeOffset;
        _format = format;
        _provider = provider;
        TotalLength = GetDateTimeOffsetLength(_dateTimeOffset, _format, _provider);
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

        _dateTimeOffset.TryFormat(destination, out _, _format, _provider);

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
        _dateTimeOffset.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a dateTimeOffset to a <see cref="DateTimeOffsetNode"/>.
    /// </summary>
    /// <param name="dateTimeOffset">Source dateTimeOffset</param>
    /// <returns><see cref="DateTimeOffsetNode"/></returns>
    public static implicit operator DateTimeOffsetNode(DateTimeOffset dateTimeOffset) => new(dateTimeOffset);

    /// <summary>
    /// Gets the length of the string representation of a dateTimeOffset.
    /// </summary>
    /// <param name="value">
    /// The dateTimeOffset value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// The length of the string representation of the dateTimeOffset.
    /// </returns>
    private static int GetDateTimeOffsetLength(DateTimeOffset value, string? format = null,
        IFormatProvider? provider = null)
    {
        var bufferSize = 32;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var dateOnlyLength))
            {
                return dateOnlyLength;
            }

            bufferSize *= 2;
            if (bufferSize > 512)
            {
                throw new InvalidOperationException("DateTimeOffset format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a DateTimeOffset.
    /// </summary>
    /// <param name="value">
    /// The DateTimeOffset value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the DateTimeOffset.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the DateTimeOffset.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(DateTimeOffset value, string? format, IFormatProvider? provider, int bufferSize,
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