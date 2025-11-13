using StringEnricher.Nodes;

namespace StringEnricher.Telegram.Nodes.Html;

/// <summary>
/// Escapes HTML reserved characters for the given text.
/// All &lt;, &gt; and &amp; symbols that are not a part of a tag or an HTML entity must be replaced with the corresponding HTML entities.
/// </summary>
/// <typeparam name="TInner">
/// The type of the inner style that will be escaped.
/// </typeparam>
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
        try
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
        catch (IndexOutOfRangeException e)
        {
            throw new ArgumentException("The destination span is too small to hold the escaped text.", e);
        }
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
    /// Calculates the length of the escape syntax for the HTML string.
    /// Does this in the most efficient way possible. No heap allocations.
    /// </summary>
    /// <param name="innerText">
    /// The inner text to determine the syntax length.
    /// </param>
    /// <returns>
    /// The total length of HTML escape syntax, which is the additional characters needed for HTML entities.
    /// </returns>
    private static int CalculateSyntaxLength(TInner innerText)
    {
        var additionalLength = 0;

        for (var i = 0; innerText.TryGetChar(i, out var character); i++)
        {
            var escapedEntity = character switch
            {
                LessThanChar => EscapedEntity,
                GreaterThanChar => Entity,
                AmpersandChar => Amp,
                _ => null
            };

            if (escapedEntity != null)
            {
                // Add the additional length (entity length - 1, since the original character is replaced)
                additionalLength += escapedEntity.Length - 1;
            }
        }

        return additionalLength;
    }

    /// <summary>
    /// A stateful iterator that maintains internal state for efficient sequential character access.
    /// This allows O(n) iteration through all characters instead of O(n²) in case of <see cref="TryGetChar"/>.
    /// </summary>
    private struct CharacterIterator
    {
        private readonly EscapeNode<TInner> _escapeNode;
        private int _currentOriginalIndex;
        private string? _currentEntity;
        private int _currentEntityIndex;
        private bool _hasReachedEnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterIterator"/> struct.
        /// </summary>
        /// <param name="escapeNode">The escape style to iterate through.</param>
        public CharacterIterator(EscapeNode<TInner> escapeNode)
        {
            _escapeNode = escapeNode;
            _currentOriginalIndex = 0;
            _currentEntity = null;
            _currentEntityIndex = 0;
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

            // If we're currently outputting an HTML entity
            if (_currentEntity != null)
            {
                character = _currentEntity[_currentEntityIndex];
                _currentEntityIndex++;

                // Check if we've finished the current entity
                if (_currentEntityIndex >= _currentEntity.Length)
                {
                    _currentEntity = null;
                    _currentEntityIndex = 0;
                    _currentOriginalIndex++;
                }

                return true;
            }

            // Try to get the next character from the inner text
            if (_escapeNode._innerText.TryGetChar(_currentOriginalIndex, out var originalChar))
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
                    // Start outputting the HTML entity
                    _currentEntity = escapedEntity;
                    _currentEntityIndex = 1; // We'll return the first character now
                    character = escapedEntity[0];
                    return true;
                }

                // Regular character, no escaping needed
                character = originalChar;
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