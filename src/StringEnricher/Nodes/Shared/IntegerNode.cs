using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors;
using StringEnricher.Configuration;

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
        : throw new ArgumentException("The destination span is too small to hold the integer value.",
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
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<int>(value, format, provider);

        // tries to allocate a buffer and use the processor to get the length of the formatted value
        var length = BufferUtils.AllocateBuffer<IntegerLengthProcessor, FormattingState<int>, int>(
            func: new IntegerLengthProcessor(),
            state: in state,
            nodeSettings: StringEnricherSettings.Nodes.Shared.IntegerNode
        );

        return length;
    }
}