using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.MarkdownV2.Formatting;

/// <summary>
/// Represents blockquote text in MarkdownV2 format.
/// Example: "&gt;blockquote text example\n&gt;new blockquote line"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with blockquote syntax.
/// </typeparam>
public struct BlockquoteNode<TInner> : INode
    where TInner : INode
{
    /// <summary>
    /// The prefix used for each line in a blockquote in MarkdownV2.
    /// </summary>
    public const char LinePrefix = '>';

    /// <summary>
    /// The character used to separate lines in the blockquote.
    /// </summary>
    public const char LineSeparator = '\n';

    private readonly TInner _innerText;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlockquoteNode{TInner}"/> struct.
    /// </summary>
    /// <param name="inner">
    /// The inner style that will be wrapped with blockquote syntax.
    /// </param>
    public BlockquoteNode(TInner inner)
    {
        _innerText = inner;
    }

    /// <summary>
    /// Returns the string representation of the blockquote style in MarkdownV2 format.
    /// Note: This method allocates a new string in the most efficient way possible.
    /// Use this method when you finished all styling operations and need the final string.
    /// </summary>
    /// <returns>The created string representation</returns>
    public override string ToString() => string.Create(TotalLength, this, static (span, style) => style.CopyTo(span));

    /// <summary>
    /// Gets the length of the inner text without the blockquote syntax.
    /// </summary>
    public int InnerLength => _innerText.TotalLength;

    /// <inheritdoc />
    /// Lazy evaluation of total length is needed to avoid unnecessary complex calculations
    public int SyntaxLength => _syntaxLength ??= CalculateSyntaxLength(_innerText);
    private int? _syntaxLength;

    /// <inheritdoc />
    public int TotalLength => SyntaxLength + InnerLength;

    /// <inheritdoc />
    public int CopyTo(Span<char> destination)
    {
        var writtenChars = 0;

        // the iterator is needed because the inner text is processed on the fly
        var iterator = GetCharacterIterator();

        while (iterator.MoveNext(out var character))
        {
            destination[writtenChars] = character;
            writtenChars++;
        }

        return writtenChars;
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

        character = '\0';
        return false;
    }

    /// <summary>
    /// Applies the blockquote style to the given inner style.
    /// </summary>
    /// <param name="innerStyle">
    /// The inner style to be wrapped with blockquote syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="BlockquoteNode{TInner}"/> containing the inner style.
    /// </returns>
    public static BlockquoteNode<TInner> Apply(TInner innerStyle) => new(innerStyle);

    /// <summary>
    /// Calculates the length of the blockquote syntax based on the number of lines in the inner text.
    /// Does this in the most efficient way possible. No heap allocations.
    /// </summary>
    /// <param name="innerText">
    /// The inner text whose lines will be counted to determine the syntax length.
    /// </param>
    /// <returns>
    /// The total length of the blockquote syntax, which is the number of lines.
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

        return lines;
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
        private readonly BlockquoteNode<TInner> _blockquoteNode;
        private int _currentVirtualIndex;
        private int _currentOriginalIndex;
        private bool _isAtLinePrefix;
        private bool _hasReachedEnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterIterator"/> struct.
        /// </summary>
        /// <param name="blockquoteNode">The blockquote style to iterate through.</param>
        public CharacterIterator(BlockquoteNode<TInner> blockquoteNode)
        {
            _blockquoteNode = blockquoteNode;
            _currentVirtualIndex = 0;
            _currentOriginalIndex = 0;
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
                _isAtLinePrefix = false;
                return true;
            }

            // Try to get the next character from the inner text
            if (_blockquoteNode._innerText.TryGetChar(_currentOriginalIndex, out character))
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

            // No more characters available
            _hasReachedEnd = true;
            character = '\0';
            return false;
        }
    }
}