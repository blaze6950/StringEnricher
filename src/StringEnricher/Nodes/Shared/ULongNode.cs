using System.Buffers;
using StringEnricher.Configuration;
using StringEnricher.Utils;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an ulong.
/// </summary>
public readonly struct ULongNode : INode
{
    private readonly ulong _ulong;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ULongNode"/> struct.
    /// </summary>
    /// <param name="ulong">
    /// The ulong value to represent.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the ulong to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the ulong to a string.
    /// </param>
    public ULongNode(ulong @ulong, string? format = null, IFormatProvider? provider = null)
    {
        _ulong = @ulong;
        _format = format;
        _provider = provider;
        TotalLength = GetLongLength(@ulong, _format, _provider);
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

        _ulong.TryFormat(destination, out _, _format, _provider);

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
        _ulong.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts a ulong to a <see cref="ULongNode"/>.
    /// </summary>
    /// <param name="ulong">Source ulong</param>
    /// <returns><see cref="ULongNode"/></returns>
    public static implicit operator ULongNode(ulong @ulong) => new(@ulong);

    /// <summary>
    /// Calculates the length of the ulong when represented as a string.
    /// </summary>
    /// <param name="value">
    /// The ulong value to calculate the length for.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the ulong to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the ulong to a string.
    /// </param>
    /// <returns>
    /// The length of the ulong when represented as a string.
    /// </returns>
    private static int GetLongLength(ulong value, string? format = null, IFormatProvider? provider = null)
    {
        var bufferSize = StringEnricherSettings.Nodes.Shared.ULongNode.InitialBufferSize;
        while (true)
        {
            if (TryGetFormattedLength(value, format, provider, bufferSize, out var ulongLength))
            {
                return ulongLength;
            }

            bufferSize = BufferSizeUtils.CalculateBufferGrowth(bufferSize,
                StringEnricherSettings.Nodes.Shared.ULongNode.GrowthFactor);

            if (bufferSize > StringEnricherSettings.Nodes.Shared.ULongNode.MaxBufferSize)
            {
                                throw new InvalidOperationException("ulong format string is too long.");
            }
        }
    }

    /// <summary>
    /// Tries to get the length of the formatted string representation of a ulong.
    /// </summary>
    /// <param name="value">
    /// The ulong value.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the ulong to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the ulong to a string.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the ulong.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the ulong.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(ulong value, string? format, IFormatProvider? provider, int bufferSize,
        out int length)
    {
        length = 0;

        if (bufferSize <= StringEnricherSettings.Nodes.Shared.ULongNode.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            if (!value.TryFormat(buffer, out var charsWritten, format, provider))
            {
                return false;
            }

            length = charsWritten;
        }
        else if (bufferSize <= StringEnricherSettings.Nodes.Shared.ULongNode.MaxPooledArrayLength)
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