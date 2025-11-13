using StringEnricher.Nodes;

namespace StringEnricher.Discord.Nodes.Markdown.Formatting;

/// <summary>
/// Represents a list in Discord markdown format.
/// Example: "- first item\n- second item\n- third item"
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be wrapped with list syntax.
/// </typeparam>
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

    /// <summary>
    /// Gets the length of the inner text without the list syntax.
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
    /// Calculates the length of the list syntax based on the number of lines in the inner text.
    /// Does this in the most efficient way possible. No heap allocations.
    /// </summary>
    /// <param name="innerText">
    /// The inner text whose lines will be counted to determine the syntax length.
    /// </param>
    /// <returns>
    /// The total length of the list syntax, which is the number of lines multiplied by the prefix length.
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

        return lines * LinePrefix.Length;
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
        private readonly ListNode<TInner> _listNode;
        private int _currentOriginalIndex;
        private int _linePrefixPosition;
        private bool _hasReachedEnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterIterator"/> struct.
        /// </summary>
        /// <param name="listNode">The list style to iterate through.</param>
        public CharacterIterator(ListNode<TInner> listNode)
        {
            _listNode = listNode;
            _currentOriginalIndex = 0;
            _linePrefixPosition = 0; // Start at the beginning of the first line prefix
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

            // If we're in the middle of outputting a line prefix
            if (_linePrefixPosition < LinePrefix.Length)
            {
                character = LinePrefix[_linePrefixPosition];
                _linePrefixPosition++;
                return true;
            }

            // Try to get the next character from the inner text
            if (_listNode._innerText.TryGetChar(_currentOriginalIndex, out character))
            {
                _currentOriginalIndex++;

                // If this character is a line separator, the next characters should be a line prefix
                if (character == LineSeparator)
                {
                    _linePrefixPosition = 0; // Reset to start outputting a new line prefix after this
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