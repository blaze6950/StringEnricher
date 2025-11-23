using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Debug;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a double.
/// </summary>
[DebuggerDisplay("{typeof(DoubleNode).Name,nq} Value={_double} Format={_format} Provider={_provider}")]
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
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _double.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.DoubleNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? provider)
    {
        format = string.IsNullOrEmpty(format) ? _format : format;
        provider ??= _provider;

        var length = _double.GetSpanFormattableLength(
            nodeSettings: StringEnricherSettings.Nodes.Shared.DoubleNode,
            format: format,
            provider: provider
        );

        return string.Create(
            length: length,
            state: ValueTuple.Create(_double, format, provider),
            action: static (span, state) =>
            {
                if (!state.Item1.TryFormat(span, out _, state.Item2, state.Item3))
                {
                    throw new InvalidOperationException("Formatting failed unexpectedly.");
                }
            }
        );
    }

    /// <inheritdoc />
    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null
    ) => _double.TryFormat(
        destination: destination,
        charsWritten: out charsWritten,
        format: format.IsEmpty ? _format : format,
        provider: provider ?? _provider
    );

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _double.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the double value.",
            nameof(destination));

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0)
        {
            character = '\0';
            return false;
        }

        // if we already have the total length cached, use it to quickly determine if the index is valid
        if (_totalLength.HasValue && index >= _totalLength.Value)
        {
#if UNIT_TESTS
            DebugCounters.DoubleNode_TryGetChar_CachedTotalLengthEvaluation++;
#endif
            character = '\0';
            return false;
        }

        var result = _double.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.DoubleNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts a double to a <see cref="DoubleNode"/>.
    /// </summary>
    /// <param name="double">Source double</param>
    /// <returns><see cref="DoubleNode"/></returns>
    public static implicit operator DoubleNode(double @double) => new(@double);
}