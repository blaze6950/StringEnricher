using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors.LengthCalculation;
using StringEnricher.Buffer.States;
using StringEnricher.Configuration;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a double.
/// </summary>
public struct DoubleNode : INode
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
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= GetDoubleLength(_double, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _double.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the double value.",
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
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<double>(value, format, provider);

        // tries to allocate a buffer and use the processor to get the length of the formatted value
        var length = BufferUtils.AllocateBuffer<DoubleLengthProcessor, FormattingState<double>, int>(
            processor: new DoubleLengthProcessor(),
            state: in state,
            nodeSettings: StringEnricherSettings.Nodes.Shared.DoubleNode
        );

        return length;
    }
}