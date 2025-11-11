using System.Buffers;
using StringEnricher.Configuration;
using StringEnricher.Utils;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a timeOnly.
/// </summary>
public readonly struct TimeOnlyNode : INode
{
    private readonly TimeOnly _timeOnly;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeOnlyNode"/> struct.
    /// </summary>
    /// <param name="timeOnly">
    /// The timeOnly value.
    /// </param>
    /// <param name="format">
    /// The format string.
    /// </param>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    public TimeOnlyNode(TimeOnly timeOnly, string? format = null, IFormatProvider? provider = null)
    {
        _timeOnly = timeOnly;
        _format = format;
        _provider = provider;
        TotalLength = GetTimeOnlyLength(_timeOnly, _format, _provider);
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

        _timeOnly.TryFormat(destination, out _, _format, _provider);

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
        _timeOnly.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a timeOnly to a <see cref="TimeOnlyNode"/>.
    /// </summary>
    /// <param name="timeOnly">Source timeOnly</param>
    /// <returns><see cref="TimeOnlyNode"/></returns>
    public static implicit operator TimeOnlyNode(TimeOnly timeOnly) => new(timeOnly);

    /// <summary>
    /// Gets the length of the string representation of a timeOnly.
    /// </summary>
    /// <param name="value">
    /// The timeOnly value.
    /// </param>
    /// <param name="format">
    /// The format string.
    /// </param>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    /// <returns>
    /// The length of the string representation of the timeOnly.
    /// </returns>
    private static int GetTimeOnlyLength(TimeOnly value, string? format = null, IFormatProvider? provider = null)
    {
        var bufferSize = StringEnricherSettings.Nodes.Shared.TimeOnlyNode.InitialBufferSize;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var dateOnlyLength))
            {
                return dateOnlyLength;
            }

            bufferSize = BufferSizeUtils.GetNewBufferSize(bufferSize,
                StringEnricherSettings.Nodes.Shared.TimeOnlyNode.GrowthFactor);

            if (bufferSize > StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxBufferSize)
            {
                throw new InvalidOperationException("TimeOnly format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a TimeOnly.
    /// </summary>
    /// <param name="value">
    /// The TimeOnly value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the TimeOnly.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the TimeOnly.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(TimeOnly value, string? format, IFormatProvider? provider,
        int bufferSize, out int length)
    {
        length = 0;

        if (bufferSize <= StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            if (!value.TryFormat(buffer, out var charsWritten, format, provider))
            {
                return false;
            }

            length = charsWritten;
        }
        else if (bufferSize <= StringEnricherSettings.Nodes.Shared.TimeOnlyNode.MaxPooledArrayLength)
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