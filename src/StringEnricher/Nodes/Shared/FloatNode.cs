using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Debug;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a float.
/// </summary>
[DebuggerDisplay("{typeof(FloatNode).Name,nq} Value={_float} Format={_format} Provider={_provider}")]
public struct FloatNode : INode
{
    private readonly float _float;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="FloatNode"/> struct.
    /// </summary>
    /// <param name="float">
    /// The float value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string that defines the format of the float.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    public FloatNode(float @float, string? format = null, IFormatProvider? provider = null)
    {
        _float = @float;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _float.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.FloatNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? provider)
    {
        format = string.IsNullOrEmpty(format) ? _format : format;
        provider ??= _provider;

        var length = _float.GetSpanFormattableLength(
            nodeSettings: StringEnricherSettings.Nodes.Shared.FloatNode,
            format: format,
            provider: provider
        );

        return string.Create(
            length: length,
            state: ValueTuple.Create(_float, format, provider),
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
    ) => _float.TryFormat(
        destination: destination,
        charsWritten: out charsWritten,
        format: format.IsEmpty ? _format : format,
        provider: provider ?? _provider
    );

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _float.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the float value.",
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
            DebugCounters.FloatNode_TryGetChar_CachedTotalLengthEvaluation++;
#endif
            character = '\0';
            return false;
        }

        var result = _float.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.FloatNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts a float to a <see cref="FloatNode"/>.
    /// </summary>
    /// <param name="float">Source float</param>
    /// <returns><see cref="FloatNode"/></returns>
    public static implicit operator FloatNode(float @float) => new(@float);
}