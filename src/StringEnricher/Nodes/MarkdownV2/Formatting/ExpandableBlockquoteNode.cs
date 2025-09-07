namespace StringEnricher.Nodes.MarkdownV2.Formatting;

/// <summary>
/// Provides methods to apply expandable blockquote style in MarkdownV2 format.
/// Example: "&gt;blockquote text example\n&gt;new expandable blockquote line||"
/// </summary>
public static class ExpandableBlockquoteMarkdownV2
{
    /// <summary>
    /// Applies expandable blockquote style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be styled as an expandable blockquote.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="ExpandableBlockquoteNode{TInner}"/> containing the styled text.
    /// </returns>
    public static ExpandableBlockquoteNode<PlainTextNode> Apply(string text) =>
        ExpandableBlockquoteNode<PlainTextNode>.Apply(text);

    /// <summary>
    /// Applies expandable blockquote style to the given style.
    /// </summary>
    /// <param name="style">
    /// The style to be wrapped with expandable blockquote syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="ExpandableBlockquoteNode{TInner}"/> containing the styled text.
    /// </returns>
    public static ExpandableBlockquoteNode<T> Apply<T>(T style) where T : INode =>
        ExpandableBlockquoteNode<T>.Apply(style);
}

/// <summary>
/// Represents expandable blockquote text in MarkdownV2 format.
/// Example: "&gt;blockquote text example\n&gt;new expandable blockquote line||"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with expandable blockquote syntax.
/// </typeparam>
public readonly struct ExpandableBlockquoteNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix used for each line in a expandable blockquote in MarkdownV2.
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
        SyntaxLength = CalculateSyntaxLength(inner);
    }

    /// <summary>
    /// Returns the string representation of the expandable blockquote style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text without the expandable blockquote syntax.
    /// </summary>
    public int InnerLength => _innerText.TotalLength;

    /// <inheritdoc />
    public int SyntaxLength { get; }

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

        var iterator = GetCharacterIterator();
        var pos = 0;

        while (iterator.MoveNext(out var character))
        {
            destination[pos] = character;
            pos++;
        }

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
    /// Calculates the length of the expandable blockquote syntax based on the number of lines in the inner text.
    /// Does this in the most efficient way possible. No heap allocations.
    /// </summary>
    /// <param name="innerText">
    /// The inner text whose lines will be counted to determine the syntax length.
    /// </param>
    /// <returns>
    /// The total length of the expandable blockquote syntax, which is the number of lines.
    /// </returns>
    private static int CalculateSyntaxLength(TInner innerText)
    {
        var lines = 1;

        for (var i = 0; innerText.TryGetChar(i, out var ch); i++)
        {
            if (ch == LineSeparator)
            {
                lines++;
            }
        }

        return lines + Suffix.Length;
    }

    /// <summary>
    /// Creates a new character iterator for efficient sequential access to all characters.
    /// Use this when you need to iterate through all characters sequentially for O(n) performance.
    /// </summary>
    /// <returns>A new <see cref="CharacterIterator"/> instance.</returns>
    private CharacterIterator GetCharacterIterator() => new(this);

    /// <summary>
    /// A stateful iterator that maintains internal state for efficient sequential character access.
    /// This allows O(n) iteration through all characters instead of O(n²) in case of <see cref="TryGetChar"/>.
    /// </summary>
    private struct CharacterIterator
    {
        private readonly ExpandableBlockquoteNode<TInner> _expandableBlockquoteNode;
        private int _currentVirtualIndex;
        private int _currentOriginalIndex;
        private int _addedNewLinePrefixes;
        private bool _isAtLinePrefix;
        private bool _hasReachedEnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterIterator"/> struct.
        /// </summary>
        /// <param name="ExpandableBlockquoteNode">The expandable blockquote style to iterate through.</param>
        public CharacterIterator(ExpandableBlockquoteNode<TInner> ExpandableBlockquoteNode)
        {
            _expandableBlockquoteNode = ExpandableBlockquoteNode;
            _currentVirtualIndex = 0;
            _currentOriginalIndex = 0;
            _addedNewLinePrefixes = 1; // Start with 1 to account for the first line prefix
            _isAtLinePrefix = true; // Start with the first line prefix
            _hasReachedEnd = false;
        }

        /// <summary>
        /// Moves to the next character and returns it.
        /// Returns true if a character was successfully retrieved, false if the end was reached.
        /// </summary>
        /// <param name="character">When this method returns, contains the character at the current position.</param>
        /// <returns>true if a character was successfully retrieved; otherwise, false.</returns>
        public bool MoveNext(out char character)
        {
            if (_hasReachedEnd)
            {
                character = '\0';
                return false;
            }

            // Handle the first line prefix
            if (_currentVirtualIndex == 0)
            {
                character = LinePrefix;
                _currentVirtualIndex++;
                _isAtLinePrefix = false;
                return true;
            }

            // If we're currently at a line prefix position after a line separator
            if (_isAtLinePrefix)
            {
                character = LinePrefix;
                _currentVirtualIndex++;
                _addedNewLinePrefixes++;
                _isAtLinePrefix = false;
                return true;
            }

            // Try to get the next character from the inner text
            if (_expandableBlockquoteNode._innerText.TryGetChar(_currentOriginalIndex, out character))
            {
                _currentOriginalIndex++;
                _currentVirtualIndex++;

                // If this character is a line separator, the next character should be a line prefix
                if (character == LineSeparator)
                {
                    _isAtLinePrefix = true;
                }

                return true;
            }

            // Now handle the suffix "||"
            var suffixIndex = _currentVirtualIndex - _addedNewLinePrefixes - _currentOriginalIndex;

            if (suffixIndex >= 0 && suffixIndex < Suffix.Length)
            {
                character = Suffix[suffixIndex];
                _currentVirtualIndex++;
                return true;
            }

            // No more characters available
            _hasReachedEnd = true;
            character = '\0';
            return false;
        }
    }
}