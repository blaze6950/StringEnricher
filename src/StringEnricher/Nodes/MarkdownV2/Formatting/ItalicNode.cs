﻿namespace StringEnricher.Nodes.MarkdownV2.Formatting;

/// <summary>
/// Represents italic text in MarkdownV2 format.
/// Example: "_italic text_"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with italic syntax.
/// </typeparam>
public readonly struct ItalicNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix and suffix used to denote italic text in MarkdownV2 format.
    /// </summary>
    public const string Prefix = "_";

    /// <summary>
    /// The suffix used to denote italic text in MarkdownV2 format.
    /// </summary>
    public const string Suffix = "_";

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItalicNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style to be wrapped with italic syntax.
    /// </param>
    public ItalicNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the string representation of the italic style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text excluding the italic syntax.
    /// </summary>
    public int InnerLength => _innerText.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength => Prefix.Length + Suffix.Length;

    /// <inheritdoc />
    public int TotalLength => SyntaxLength + InnerLength;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var totalLength = TotalLength;
        if (destination.Length < totalLength)
        {
            throw new ArgumentException("The destination span is too small to hold the formatted text.");
        }

        var pos = 0;

        // Copy prefix
        Prefix.AsSpan().CopyTo(destination.Slice(pos, Prefix.Length));
        pos += Prefix.Length;

        // Copy inner text
        _innerText.CopyTo(destination.Slice(pos, InnerLength));
        pos += InnerLength;

        // Copy suffix
        Suffix.AsSpan().CopyTo(destination.Slice(pos, Suffix.Length));

        return totalLength;
    }

    /// <inheritdoc />
    public bool TryGetChar(int index, out char character)
    {
        if (index < 0 || index >= TotalLength)
        {
            character = '\0';
            return false;
        }

        if (index < Prefix.Length)
        {
            character = Prefix[index];
            return true;
        }

        index -= Prefix.Length;

        if (index < InnerLength)
        {
            return _innerText.TryGetChar(index, out character);
        }

        index -= InnerLength;

        character = Suffix[index];
        return true;
    }

    /// <summary>
    /// Applies italic style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with italic syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ItalicNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    public static ItalicNode<TInner> Apply(TInner innerStyle) => new(innerStyle);
}