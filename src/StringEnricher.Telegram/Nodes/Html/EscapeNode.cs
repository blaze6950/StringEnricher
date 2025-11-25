using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using StringEnricher.Buffer;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.Html;

/// <summary>
/// Escapes HTML reserved characters for the given text.
/// All &lt;, &gt; and &amp; symbols that are not a part of a tag or an HTML entity must be replaced with the corresponding HTML entities.
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be escaped.
/// </typeparam>
[DebuggerDisplay("{typeof(EscapeNode).Name,nq} InnerType={typeof(TInner).Name,nq}")]
public struct EscapeNode<TInner> : INode
    where TInner : INode
{
    private const string EscapedEntity = "&lt;";
    private const string Entity = "&gt;";
    private const string Amp = "&amp;";
    private const char LessThanChar = '<';
    private const char GreaterThanChar = '>';
    private const char AmpersandChar = '&';

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="EscapeNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style that will be escaped.</param>
    public EscapeNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the escaped string representation of HTML string.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var charCountsResult = _innerText.GetCustomTotalAndEscapedCharsCounts(
            ToEscape,
            StringEnricherSettings.Extensions.StringBuilder,
            format,
            formatProvider
        );

        var totalCountWithoutEscaped = charCountsResult.TotalCount - charCountsResult.ToEscapeCount;
        var escapedCount = charCountsResult.EscapedCount;
        var totalLength = totalCountWithoutEscaped + escapedCount;

        return string.Create(
            totalLength, // Total length after escaping
            ValueTuple.Create(_innerText, charCountsResult, format, formatProvider),
            static (span, state) =>
            {
                BufferUtils.StreamBuffer(
                    source: state.Item1,
                    destination: span,
                    streamWriter: static (c, _, destination) =>
                    {
                        var escapedEntity = ToEscape(c);

                        if (escapedEntity is null)
                        {
                            destination[0] = c;
                            return 1;
                        }

                        // Write the escaped entity to the destination span
                        for (var i = 0; i < escapedEntity.Length; i++)
                        {
                            destination[i] = escapedEntity[i];
                        }

                        // Return the total number of characters written
                        return escapedEntity.Length;
                    },
                    nodeSettings: (NodeSettingsInternal)StringEnricherSettings.Extensions.StringBuilder,
                    format: state.Item3,
                    provider: state.Item4,
                    initialBufferLengthHint: state.Item2.TotalCount // Inner length before escaping
                );
            });
    }

    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        try
        {
            charsWritten = BufferUtils.StreamBuffer(
                source: _innerText,
                destination: destination,
                streamWriter: static (c, _, destination) =>
                {
                    var escapedEntity = ToEscape(c);

                    if (escapedEntity is null)
                    {
                        destination[0] = c;
                        return 1;
                    }

                    // Write the escaped entity to the destination span
                    for (var i = 0; i < escapedEntity.Length; i++)
                    {
                        destination[i] = escapedEntity[i];
                    }

                    // Return the total number of characters written
                    return escapedEntity.Length;
                },
                nodeSettings: (NodeSettingsInternal)StringEnricherSettings.Extensions.StringBuilder,
                format: format.IsEmpty ? null : format.ToString(),
                provider: provider,
                initialBufferLengthHint: _innerLength
            );
        }
        catch (Exception)
        {
            charsWritten = 0;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Gets the length of the inner text.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _innerLength ??= CalculateLength(_innerText).Item1;

    private int? _innerLength;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int SyntaxLength => _syntaxLength ??= CalculateLength(_innerText).Item2;

    private int? _syntaxLength;

    /// <inheritdoc />
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??= CalculateLength(_innerText).Item3;

    private int? _totalLength;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => BufferUtils.StreamBuffer(
        source: _innerText,
        destination: destination,
        streamWriter: static (c, _, destination) =>
        {
            var escapedEntity = ToEscape(c);

            if (escapedEntity is null)
            {
                destination[0] = c;
                return 1;
            }

            // Write the escaped entity to the destination span
            for (var i = 0; i < escapedEntity.Length; i++)
            {
                destination[i] = escapedEntity[i];
            }

            // Return the total number of characters written
            return escapedEntity.Length;
        },
        nodeSettings: (NodeSettingsInternal)StringEnricherSettings.Extensions.StringBuilder,
        initialBufferLengthHint: _innerLength
    );

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        var neededIndex = index;
        var virtualIndex = 0;
        var originalIndex = 0;

        while (_innerText.TryGetChar(originalIndex, out var originalChar))
        {
            var escapedEntity = originalChar switch
            {
                LessThanChar => EscapedEntity,
                GreaterThanChar => Entity,
                AmpersandChar => Amp,
                _ => null
            };

            if (escapedEntity != null)
            {
                // This character needs to be escaped with an HTML entity
                var entityLength = escapedEntity.Length;

                if (virtualIndex + entityLength > neededIndex)
                {
                    // The needed index falls within this entity
                    var entityCharIndex = neededIndex - virtualIndex;
                    character = escapedEntity[entityCharIndex];
                    return true;
                }

                virtualIndex += entityLength;
            }
            else
            {
                // Regular character, no escaping needed
                if (virtualIndex == neededIndex)
                {
                    character = originalChar;
                    return true;
                }

                virtualIndex++;
            }

            originalIndex++;
        }

        character = '\0';
        return false;
    }

    /// <summary>
    /// Escapes HTML reserved characters for the given text.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be escaped.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="EscapeNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static EscapeNode<TInner> Apply(TInner innerStyle) => new(innerStyle);

    /// <summary>
    /// Calculates the total and escaped character counts for the given inner text.
    /// It also caches the inner and syntax lengths.
    /// </summary>
    /// <param name="innerText">
    /// The inner text to calculate lengths for.
    /// </param>
    /// <returns>
    /// A tuple containing different lengths:
    /// 1. Inner text total length;
    /// 2. Number of escaped characters (i.e., added line prefixes);
    /// 3. Total length including syntax;
    /// </returns>
    private ValueTuple<int, int, int> CalculateLength(TInner innerText)
    {
        var result = innerText.GetCustomTotalAndEscapedCharsCounts(
            ToEscape,
            StringEnricherSettings.Extensions.StringBuilder
        );

        var totalCountWithoutEscaped = result.TotalCount - result.ToEscapeCount;

        _innerLength = result.TotalCount;
        _syntaxLength = result.EscapedCount;
        _totalLength = totalCountWithoutEscaped + _syntaxLength;

        return ValueTuple.Create(_innerLength.Value, _syntaxLength.Value, _totalLength!.Value);
    }

    /// <summary>
    /// Converts a character to its corresponding HTML escaped entity if needed.
    /// </summary>
    /// <param name="c">
    /// The character to convert.
    /// </param>
    /// <returns>
    /// The HTML escaped entity as a string, or the original character as a string if no escaping is needed.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? ToEscape(char c) => c switch
    {
        LessThanChar => EscapedEntity,
        GreaterThanChar => Entity,
        AmpersandChar => Amp,
        _ => null
    };
}