using System.Buffers;
using StringEnricher.Configuration;
using StringEnricher.Utils;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an integer.
/// </summary>
public struct IntegerNode : INode
{
    private readonly int _integer;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegerNode"/> struct.
    /// </summary>
    /// <param name="integer">
    /// The integer value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// An optional format string that defines how the integer should be formatted.
    /// </param>
    /// <param name="provider">
    /// An optional format provider that supplies culture-specific formatting information.
    /// </param>
    public IntegerNode(int integer, string? format = null, IFormatProvider? provider = null)
    {
        _integer = integer;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= GetIntLength(_integer, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _integer.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the boolean value.",
            nameof(destination));

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        Span<char> buffer = stackalloc char[TotalLength];
        _integer.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts an integer to a <see cref="IntegerNode"/>.
    /// </summary>
    /// <param name="integer">Source integer</param>
    /// <returns><see cref="IntegerNode"/></returns>
    public static implicit operator IntegerNode(int integer) => new(integer);

    /// <summary>
    /// Calculates the length of the integer when represented as a string.
    /// </summary>
    /// <param name="value">
    /// The integer value whose length is to be calculated.
    /// </param>
    /// <param name="format">
    /// An optional format string that defines how the integer should be formatted.
    /// </param>
    /// <param name="provider">
    /// An optional format provider that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// The length of the integer when formatted as a string.
    /// </returns>
    private static int GetIntLength(int value, string? format, IFormatProvider? provider)
    {
        var bufferSize = StringEnricherSettings.Nodes.Shared.IntegerNode.InitialBufferSize;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var dateOnlyLength))
            {
                return dateOnlyLength;
            }

            bufferSize = BufferSizeUtils.GetNewBufferSize(bufferSize,
                StringEnricherSettings.Nodes.Shared.IntegerNode.GrowthFactor);

            if (bufferSize > StringEnricherSettings.Nodes.Shared.IntegerNode.MaxBufferSize)
            {
                throw new InvalidOperationException("int format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a int.
    /// </summary>
    /// <param name="value">
    /// The int value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the int to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the int to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the int.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the int.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(int value, string? format, IFormatProvider? provider, int bufferSize,
        out int length)
    {
        length = 0;

        if (bufferSize <= StringEnricherSettings.Nodes.Shared.IntegerNode.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            if (!value.TryFormat(buffer, out var charsWritten, format, provider))
            {
                return false;
            }

            length = charsWritten;
        }
        else if (bufferSize <= StringEnricherSettings.Nodes.Shared.IntegerNode.MaxPooledArrayLength)
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