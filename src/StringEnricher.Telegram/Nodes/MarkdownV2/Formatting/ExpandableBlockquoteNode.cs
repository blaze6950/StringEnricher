using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using StringEnricher.Buffer;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.MarkdownV2.Formatting;

/// <summary>
/// Represents expandable blockquote text in MarkdownV2 format.
/// Example: "&gt;blockquote text example\n&gt;new expandable blockquote line||"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with expandable blockquote syntax.
/// </typeparam>
[DebuggerDisplay(
    "{typeof(ExpandableBlockquoteNode).Name,nq} LinePrefix={LinePrefix} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
public struct ExpandableBlockquoteNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix used for each line in an expandable blockquote in MarkdownV2.
    /// </summary>
    public const char LinePrefix = '>';

    /// <summary>
    /// The character used to separate lines in the expandable blockquote.
    /// </summary>
    public const char LineSeparator = '\n';

    /// <summary>
    /// The suffix used to denote the end of an expandable blockquote in MarkdownV2.
    /// </summary>
    public const string Suffix = "||";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandableBlockquoteNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style that will be wrapped with expandable blockquote syntax.
    /// </param>
    public ExpandableBlockquoteNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the string representation of the expandable blockquote style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var charCountsResult = _innerText.GetTotalAndEscapedCharsCounts(
            IsLineSeparator,
            StringEnricherSettings.Extensions.StringBuilder,
            format,
            formatProvider
        );

        return string.Create(
            charCountsResult.TotalCount + charCountsResult.EscapedCount, // Total length after escaping
            ValueTuple.Create(_innerText, charCountsResult, format, formatProvider),
            static (span, state) =>
            {
                span[0] = LinePrefix; // Write the line prefix

                BufferUtils.StreamBuffer(
                    source: state.Item1,
                    destination: span[1..], // Start writing after the first line prefix
                    streamWriter: static (c, _, destination) =>
                    {
                        if (!IsLineSeparator(c))
                        {
                            // Just write the character as is
                            destination[0] = c;
                            return 1;
                        }

                        destination[0] = c; // Write the line separator first
                        destination[1] = LinePrefix; // Then write the line prefix

                        // Return the total number of characters written
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
            charsWritten = 1; // For the first line prefix
            destination[0] = LinePrefix; // Write the line prefix

            charsWritten += BufferUtils.StreamBuffer(
                source: _innerText,
                destination: destination[1..], // Start writing after the first line prefix
                streamWriter: static (c, _, destination) =>
                {
                    if (!IsLineSeparator(c))
                    {
                        // Just write the character as is
                        destination[0] = c;
                        return 1;
                    }

                    destination[0] = c; // Write the line separator first
                    destination[1] = LinePrefix; // Then write the line prefix

                    // Return the total number of characters written
                    return 2;
                },
                nodeSettings: (NodeSettingsInternal)StringEnricherSettings.Extensions.StringBuilder,
                format: format.IsEmpty ? null : format.ToString(),
                provider: provider,
                initialBufferLengthHint: _innerLength
            );

            // Add the suffix length
            for (var i = 0; i < Suffix.Length; i++)
            {
                destination[charsWritten + i] = Suffix[i];
            }

            charsWritten += Suffix.Length;
        }
        catch (IndexOutOfRangeException)
        {
            charsWritten = 0;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Gets the length of the inner text without the expandable blockquote syntax.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _innerLength ??= CalculateLength(_innerText).Item1;

    private int? _innerLength;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int SyntaxLength => _syntaxLength ??= CalculateLength(_innerText).Item2;

    private int? _syntaxLength;

    /// <inheritdoc />
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => _totalLength ??= CalculateLength(_innerText).Item3;

    private int? _totalLength;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var charsWritten = 1;
        destination[0] = LinePrefix; // Write the line prefix

        charsWritten += BufferUtils.StreamBuffer(
            source: _innerText,
            destination: destination[1..], // Start writing after the first line prefix
            streamWriter: static (c, index, destination) =>
            {
                if (!IsLineSeparator(c))
                {
                    // Just write the character as is
                    destination[0] = c;
                    return 1;
                }

                destination[0] = c; // Write the line separator first
                destination[1] = LinePrefix; // Then write the line prefix

                // Return the total number of characters written
                return 2;
            },
            nodeSettings: (NodeSettingsInternal)StringEnricherSettings.Extensions.StringBuilder,
            initialBufferLengthHint: _syntaxLength.HasValue ? _innerText.TotalLength : null
        );

        // Add the suffix length
        for (var i = 0; i < Suffix.Length; i++)
        {
            destination[charsWritten + i] = Suffix[i];
        }

        charsWritten += Suffix.Length;

        return charsWritten;
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        if (index == 0)
        {
            character = LinePrefix;
            return true;
        }

        var neededIndex = index - 1; // Adjust for the first line prefix character that is always present at index 0
        var virtualIndex = 0;
        var originalIndex = 0;
        while (_innerText.TryGetChar(originalIndex, out character))
        {
            if (character == LineSeparator)
            {
                // means that the next character should be a line prefix that should be virtually added
                // so the next iteration should process a virtually added line prefix character
                // original index should not be incremented in this case
                // but virtual index should be incremented to account for the added character

                if (virtualIndex == neededIndex)
                {
                    // We are at the position of the line separator
                    // Return the line separator character
                    return true;
                }

                // Add the line prefix character virtually
                character = LinePrefix;
                virtualIndex++;

                if (virtualIndex == neededIndex)
                {
                    // We are at the position of the virtually added line prefix
                    // Return the line prefix character
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

        var suffixIndex = neededIndex - virtualIndex;

        if (suffixIndex >= 0 && suffixIndex < Suffix.Length)
        {
            character = Suffix[suffixIndex];
            return true;
        }

        character = '\0';
        return false;
    }

    /// <summary>
    /// Applies the expandable blockquote style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with expandable blockquote syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ExpandableBlockquoteNode{TInner}"/> containing the inner style.
    /// </returns>
    public static ExpandableBlockquoteNode<TInner> Apply(TInner innerStyle) => new(innerStyle);

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
        var result = innerText.GetTotalAndEscapedCharsCounts(
            IsLineSeparator,
            StringEnricherSettings.Extensions.StringBuilder
        );

        // Each line gets a '>' prefix, so the number of syntax characters is equal to the number of line separators
        // plus one for the first line prefix. Also add the length of the suffix.
        var syntaxLength = result.EscapedCount + 1 + Suffix.Length;

        _innerLength = result.TotalCount;
        _syntaxLength = syntaxLength;
        _totalLength = _innerLength + _syntaxLength;

        return ValueTuple.Create(_innerLength.Value, _syntaxLength.Value, _totalLength!.Value);
    }

    /// <summary>
    /// Determines if the given character is a line separator.
    /// </summary>
    /// <param name="character">
    /// The character to check.
    /// </param>
    /// <returns>
    /// ><c>true</c> if the character is a line separator; otherwise, <c>false</c>.
    /// </returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsLineSeparator(char character) => character == LineSeparator;
}