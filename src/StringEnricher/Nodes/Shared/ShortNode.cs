using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Debug;
using StringEnricher.Extensions;

namespace StringEnricher.Nodes.Shared;

/// <summary>
/// A style that represents a short.
/// </summary>
[DebuggerDisplay("{typeof(ShortNode).Name,nq} Value={_short} Format={_format} Provider={_provider}")]
public struct ShortNode : INode
{
    private readonly short _short;
    private readonly string? _format;
    private readonly IFormatProvider? _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShortNode"/> struct.
    /// </summary>
    /// <param name="short">
    /// The short value to be represented by this node.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string that defines the format of the short.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    public ShortNode(short @short, string? format = null, IFormatProvider? provider = null)
    {
        _short = @short;
        _format = format;
        _provider = provider;
    }

    /// <inheritdoc />
    public int SyntaxLength => 0;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??=
        _short.GetSpanFormattableLength(StringEnricherSettings.Nodes.Shared.ShortNode, _format, _provider);

    private int? _totalLength;

    /// <inheritdoc />
    public override string ToString() => string.Create(TotalLength, this, static (span, node) => node.CopyTo(span));

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? provider)
    {
        format = string.IsNullOrEmpty(format) ? _format : format;
        provider ??= _provider;

        var length = _short.GetSpanFormattableLength(
            nodeSettings: StringEnricherSettings.Nodes.Shared.ShortNode,
            format: format,
            provider: provider
        );

        return string.Create(
            length: length,
            state: ValueTuple.Create(_short, format, provider),
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
    ) => _short.TryFormat(
        destination: destination,
        charsWritten: out charsWritten,
        format: format.IsEmpty ? _format : format,
        provider: provider ?? _provider
    );

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => _short.TryFormat(destination, out var textLength, _format, _provider)
        ? textLength
        : throw new ArgumentException("The destination span is too small to hold the short value.",
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
            DebugCounters.ShortNode_TryGetChar_CachedTotalLengthEvaluation++;
#endif
            character = '\0';
            return false;
        }

        var result = _short.GetCharAtIndex(
            index: index,
            nodeSettings: StringEnricherSettings.Nodes.Shared.ShortNode,
            format: _format,
            provider: _provider,
            initialBufferLengthHint: _totalLength
        );

        character = result.Char;

        _totalLength ??= result.CharsWritten;

        return result.Success;
    }

    /// <summary>
    /// Implicitly converts a short to a <see cref="ShortNode"/>.
    /// </summary>
    /// <param name="short">Source short</param>
    /// <returns><see cref="ShortNode"/></returns>
    public static implicit operator ShortNode(short @short) => new(@short);
}