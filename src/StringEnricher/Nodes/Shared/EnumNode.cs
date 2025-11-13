using System.Buffers;
using StringEnricher.Configuration;
using StringEnricher.Utils;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an enum.
/// </summary>
public struct EnumNode<TEnum> : INode where TEnum : struct, Enum
{
    private readonly TEnum _enum;
    private readonly string? _format;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumNode{TEnum}"/> struct.
    /// </summary>
    /// <param name="enum">
    /// The enum value to represent.
    /// </param>
    /// <param name="format">
    /// The format string (optional).
    /// </param>
    public EnumNode(TEnum @enum, string? format = null)
    {
        _enum = @enum;
        _format = format;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= GetEnumLength(_enum, _format);
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

        Enum.TryFormat(_enum, destination, out _, _format);

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
        Enum.TryFormat(_enum, buffer, out _, _format);
        character = buffer[index];

        return true;
    }

    /// <summary>
    /// Gets the length of the string representation of an enum.
    /// </summary>
    /// <param name="value">
    /// The enum value.
    /// </param>
    /// <param name="format">
    /// The format string (optional).
    /// </param>
    /// <returns>
    /// The length of the string representation of the enum.
    /// </returns>
    private static int GetEnumLength(TEnum value, string? format = null)
    {
        var bufferSize = StringEnricherSettings.Nodes.Shared.EnumNode.InitialBufferSize;
        while (true)
        {
            if (TryGetFormattedLength(value, format, bufferSize, out var enumLength))
            {
                return enumLength;
            }

            bufferSize = BufferSizeUtils.GetNewBufferSize(bufferSize,
                StringEnricherSettings.Nodes.Shared.EnumNode.GrowthFactor);

            if (bufferSize > StringEnricherSettings.Nodes.Shared.EnumNode.MaxBufferSize)
            {
                throw new InvalidOperationException("enum format string is too long.");
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
    /// <param name="bufferSize">
    /// The size of the buffer to use when formatting the int.
    /// </param>
    /// <param name="length">
    /// The length of the formatted string representation of the int.
    /// </param>
    /// <returns>
    /// True if the length was successfully obtained; otherwise, false.
    /// </returns>
    private static bool TryGetFormattedLength(TEnum value, string? format, int bufferSize,
        out int length)
    {
        length = 0;

        if (bufferSize <= StringEnricherSettings.Nodes.Shared.EnumNode.MaxStackAllocLength)
        {
            // stackalloc for small sizes (fastest)
            Span<char> buffer = stackalloc char[bufferSize];
            if (!Enum.TryFormat(value, buffer, out var charsWritten, format))
            {
                return false;
            }

            length = charsWritten;
        }
        else if (bufferSize <= StringEnricherSettings.Nodes.Shared.EnumNode.MaxPooledArrayLength)
        {
            // array pool for medium sizes (less pressure on the GC)
            var buffer = ArrayPool<char>.Shared.Rent(bufferSize);
            try
            {
                if (!Enum.TryFormat(value, buffer, out var charsWritten, format))
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
            if (!Enum.TryFormat(value, buffer, out var charsWritten, format))
            {
                return false;
            }

            length = charsWritten;
        }

        return true;
    }
}