using System.Buffers;
using StringEnricher.Configuration;
using StringEnricher.Utils;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a dateTime.
/// </summary>
public readonly struct DateTimeNode : INode
{
    private readonly DateTime _dateTime;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeNode"/> struct.
    /// </summary>
    /// <param name="dateTime">
    /// The dateTime value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateTime to a string.
    /// </param>
    public DateTimeNode(DateTime dateTime, string? format = null, IFormatProvider? provider = null)
    {
        _dateTime = dateTime;
        _format = format;
        _provider = provider;
        TotalLength = GetDateTimeLength(_dateTime, _format, _provider);
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

        _dateTime.TryFormat(destination, out _, _format, _provider);

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
        _dateTime.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a dateTime to a <see cref="DateTimeNode"/>.
    /// </summary>
    /// <param name="dateTime">Source dateTime</param>
    /// <returns><see cref="DateTimeNode"/></returns>
    public static implicit operator DateTimeNode(DateTime dateTime) => new(dateTime);

    /// <summary>
    /// Gets the length of the string representation of a dateTime.
    /// </summary>
    /// <param name="value">
    /// The dateTime value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateTime to a string.
    /// </param>
    /// <returns>
    /// The length of the string representation of the dateTime ("true" or "false").
    /// </returns>
    private static int GetDateTimeLength(DateTime value, string? format = null, IFormatProvider? provider = null)
    {
        var bufferSize = StringEnricherSettings.Nodes.Shared.DateTimeNode.InitialBufferSize;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var dateOnlyLength))
            {
                return dateOnlyLength;
            }

            bufferSize = BufferSizeUtils.GetNewBufferSize(bufferSize,
                StringEnricherSettings.Nodes.Shared.DateTimeNode.GrowthFactor);

            if (bufferSize > StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxBufferSize)
            {
                throw new InvalidOperationException("DateTime format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a DateTime.
    /// </summary>
    /// <param name="value">
    /// The DateTime value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the DateTime.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the DateTime.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(DateTime value, string? format, IFormatProvider? provider, int bufferSize,
        out int length)
    {
        length = 0;

        if (bufferSize <= StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            if (!value.TryFormat(buffer, out var charsWritten, format, provider))
            {
                return false;
            }

            length = charsWritten;
        }
        else if (bufferSize <= StringEnricherSettings.Nodes.Shared.DateTimeNode.MaxPooledArrayLength)
        {
            // array pool for medium sizes (less pressure on the GC)
            var buffer = ArrayPool<char>.Shared.Rent(bufferSize);
            try
            {
                if (!value.TryFormat(buffer, out var charsWritten, format, provider))
                {
                    return false;
                }

                length = charsWritten;
            }
            finally
            {
                ArrayPool<char>.Shared.Return(buffer);
            }
        }
        else
        {
            // fallback: direct heap allocation (rare but safe)
            var buffer = new char[bufferSize];
            if (!value.TryFormat(buffer, out var charsWritten, format, provider))
            {
                return false;
            }

            length = charsWritten;
        }

        return true;
    }
}