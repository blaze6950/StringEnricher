﻿using System.Diagnostics;
using StringEnricher.Configuration;
using StringEnricher.Extensions;
using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.Html.Formatting;

/// <summary>
/// Represents spoiler text in HTML format.
/// Example: "&lt;tg-spoiler&gt;spoiler text&lt;/tg-spoiler&gt;"
/// </summary>
[DebuggerDisplay("{typeof(SpoilerNode).Name,nq} Prefix={Prefix} InnerType={typeof(TInner).Name,nq} Suffix={Suffix}")]
public readonly struct SpoilerNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The opening spoiler tag.
    /// </summary>
    public const string Prefix = "<tg-spoiler>";

    /// <summary>
    /// The closing spoiler tag.
    /// </summary>
    public const string Suffix = "</tg-spoiler>";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpoilerNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">The inner style to be wrapped with spoiler HTML tags.</param>
    public SpoilerNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <inheritdoc/>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? provider)
    {
        var length = this.GetSpanFormattableLength(
            nodeSettings: StringEnricherSettings.Extensions.StringBuilder,
            format: format,
            provider: provider
        );

        return string.Create(
            length: length,
            state: ValueTuple.Create(this, format, provider),
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
    )
    {
        charsWritten = 0;

        // Copy prefix
        if (!Prefix.AsSpan().TryCopyTo(destination))
        {
            return false;
        }

        charsWritten += Prefix.Length;

        // Copy inner text
        var isInnerTextFormatSuccess = _innerText.TryFormat(
            destination.SliceSafe(charsWritten),
            out var innerCharsWritten,
            format,
            provider
        );

        if (!isInnerTextFormatSuccess)
        {
            return false;
        }

        charsWritten += innerCharsWritten;

        // Copy suffix
        if (!Suffix.AsSpan().TryCopyTo(destination.SliceSafe(charsWritten)))
        {
            return false;
        }

        charsWritten += Suffix.Length;

        return true;
    }

    /// <summary>
    /// Gets the length of the inner text.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int InnerLength => _innerText.TotalLength;

    /// <summary>
    /// Gets the total length of the HTML spoiler syntax.
    /// </summary>
    public int SyntaxLength => Prefix.Length + Suffix.Length;

    /// <summary>
    /// Gets the total length of the formatted text.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TotalLength => SyntaxLength + InnerLength;

    /// <summary>
    /// Copies the formatted spoiler text to the provided span.
    /// </summary>
    /// <param name="destination">The span to copy the formatted text into.</param>
    /// <returns>The total length of the formatted text.</returns>
    /// <exception cref="ArgumentException">Thrown if the destination span is too small.</exception>
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;
        Prefix.AsSpan().CopyTo(destination);
        writtenChars += Prefix.Length;

        writtenChars += _innerText.CopyTo(destination[writtenChars..]);

        Suffix.AsSpan().CopyTo(destination[writtenChars..]);
        writtenChars += Suffix.Length;

        return writtenChars;
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        var totalLength = TotalLength;
        if (index < 0 || index >= totalLength)
        {
            character = '\0';
            return false;
        }

        if (index < Prefix.Length)
        {
            character = Prefix[index];
            return true;
        }

        if (index >= Prefix.Length + InnerLength)
        {
            character = Suffix[index - Prefix.Length - InnerLength];
            return true;
        }

        return _innerText.TryGetChar(index - Prefix.Length, out character);
    }

    public static SpoilerNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}