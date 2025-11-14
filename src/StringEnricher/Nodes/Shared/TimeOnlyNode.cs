using StringEnricher.Buffer;
using StringEnricher.Buffer.Processors;
using StringEnricher.Buffer.Processors.LengthCalculation;
using StringEnricher.Configuration;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a timeOnly.
/// </summary>
public struct TimeOnlyNode : INode
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
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int TotalLength => _totalLength ??= GetTimeOnlyLength(_timeOnly, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) =>
        _timeOnly.TryFormat(destination, out var textLength, _format, _provider)
            ? textLength
            : throw new ArgumentException("The destination span is too small to hold the TimeOnly value.",
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
        // prepare state - the value and everything needed for formatting into an allocated buffer
        var state = new FormattingState<TimeOnly>(value, format, provider);

        // tries to allocate a buffer and use the processor to get the length of the formatted value
        var length = BufferUtils.AllocateBuffer<TimeOnlyLengthProcessor, FormattingState<TimeOnly>, int>(
            func: new TimeOnlyLengthProcessor(),
            state: in state,
            nodeSettings: StringEnricherSettings.Nodes.Shared.TimeOnlyNode
        );

        return length;
    }
}