namespace StringEnricher.Nodes;

/// <summary>
/// A helper for building messages with a known total length.
/// This struct uses <see cref="string.Create{TState}"/> internally to avoid unnecessary allocations.
/// It provides a <see cref="MessageWriter"/> to append text and nodes to the message.
/// The <see cref="MessageWriter"/> is a ref struct that allows appending text and nodes
/// directly to the provided span without additional allocations.
/// Example usage:
/// <code>
/// var builder = new MessageBuilder(totalLength);
/// var message = builder.Create(state, static (s, writer) =>
/// {
///     writer.Append("Hello, ");
///     writer.Append(s.UserName);
///     writer.Append("! Here is your styled message: ");
///     writer.Append(BoldMarkdownV2.Apply("This is bold text"));
/// });
/// </code>
/// </summary>
public readonly struct MessageBuilder
{
    private readonly int _totalLength;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageBuilder"/> struct with the specified total length.
    /// </summary>
    /// <param name="totalLength"></param>
    public MessageBuilder(int totalLength)
    {
        _totalLength = totalLength;
    }

    /// <summary>
    /// Creates a message by invoking the provided build action with the given state and a <see cref="MessageWriter"/>.
    /// The build action is responsible for appending text and nodes to the message using the <see cref="MessageWriter"/>.
    /// The resulting message will have the exact length specified during the initialization of the <see cref="MessageBuilder"/>.
    /// </summary>
    /// <param name="state">
    /// An arbitrary state object that can be used within the build action to customize the message content.
    /// </param>
    /// <param name="buildAction">
    /// An action that takes the state and a <see cref="MessageWriter"/> to build the message content.
    /// This action should append text and nodes to the <see cref="MessageWriter"/> to construct the final message.
    /// Example:
    /// <code>
    /// (s, writer) =>
    /// {
    ///     writer.Append("Hello, ");
    ///     writer.Append(s.UserName);
    ///     writer.Append("! Here is your styled message: ");
    ///     writer.Append(new BoldNode("This is bold text"));
    /// }
    /// </code>
    /// </param>
    /// <typeparam name="TState">
    /// The type of the state object passed to the build action.
    /// </typeparam>
    /// <returns>
    /// The constructed message as a string with the exact length specified during initialization.
    /// The returned string is the only object that was allocated on the heap during this process, except for any allocations made within the build action itself.
    /// </returns>
    public string Create<TState>(TState state, Action<TState, MessageWriter> buildAction) =>
        string.Create(_totalLength, state, (span, s) =>
        {
            var context = new MessageWriter(span);
            buildAction(s, context);
        });

    /// <summary>
    /// A writer for building messages with a known total length.
    /// </summary>
    public ref struct MessageWriter
    {
        private int _position = 0;
        private readonly Span<char> _destination;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageWriter"/> struct with the specified destination span.
        /// </summary>
        /// <param name="destination">
        /// The destination span where the message content will be written.
        /// This span should have a length equal to the total length specified during the initialization of the <see cref="MessageBuilder"/>.
        /// The <see cref="MessageWriter"/> will append text and nodes directly to this span.
        /// </param>
        internal MessageWriter(Span<char> destination)
        {
            _destination = destination;
        }

        /// <summary>
        /// Appends the specified text to the message.
        /// This method copies the text to the end.
        /// </summary>
        /// <param name="text">
        /// The text to append to the message.
        /// </param>
        public void Append(string text)
        {
            text.AsSpan().CopyTo(_destination[_position..]);
            _position += text.Length;
        }

        /// <summary>
        /// Appends a single character to the message.
        /// This method copies the character to the end.
        /// </summary>
        /// <param name="ch">
        /// The character to append to the message.
        /// </param>
        public void Append(char ch)
        {
            _destination[_position++] = ch;
        }

        /// <summary>
        /// Appends the specified span of characters to the message.
        /// This method copies the span to the end.
        /// </summary>
        /// <param name="span">
        /// The span of characters to append to the message.
        /// </param>
        public void Append(ReadOnlySpan<char> span)
        {
            span.CopyTo(_destination.Slice(_position, span.Length));
            _position += span.Length;
        }

        /// <summary>
        /// Appends the specified node to the message.
        /// This method copies the node's syntax to the end.
        /// </summary>
        /// <param name="node">
        /// The node to append to the message.
        /// </param>
        /// <typeparam name="TNode">
        /// The type of the node to append. Must implement <see cref="INode"/>.
        /// </typeparam>
        public void Append<TNode>(TNode node) where TNode : INode
        {
            _position += node.CopyTo(_destination[_position..]);
        }
    }
}