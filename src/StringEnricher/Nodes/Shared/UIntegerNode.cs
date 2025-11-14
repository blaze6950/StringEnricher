using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors.LengthCalculation;
using StringEnricher.Buffer.States;
using StringEnricher.Configuration;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an uinteger.
/// </summary>
public struct UIntegerNode : INode
{
    private readonly uint _uinteger;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UIntegerNode"/> struct.
    /// </summary>
    /// <param name="uinteger">
    /// The uinteger value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// An optional format string that defines how the uinteger should be formatted.
    /// </param>
    /// <param name="provider">
    /// An optional format provider that supplies culture-specific formatting information.
    /// </param>
    public UIntegerNode(uint uinteger, string? format = null, IFormatProvider? provider = null)
    {
        _uinteger = uinteger;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= GetUIntLength(_uinteger, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) =>
        _uinteger.TryFormat(destination, out var textLength, _format, _provider)
            ? textLength
            : throw new ArgumentException("The destination span is too small to hold the unsigned integer value.",
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
        _uinteger.TryFormat(buffer, out _, _format, _provider);
        character = buffer[index];
        return true;
    }

    /// <summary>
    /// Implicitly converts an uinteger to a <see cref="UIntegerNode"/>.
    /// </summary>
    /// <param name="uinteger">Source uinteger</param>
    /// <returns><see cref="UIntegerNode"/></returns>
    public static implicit operator UIntegerNode(uint uinteger) => new(uinteger);

    /// <summary>
    /// Calculates the length of the uinteger when represented as a string.
    /// </summary>
    /// <param name="value">
    /// The uinteger value whose length is to be calculated.
    /// </param>
    /// <param name="format">
    /// An optional format string that defines how the uinteger should be formatted.
    /// </param>
    /// <param name="provider">
    /// An optional format provider that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// The length of the uinteger when formatted as a string.
    /// </returns>
    private static int GetUIntLength(uint value, string? format, IFormatProvider? provider)
    {
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<uint>(value, format, provider);

        // tries to allocate a buffer and use the processor to get the length of the formatted value
        var length = BufferUtils.AllocateBuffer<UIntegerLengthProcessor, FormattingState<uint>, int>(
            processor: new UIntegerLengthProcessor(),
            state: in state,
            nodeSettings: StringEnricherSettings.Nodes.Shared.UIntegerNode
        );

        return length;
    }
}