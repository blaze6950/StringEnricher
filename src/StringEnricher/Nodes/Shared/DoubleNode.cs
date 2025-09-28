using System.Buffers;
using StringEnricher.Configuration;
using StringEnricher.Utils;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a double.
/// </summary>
public readonly struct DoubleNode : INode
{
    private readonly double _double;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleNode"/> struct.
    /// </summary>
    /// <param name="double">
    /// The double value to represent.
    /// </param>
    /// <param name="format">
    /// The format string (optional).
    /// </param>
    /// <param name="provider">
    /// The format provider (optional).
    /// </param>
    public DoubleNode(double @double, string? format = null, IFormatProvider? provider = null)
    {
        _double = @double;
        _format = format;
        _provider = provider;
        TotalLength = GetDoubleLength(@double, _format, _provider);
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

        _double.TryFormat(destination, out _, _format, _provider);

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
        _double.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a double to a <see cref="DoubleNode"/>.
    /// </summary>
    /// <param name="double">Source double</param>
    /// <returns><see cref="DoubleNode"/></returns>
    public static implicit operator DoubleNode(double @double) => new(@double);

    /// <summary>
    /// Calculates the length of the double when represented as a string.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="format"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    private static int GetDoubleLength(double value, string? format = null, IFormatProvider? provider = null)
    {
        var bufferSize = StringEnricherSettings.Nodes.Shared.DoubleNode.InitialBufferSize;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var dateOnlyLength))
            {
                return dateOnlyLength;
            }

            bufferSize = BufferSizeUtils.CalculateBufferGrowth(bufferSize,
                StringEnricherSettings.Nodes.Shared.DoubleNode.GrowthFactor);

            if (bufferSize > StringEnricherSettings.Nodes.Shared.DoubleNode.MaxBufferSize)
            {
                throw new InvalidOperationException("double format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a double.
    /// </summary>
    /// <param name="value">
    /// The double value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the double.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the double.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(double value, string? format, IFormatProvider? provider, int bufferSize,
        out int length)
    {
        length = 0;

        if (bufferSize <= StringEnricherSettings.Nodes.Shared.DoubleNode.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            if (!value.TryFormat(buffer, out var charsWritten, format, provider))
            {
                return false;
            }

            length = charsWritten;
        }
        else if (bufferSize <= StringEnricherSettings.Nodes.Shared.DoubleNode.MaxPooledArrayLength)
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