using System.Buffers;
using StringEnricher.Configuration;
using StringEnricher.Utils;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a dateOnly.
/// </summary>
public struct DateOnlyNode : INode
{
    private readonly DateOnly _dateOnly;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateOnlyNode"/> struct.
    /// </summary>
    /// <param name="dateOnly">
    /// The dateOnly value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateOnly to a string.
    /// </param>
    public DateOnlyNode(DateOnly dateOnly, string? format = null, IFormatProvider? provider = null)
    {
        _dateOnly = dateOnly;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= GetDateOnlyLength(_dateOnly, _format, _provider);
    private int? _totalLength;

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

        _dateOnly.TryFormat(destination, out _, _format, _provider);

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
        _dateOnly.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a dateOnly to a <see cref="DateOnlyNode"/>.
    /// </summary>
    /// <param name="dateOnly">Source dateOnly</param>
    /// <returns><see cref="DateOnlyNode"/></returns>
    public static implicit operator DateOnlyNode(DateOnly dateOnly) => new(dateOnly);

    /// <summary>
    /// Gets the length of the string representation of a dateOnly.
    /// </summary>
    /// <param name="value">
    /// The dateOnly value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateOnly to a string.
    /// </param>
    /// <returns>
    /// The length of the string representation of the dateOnly.
    /// </returns>
    private static int GetDateOnlyLength(DateOnly value, string? format, IFormatProvider? provider)
    {
        var bufferSize = StringEnricherSettings.Nodes.Shared.DateOnlyNode.InitialBufferSize;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var dateOnlyLength))
            {
                return dateOnlyLength;
            }

            bufferSize = BufferSizeUtils.GetNewBufferSize(bufferSize,
                StringEnricherSettings.Nodes.Shared.DateOnlyNode.GrowthFactor);

            if (bufferSize > StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxBufferSize)
            {
                throw new InvalidOperationException("DateOnly format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a dateOnly.
    /// </summary>
    /// <param name="value">
    /// The dateOnly value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the dateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the dateOnly to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the dateOnly.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the dateOnly.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(DateOnly value, string? format, IFormatProvider? provider, int bufferSize,
        out int length)
    {
        length = 0;

        if (bufferSize <= StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            if (!value.TryFormat(buffer, out var charsWritten, format, provider))
            {
                return false;
            }

            length = charsWritten;
        }
        else if (bufferSize <= StringEnricherSettings.Nodes.Shared.DateOnlyNode.MaxPooledArrayLength)
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