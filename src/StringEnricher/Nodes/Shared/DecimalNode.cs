using System.Buffers;
using StringEnricher.Configuration;
using StringEnricher.Utils;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a decimal.
/// </summary>
public readonly struct DecimalNode : INode
{
    private readonly decimal _decimal;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DecimalNode"/> struct.
    /// </summary>
    /// <param name="decimal">
    /// The decimal value to represent.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    public DecimalNode(decimal @decimal, string? format = null, IFormatProvider? provider = null)
    {
        _decimal = @decimal;
        _format = format;
        _provider = provider;
        TotalLength = GetDecimalLength(@decimal, _format, _provider);
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

        _decimal.TryFormat(destination, out _, _format, _provider);

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
        _decimal.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a decimal to a <see cref="DecimalNode"/>.
    /// </summary>
    /// <param name="decimal">Source decimal</param>
    /// <returns><see cref="DecimalNode"/></returns>
    public static implicit operator DecimalNode(decimal @decimal) => new(@decimal);

    /// <summary>
    /// Calculates the length of the decimal when represented as a string.
    /// </summary>
    /// <param name="value">
    /// The decimal value to measure.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// The length of the decimal when formatted as a string.
    /// </returns>
    private static int GetDecimalLength(decimal value, string? format = null, IFormatProvider? provider = null)
    {
        var bufferSize = StringEnricherSettings.Nodes.Shared.DecimalNode.InitialBufferSize;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var dateOnlyLength))
            {
                return dateOnlyLength;
            }

            bufferSize = BufferSizeUtils.GetNewBufferSize(bufferSize,
                StringEnricherSettings.Nodes.Shared.DecimalNode.GrowthFactor);

            if (bufferSize > StringEnricherSettings.Nodes.Shared.DecimalNode.MaxBufferSize)
            {
                throw new InvalidOperationException("decimal format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a decimal.
    /// </summary>
    /// <param name="value">
    /// The decimal value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the decimal.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the decimal.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(decimal value, string? format, IFormatProvider? provider, int bufferSize,
        out int length)
    {
        length = 0;

        if (bufferSize <= StringEnricherSettings.Nodes.Shared.DecimalNode.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            if (!value.TryFormat(buffer, out var charsWritten, format, provider))
            {
                return false;
            }

            length = charsWritten;
        }
        else if (bufferSize <= StringEnricherSettings.Nodes.Shared.DecimalNode.MaxPooledArrayLength)
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