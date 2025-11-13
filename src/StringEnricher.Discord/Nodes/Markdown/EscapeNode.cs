using StringEnricher.Nodes;

namespace StringEnricher.Discord.Nodes.Markdown;

/// <summary>
/// Escapes Discord markdown reserved characters for the given text.
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be escaped.
/// </typeparam>
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

    /// <summary>
    /// Gets the length of the inner text.
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
        var iterator = new CharacterIterator(this);

        while (iterator.MoveNext(out var character))
        {
            destination[writtenChars++] = character;
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

        var neededIndex = index;
        var virtualIndex = 0;
        var originalIndex = 0;
        while (_innerText.TryGetChar(originalIndex, out character))
        {
            var isCharToEscape = false;

            switch (character)
            {
                case '_':
                case '*':
                case '~':
                case '`':
                case '#':
                case '-':
                case '[':
                case ']':
                case '(':
                case ')':
                case '>':
                case '|':
                case '\\':
                    isCharToEscape = true;
                    break;
            }

            if (isCharToEscape)
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
    /// Calculates the length of the escape syntax for the Discord markdown string.
    /// Does this in the most efficient way possible. No heap allocations.
    /// </summary>
    /// <param name="innerText">
    /// The inner text to determine the syntax length.
    /// </param>
    /// <returns>
    /// The total length of Discord markdown escape syntax, which is the number of special characters that should be fronted by '\'.
    /// </returns>
    private static int CalculateSyntaxLength(TInner innerText)
    {
        var charactersToEscape = 0;

        for (var i = 0; innerText.TryGetChar(i, out var character); i++)
        {
            switch (character)
            {
                case '_':
                case '*':
                case '~':
                case '`':
                case '#':
                case '-':
                case '[':
                case ']':
                case '(':
                case ')':
                case '>':
                case '|':
                case '\\':
                    charactersToEscape++;
                    break;
            }
        }

        // Each special character needs to be escaped with a backslash '\', so the syntax length is equal to the number of special characters.
        return charactersToEscape;
    }

    /// <summary>
    /// A stateful iterator that maintains internal state for efficient sequential character access.
    /// This allows O(n) iteration through all characters instead of O(n²) in case of <see cref="TryGetChar"/>.
    /// </summary>
    private struct CharacterIterator
    {
        private readonly EscapeNode<TInner> _escapeNode;
        private int _currentVirtualIndex;
        private int _currentOriginalIndex;
        private bool _isAtCharToEscape;
        private bool _hasReachedEnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterIterator"/> struct.
        /// </summary>
        /// <param name="escapeNode">The escape style to iterate through.</param>
        public CharacterIterator(EscapeNode<TInner> escapeNode)
        {
            _escapeNode = escapeNode;
            _currentVirtualIndex = 0;
            _currentOriginalIndex = 0;
            _isAtCharToEscape = false;
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

            // If we're currently at a character to escape position after an escape symbol
            if (_isAtCharToEscape)
            {
                _escapeNode._innerText.TryGetChar(_currentOriginalIndex,
                    out character); // set character to the actual character to be escaped on a previous call
                _currentVirtualIndex++;
                _currentOriginalIndex++;
                _isAtCharToEscape = false;
                return true;
            }

            // Try to get the next character from the inner text
            if (_escapeNode._innerText.TryGetChar(_currentOriginalIndex, out character))
            {
                _currentVirtualIndex++;

                switch (character)
                {
                    case '_':
                    case '*':
                    case '~':
                    case '`':
                    case '#':
                    case '-':
                    case '[':
                    case ']':
                    case '(':
                    case ')':
                    case '>':
                    case '|':
                    case '\\':
                        character = EscapeSymbol;
                        _isAtCharToEscape = true;
                        return true;
                }

                _currentOriginalIndex++;

                return true;
            }

            // No more characters available
            _hasReachedEnd = true;
            character = '\0';
            return false;
        }
    }
}