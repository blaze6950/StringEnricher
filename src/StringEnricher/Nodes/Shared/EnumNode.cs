using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors.LengthCalculation;
using StringEnricher.Buffer.States;
using StringEnricher.Configuration;

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
    public int CopyTo(Span<char> destination) => Enum.TryFormat(_enum, destination, out var textLength, _format)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the enum value.",
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
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<TEnum>(value, format);

        // tries to allocate a buffer and use the processor to get the length of the formatted value
        var length = BufferUtils.AllocateBuffer<EnumLengthProcessor<TEnum>, FormattingState<TEnum>, int>(
            processor: new EnumLengthProcessor<TEnum>(),
            state: in state,
            nodeSettings: StringEnricherSettings.Nodes.Shared.EnumNode
        );

        return length;
    }
}