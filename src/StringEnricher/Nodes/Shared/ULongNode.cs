using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Debug;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents an ulong.
/// </summary>
[DebuggerDisplay("{typeof(ULongNode).Name,nq} Value={_ulong} Format={_format} Provider={_provider}")]
public struct ULongNode : INode
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
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _ulong.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.ULongNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? provider)
    {
        format = string.IsNullOrEmpty(format) ? _format : format;
        provider ??= _provider;

        var length = _ulong.GetSpanFormattableLength(
            nodeSettings: StringEnricherSettings.Nodes.Shared.ULongNode,
            format: format,
            provider: provider
        );

        return string.Create(
            length: length,
            state: ValueTuple.Create(_ulong, format, provider),
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
    ) => _ulong.TryFormat(
        destination: destination,
        charsWritten: out charsWritten,
        format: format.IsEmpty ? _format : format,
        provider: provider ?? _provider
    );

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _ulong.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the unsigned long value.",
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
            DebugCounters.ULongNode_TryGetChar_CachedTotalLengthEvaluation++;
#endif
            character = '\0';
            return false;
        }

        var result = _ulong.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.ULongNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts a ulong to a <see cref="ULongNode"/>.
    /// </summary>
    /// <param name="ulong">Source ulong</param>
    /// <returns><see cref="ULongNode"/></returns>
    public static implicit operator ULongNode(ulong @ulong) => new(@ulong);
}