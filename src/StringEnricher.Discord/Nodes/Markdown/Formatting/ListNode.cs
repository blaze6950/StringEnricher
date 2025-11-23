using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using StringEnricher.Buffer;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;

namespace StringEnricher.Discord.Nodes.Markdown.Formatting;

/// <summary>
/// Represents a list in Discord markdown format.
/// Example: "- first item\n- second item\n- third item"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with list syntax.
/// </typeparam>
[DebuggerDisplay("{typeof(ListNode).Name,nq} LinePrefix={LinePrefix} InnerType={typeof(TInner).Name,nq}")]
public struct ListNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix used for each line in a list in Discord markdown.
    /// </summary>
    public const string LinePrefix = "- ";

    /// <summary>
    /// The character used to separate lines in the list.
    /// </summary>
    public const char LineSeparator = '\n';

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style that will be wrapped with list syntax.
    /// </param>
    public ListNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the string representation of the list style in Discord markdown format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var charCountsResult = this.GetTotalAndEscapedCharsCounts(
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
                // Write the line prefix
                for (var i = 0; i < LinePrefix.Length; i++)
                {
                    span[i] = LinePrefix[i];
                }

                BufferUtils.StreamBuffer(
                    source: state.Item1,
                    destination: span[LinePrefix.Length..],
                    streamWriter: static (c, _, destination) =>
                    {
                        if (!IsLineSeparator(c))
                        {
                            // Just write the character as is
                            destination[0] = c;
                            return 1;
                        }

                        // Write the line separator first
                        destination[0] = c;

                        // Then write the line prefix
                        for (var i = 1; i <= LinePrefix.Length; i++)
                        {
                            destination[i] = LinePrefix[i - 1];
                        }

                        // Return the total number of characters written
                        return 1 + LinePrefix.Length;
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
            charsWritten = 0;

            // Write the line prefix
            for (var i = 0; i < LinePrefix.Length; i++)
            {
                destination[i] = LinePrefix[i];
            }

            charsWritten += LinePrefix.Length;

            charsWritten += BufferUtils.StreamBuffer(
                source: _innerText,
                destination: destination.SliceSafe(LinePrefix.Length),
                streamWriter: static (c, _, destination) =>
                {
                    if (!IsLineSeparator(c))
                    {
                        // Just write the character as is
                        destination[0] = c;
                        return 1;
                    }

                    // Write the line separator first
                    destination[0] = c;

                    // Then write the line prefix
                    for (var i = 1; i <= LinePrefix.Length; i++)
                    {
                        destination[i] = LinePrefix[i - 1];
                    }

                    // Return the total number of characters written
                    return 1 + LinePrefix.Length;
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
    /// Gets the length of the inner text without the list syntax.
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
    public int CopyTo(Span<char> destination)
    {
        // Write the first line prefix
        for (var i = 0; i < LinePrefix.Length; i++)
        {
            destination[i] = LinePrefix[i];
        }

        var writtenChars = BufferUtils.StreamBuffer(
            source: _innerText,
            destination: destination[LinePrefix.Length..],
            streamWriter: static (c, _, destination) =>
            {
                if (!IsLineSeparator(c))
                {
                    // Just write the character as is
                    destination[0] = c;
                    return 1;
                }

                // Write the line separator first
                destination[0] = c;

                // Then write the line prefix
                for (var i = 1; i <= LinePrefix.Length; i++)
                {
                    destination[i] = LinePrefix[i - 1];
                }

                // Return the total number of characters written
                return 1 + LinePrefix.Length;
            },
            nodeSettings: (NodeSettingsInternal)StringEnricherSettings.Extensions.StringBuilder,
            initialBufferLengthHint: _innerLength
        );

        return writtenChars + LinePrefix.Length;
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        // Track our position in the virtual output
        var virtualIndex = 0;
        var linePrefixPosition = 0;

        // Start with the first line prefix
        while (linePrefixPosition < LinePrefix.Length)
        {
            if (virtualIndex == index)
            {
                character = LinePrefix[linePrefixPosition];
                return true;
            }

            virtualIndex++;
            linePrefixPosition++;
        }

        // Now iterate through the inner text
        var originalIndex = 0;
        while (_innerText.TryGetChar(originalIndex, out character))
        {
            if (virtualIndex == index)
            {
                return true;
            }

            virtualIndex++;
            originalIndex++;

            // If we just read a line separator, we need to add a line prefix after it
            if (character == LineSeparator)
            {
                linePrefixPosition = 0;
                while (linePrefixPosition < LinePrefix.Length)
                {
                    if (virtualIndex == index)
                    {
                        character = LinePrefix[linePrefixPosition];
                        return true;
                    }

                    virtualIndex++;
                    linePrefixPosition++;
                }
            }
        }

        character = '\0';
        return false;
    }

    /// <summary>
    /// Applies the list style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with list syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ListNode{TInner}"/> containing the inner style.
    /// </returns>
    public static ListNode<TInner> Apply(TInner innerStyle) => new(innerStyle);

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
        var (totalCount, lineSeparatorsCount) = innerText.GetTotalAndEscapedCharsCounts(
            IsLineSeparator,
            StringEnricherSettings.Extensions.StringBuilder
        );

        // Each line gets a prefix added
        // The number of prefixes added is equal to the number of line separators + 1 (for the first line)
        // Each prefix has a length of LinePrefix.Length
        var syntaxLength = (lineSeparatorsCount + 1) * LinePrefix.Length;

        _innerLength = totalCount;
        _syntaxLength = syntaxLength;
        _totalLength = totalCount + syntaxLength;

        return ValueTuple.Create(_innerLength.Value, _syntaxLength.Value, _totalLength.Value);
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