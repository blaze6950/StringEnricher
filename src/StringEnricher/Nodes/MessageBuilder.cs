using System.Text;

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
        string.Create(_totalLength, ValueTuple.Create(state, buildAction), static (span, s) =>
        {
            var context = new MessageWriter(span);
            s.Item2(s.Item1, context);
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
        public MessageWriter Append(string text)
        {
            text.AsSpan().CopyTo(_destination[_position..]);
            _position += text.Length;
            return this;
        }

        /// <summary>
        /// Appends a single character to the message.
        /// This method copies the character to the end.
        /// </summary>
        /// <param name="ch">
        /// The character to append to the message.
        /// </param>
        public MessageWriter Append(char ch)
        {
            _destination[_position++] = ch;
            return this;
        }

        /// <summary>
        /// Appends the specified span of characters to the message.
        /// This method copies the span to the end.
        /// </summary>
        /// <param name="span">
        /// The span of characters to append to the message.
        /// </param>
        public MessageWriter Append(ReadOnlySpan<char> span)
        {
            span.CopyTo(_destination.Slice(_position, span.Length));
            _position += span.Length;
            return this;
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
        public MessageWriter Append<TNode>(TNode node) where TNode : struct, INode
        {
            _position += node.CopyTo(_destination[_position..]);
            return this;
        }

        /// <summary>
        /// Appends the string representation of the specified integer to the message.
        /// This method formats the integer and copies it to the end.
        /// </summary>
        /// <param name="number">
        /// The integer to append to the message.
        /// </param>
        /// <param name="format">
        /// An optional format string to customize the integer representation.
        /// If not provided, the default format will be used.
        /// </param>
        /// <param name="provider">
        /// An optional format provider to customize the integer representation.
        /// If not provided, the current culture's format provider will be used.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the integer could not be formatted into the destination span.
        /// </exception>
        public MessageWriter Append(int number, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            var written = number.TryFormat(_destination[_position..], out var charsWritten, format, provider);
            if (!written)
            {
                throw new InvalidOperationException("Failed to format the number.");
            }

            _position += charsWritten;
            return this;
        }

        /// <summary>
        /// Appends the string representation of the specified long integer to the message.
        /// This method formats the long integer and copies it to the end.
        /// </summary>
        /// <param name="number">
        /// The long integer to append to the message.
        /// </param>
        /// <param name="format">
        /// An optional format string to customize the long integer representation.
        /// If not provided, the default format will be used.
        /// </param>
        /// <param name="provider">
        /// An optional format provider to customize the long integer representation.
        /// If not provided, the current culture's format provider will be used.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the long integer could not be formatted into the destination span.
        /// </exception>
        public MessageWriter Append(long number, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            var written = number.TryFormat(_destination[_position..], out var charsWritten, format, provider);
            if (!written)
            {
                throw new InvalidOperationException("Failed to format the number.");
            }

            _position += charsWritten;
            return this;
        }

        /// <summary>
        /// Appends the string representation of the specified double to the message.
        /// This method formats the double and copies it to the end.
        /// </summary>
        /// <param name="number">
        /// The double to append to the message.
        /// </param>
        /// <param name="format">
        /// An optional format string to customize the double representation.
        /// If not provided, the default format will be used.
        /// </param>
        /// <param name="provider">
        /// An optional format provider to customize the double representation.
        /// If not provided, the current culture's format provider will be used.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the double could not be formatted into the destination span.
        /// </exception>
        public MessageWriter Append(double number, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            var written = number.TryFormat(_destination[_position..], out var charsWritten, format, provider);
            if (!written)
            {
                throw new InvalidOperationException("Failed to format the number.");
            }

            _position += charsWritten;
            return this;
        }

        /// <summary>
        /// Appends the string representation of the specified float to the message.
        /// This method formats the float and copies it to the end.
        /// </summary>
        /// <param name="number">
        /// The float to append to the message.
        /// </param>
        /// <param name="format">
        /// An optional format string to customize the float representation.
        /// If not provided, the default format will be used.
        /// </param>
        /// <param name="provider">
        /// An optional format provider to customize the float representation.
        /// If not provided, the current culture's format provider will be used.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the float could not be formatted into the destination span.
        /// </exception>
        public MessageWriter Append(float number, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            var written = number.TryFormat(_destination[_position..], out var charsWritten, format, provider);
            if (!written)
            {
                throw new InvalidOperationException("Failed to format the number.");
            }

            _position += charsWritten;
            return this;
        }

        /// <summary>
        /// Appends the string representation of the specified decimal to the message.
        /// This method formats the decimal and copies it to the end.
        /// </summary>
        /// <param name="number">
        /// The decimal to append to the message.
        /// </param>
        /// <param name="format">
        /// An optional format string to customize the decimal representation.
        /// If not provided, the default format will be used.
        /// </param>
        /// <param name="provider">
        /// An optional format provider to customize the decimal representation.
        /// If not provided, the current culture's format provider will be used.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the decimal could not be formatted into the destination span.
        /// </exception>
        public MessageWriter Append(decimal number, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            var written = number.TryFormat(_destination[_position..], out var charsWritten, format, provider);
            if (!written)
            {
                throw new InvalidOperationException("Failed to format the number.");
            }

            _position += charsWritten;
            return this;
        }

        /// <summary>
        /// Appends the string representation of the specified boolean to the message.
        /// This method formats the boolean and copies it to the end.
        /// </summary>
        /// <param name="value">
        /// The boolean value to append to the message.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the boolean could not be formatted into the destination span.
        /// </exception>
        public MessageWriter Append(bool value)
        {
            var written = value.TryFormat(_destination[_position..], out var charsWritten);
            if (!written)
            {
                throw new InvalidOperationException("Failed to format the boolean value.");
            }

            _position += charsWritten;
            return this;
        }

        /// <summary>
        /// Appends the string representation of the specified DateTime to the message.
        /// This method formats the DateTime and copies it to the end.
        /// </summary>
        /// <param name="dateTime">
        /// The DateTime to append to the message.
        /// </param>
        /// <param name="format">
        /// An optional format string to customize the DateTime representation.
        /// If not provided, the default format will be used.
        /// </param>
        /// <param name="provider">
        /// An optional format provider to customize the DateTime representation.
        /// If not provided, the current culture's format provider will be used.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the DateTime could not be formatted into the destination span.
        /// </exception>
        public MessageWriter Append(DateTime dateTime, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            var written = dateTime.TryFormat(_destination[_position..], out var charsWritten, format, provider);
            if (!written)
            {
                throw new InvalidOperationException("Failed to format the DateTime.");
            }

            _position += charsWritten;
            return this;
        }

        /// <summary>
        /// Appends the string representation of the specified DateTimeOffset to the message.
        /// This method formats the DateTimeOffset and copies it to the end.
        /// </summary>
        /// <param name="dateTimeOffset">
        /// The DateTimeOffset to append to the message.
        /// </param>
        /// <param name="format">
        /// An optional format string to customize the DateTimeOffset representation.
        /// If not provided, the default format will be used.
        /// </param>
        /// <param name="provider">
        /// An optional format provider to customize the DateTimeOffset representation.
        /// If not provided, the current culture's format provider will be used.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the DateTimeOffset could not be formatted into the destination span.
        /// </exception>
        public MessageWriter Append(DateTimeOffset dateTimeOffset, ReadOnlySpan<char> format = default,
            IFormatProvider? provider = null)
        {
            var written = dateTimeOffset.TryFormat(_destination[_position..], out var charsWritten, format, provider);
            if (!written)
            {
                throw new InvalidOperationException("Failed to format the DateTimeOffset.");
            }

            _position += charsWritten;
            return this;
        }

        /// <summary>
        /// Appends the string representation of the specified Guid to the message.
        /// This method formats the Guid and copies it to the end.
        /// </summary>
        /// <param name="guid">
        /// The Guid to append to the message.
        /// </param>
        /// <param name="format">
        /// An optional format string to customize the Guid representation.
        /// If not provided, the default format will be used.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the Guid could not be formatted into the destination span.
        /// </exception>
        public MessageWriter Append(Guid guid, ReadOnlySpan<char> format = default)
        {
            var written = guid.TryFormat(_destination[_position..], out var charsWritten, format);
            if (!written)
            {
                throw new InvalidOperationException("Failed to format the Guid.");
            }

            _position += charsWritten;
            return this;
        }

        /// <summary>
        /// Appends the string representation of the specified TimeSpan to the message.
        /// This method formats the TimeSpan and copies it to the end.
        /// </summary>
        /// <param name="timeSpan">
        /// The TimeSpan to append to the message.
        /// </param>
        /// <param name="format">
        /// An optional format string to customize the TimeSpan representation.
        /// If not provided, the default format will be used.
        /// </param>
        /// <param name="provider">
        /// An optional format provider to customize the TimeSpan representation.
        /// If not provided, the current culture's format provider will be used.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the TimeSpan could not be formatted into the destination span.
        /// </exception>
        public MessageWriter Append(TimeSpan timeSpan, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            var written = timeSpan.TryFormat(_destination[_position..], out var charsWritten, format, provider);
            if (!written)
            {
                throw new InvalidOperationException("Failed to format the TimeSpan.");
            }

            _position += charsWritten;
            return this;
        }

        /// <summary>
        /// Appends the string representation of the specified DateOnly to the message.
        /// This method formats the DateOnly and copies it to the end.
        /// </summary>
        /// <param name="dateOnly">
        /// The DateOnly to append to the message.
        /// </param>
        /// <param name="format">
        /// An optional format string to customize the DateOnly representation.
        /// If not provided, the default format will be used.
        /// </param>
        /// <param name="provider">
        /// An optional format provider to customize the DateOnly representation.
        /// If not provided, the current culture's format provider will be used.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the DateOnly could not be formatted into the destination span.
        /// </exception>
        public MessageWriter Append(DateOnly dateOnly, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            var written = dateOnly.TryFormat(_destination[_position..], out var charsWritten, format, provider);
            if (!written)
            {
                throw new InvalidOperationException("Failed to format the DateOnly.");
            }

            _position += charsWritten;
            return this;
        }

        /// <summary>
        /// Appends the string representation of the specified TimeOnly to the message.
        /// This method formats the TimeOnly and copies it to the end.
        /// </summary>
        /// <param name="timeOnly">
        /// The TimeOnly to append to the message.
        /// </param>
        /// <param name="format">
        /// An optional format string to customize the TimeOnly representation.
        /// If not provided, the default format will be used.
        /// </param>
        /// <param name="provider">
        /// An optional format provider to customize the TimeOnly representation.
        /// If not provided, the current culture's format provider will be used.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the TimeOnly could not be formatted into the destination span.
        /// </exception>
        public MessageWriter Append(TimeOnly timeOnly, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            var written = timeOnly.TryFormat(_destination[_position..], out var charsWritten, format, provider);
            if (!written)
            {
                throw new InvalidOperationException("Failed to format the TimeOnly.");
            }

            _position += charsWritten;
            return this;
        }

        /// <summary>
        /// Appends the content of the specified StringBuilder to the message.
        /// This method copies the content of the StringBuilder to the end.
        /// </summary>
        /// <param name="stringBuilder">
        /// The StringBuilder whose content will be appended to the message.
        /// </param>
        public MessageWriter Append(StringBuilder stringBuilder)
        {
            stringBuilder.CopyTo(0, _destination.Slice(_position, stringBuilder.Length), stringBuilder.Length);
            _position += stringBuilder.Length;
            return this;
        }
    }
}