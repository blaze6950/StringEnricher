using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using StringEnricher.Buffer;
using StringEnricher.Buffer.Results;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;

namespace StringEnricher.Discord.Nodes.Markdown;

/// <summary>
/// Escapes Discord markdown reserved characters for the given text.
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be escaped.
/// </typeparam>
[DebuggerDisplay("{typeof(EscapeNode).Name,nq} InnerType={typeof(TInner).Name,nq}")]
public struct EscapeNode<TInner> : INode
    where TInner : INode
{
    private const char
        EscapeSymbol =
            '\\'; // Character used to escape special characters in Discord markdown. '\\' is the escaped form of '\'.

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
    /// Returns the escaped string representation of Discord markdown string.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var charCountsResult = _innerText.GetTotalAndEscapedCharsCounts(
            IsCharacterToEscape,
            StringEnricherSettings.Extensions.StringBuilder,
            format,
            formatProvider
        );

        return string.Create(
            charCountsResult.TotalCount + charCountsResult.EscapedCount, // Total length after escaping
            ValueTuple.Create(_innerText, charCountsResult, format, formatProvider),
            static (span, state) =>
            {
                BufferUtils.StreamBuffer(
                    source: state.Item1,
                    destination: span,
                    streamWriter: static (c, index, destination) =>
                    {
                        if (!IsCharacterToEscape(c))
                        {
                            destination[0] = c;
                            return 1;
                        }

                        destination[0] = EscapeSymbol;
                        destination[1] = c;
                        return 2;
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
                streamWriter: static (c, index, destination) =>
                {
                    if (!IsCharacterToEscape(c))
                    {
                        destination[0] = c;
                        return 1;
                    }

                    destination[0] = EscapeSymbol;
                    destination[1] = c;
                    return 2;
                },
                nodeSettings: (NodeSettingsInternal)StringEnricherSettings.Extensions.StringBuilder,
                format: format.IsEmpty ? null : format.ToString(),
                provider: provider,
                initialBufferLengthHint: _innerLength
            );
        }
        catch (IndexOutOfRangeException)
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
    public int InnerLength => _innerLength ??= CalculateLength(_innerText).TotalCount;

    private int? _innerLength;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int SyntaxLength => _syntaxLength ??= CalculateLength(_innerText).EscapedCount;

    private int? _syntaxLength;

    /// <inheritdoc />
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??= SyntaxLength + InnerLength;

    private int? _totalLength;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination) => BufferUtils.StreamBuffer(
        source: _innerText,
        destination: destination,
        streamWriter: static (c, index, destination) =>
        {
            if (!IsCharacterToEscape(c))
            {
                destination[0] = c;
                return 1;
            }

            destination[0] = EscapeSymbol;
            destination[1] = c;
            return 2;
        },
        nodeSettings: (NodeSettingsInternal)StringEnricherSettings.Extensions.StringBuilder,
        initialBufferLengthHint: _syntaxLength.HasValue ? _innerText.TotalLength : null
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
        while (_innerText.TryGetChar(originalIndex, out character))
        {
            if (IsCharacterToEscape(character))
            {
                if (virtualIndex == neededIndex)
                {
                    // Add the escape character virtually
                    character = EscapeSymbol;

                    // We are at the position of the escape character
                    // Return the escape character
                    return true;
                }

                // Add the escape character virtually
                virtualIndex++;

                if (virtualIndex == neededIndex)
                {
                    // We are at the position of the escaped character
                    // Return the escaped character
                    return true;
                }
            }

            if (virtualIndex == neededIndex)
            {
                return true;
            }

            originalIndex++;
            virtualIndex++;
        }

        character = '\0';
        return false;
    }

    /// <summary>
    /// Escapes Discord markdown reserved characters for the given text.
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
    /// A <see cref="TotalAndEscapedCharCountsResult"/> containing the total and escaped character counts.
    /// </returns>
    private TotalAndEscapedCharCountsResult CalculateLength(TInner innerText)
    {
        var result = innerText.GetTotalAndEscapedCharsCounts(
            IsCharacterToEscape,
            StringEnricherSettings.Extensions.StringBuilder
        );
        _innerLength ??= result.TotalCount;
        _syntaxLength ??= result.EscapedCount;
        return result;
    }

    /// <summary>
    /// Determines whether the specified character is a Discord markdown character that needs to be escaped.
    /// </summary>
    /// <param name="character">
    /// The character to check.
    /// </param>
    /// <returns>
    /// true if the character is a Discord markdown character that needs to be escaped; otherwise, false.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCharacterToEscape(char character) => character switch
    {
        '_' => true,
        '*' => true,
        '~' => true,
        '`' => true,
        '#' => true,
        '-' => true,
        '[' => true,
        ']' => true,
        '(' => true,
        ')' => true,
        '>' => true,
        '|' => true,
        '\\' => true,
        _ => false
    };
}