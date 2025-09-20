using System.Text;
using StringEnricher.Helpers.Shared;
using StringEnricher.Nodes;

namespace StringEnricher.Builders;

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
        /// Appends the content of the specified StringBuilder to the message.
        /// This method copies the content of the StringBuilder to the end.
        /// </summary>
        /// <param name="stringBuilder">
        /// The StringBuilder whose content will be appended to the message.
        /// </param>
        public void Append(StringBuilder stringBuilder)
        {
            stringBuilder.CopyTo(0, _destination.Slice(_position, stringBuilder.Length), stringBuilder.Length);
            _position += stringBuilder.Length;
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
        public void Append<TNode>(TNode node) where TNode : struct, INode
        {
            _position += node.CopyTo(_destination[_position..]);
        }

        /// <summary>
        /// Appends the specified text to the message.
        /// This method copies the text to the end.
        /// </summary>
        /// <param name="value">
        /// The text to append to the message.
        /// </param>
        public void Append(string value) => Append(value.ToNode());

        /// <summary>
        /// Appends a single character to the message.
        /// This method copies the character to the end.
        /// </summary>
        /// <param name="value">
        /// The character to append to the message.
        /// </param>
        public void Append(char value) => Append(value.ToNode());

        /// <summary>
        /// Appends the string representation of the specified integer to the message.
        /// This method formats the integer and copies it to the end.
        /// </summary>
        /// <param name="value">
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
        public void Append(int value, string? format = null, IFormatProvider? provider = null) =>
            Append(value.ToNode(format, provider));

        /// <summary>
        /// Appends the string representation of the specified long integer to the message.
        /// This method formats the long integer and copies it to the end.
        /// </summary>
        /// <param name="value">
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
        public void Append(long value, string? format = null, IFormatProvider? provider = null) =>
            Append(value.ToNode(format, provider));

        /// <summary>
        /// Appends the string representation of the specified double to the message.
        /// This method formats the double and copies it to the end.
        /// </summary>
        /// <param name="value">
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
        public void Append(double value, string? format = null, IFormatProvider? provider = null) =>
            Append(value.ToNode(format, provider));

        /// <summary>
        /// Appends the string representation of the specified float to the message.
        /// This method formats the float and copies it to the end.
        /// </summary>
        /// <param name="value">
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
        public void Append(float value, string? format = null, IFormatProvider? provider = null) =>
            Append(value.ToNode(format, provider));

        /// <summary>
        /// Appends the string representation of the specified decimal to the message.
        /// This method formats the decimal and copies it to the end.
        /// </summary>
        /// <param name="value">
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
        public void Append(decimal value, string? format = null, IFormatProvider? provider = null) =>
            Append(value.ToNode(format, provider));

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
        public void Append(bool value) => Append(value.ToNode());

        /// <summary>
        /// Appends the string representation of the specified DateTime to the message.
        /// This method formats the DateTime and copies it to the end.
        /// </summary>
        /// <param name="value">
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
        public void Append(DateTime value, string? format = null, IFormatProvider? provider = null) =>
            Append(value.ToNode(format, provider));

        /// <summary>
        /// Appends the string representation of the specified DateTimeOffset to the message.
        /// This method formats the DateTimeOffset and copies it to the end.
        /// </summary>
        /// <param name="value">
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
        public void Append(DateTimeOffset value, string? format = null, IFormatProvider? provider = null) =>
            Append(value.ToNode(format, provider));

        /// <summary>
        /// Appends the string representation of the specified Guid to the message.
        /// This method formats the Guid and copies it to the end.
        /// </summary>
        /// <param name="value">
        /// The Guid to append to the message.
        /// </param>
        /// <param name="format">
        /// An optional format string to customize the Guid representation.
        /// If not provided, the default format will be used.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the Guid could not be formatted into the destination span.
        /// </exception>
        public void Append(Guid value, string? format = null) => Append(value.ToNode(format));

        /// <summary>
        /// Appends the string representation of the specified TimeSpan to the message.
        /// This method formats the TimeSpan and copies it to the end.
        /// </summary>
        /// <param name="value">
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
        public void Append(TimeSpan value, string? format = null, IFormatProvider? provider = null) =>
            Append(value.ToNode(format, provider));

        /// <summary>
        /// Appends the string representation of the specified DateOnly to the message.
        /// This method formats the DateOnly and copies it to the end.
        /// </summary>
        /// <param name="value">
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
        public void Append(DateOnly value, string? format = null, IFormatProvider? provider = null) =>
            Append(value.ToNode(format, provider));

        /// <summary>
        /// Appends the string representation of the specified TimeOnly to the message.
        /// This method formats the TimeOnly and copies it to the end.
        /// </summary>
        /// <param name="value">
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
        public void Append(TimeOnly value, string? format = null, IFormatProvider? provider = null) =>
            Append(value.ToNode(format, provider));

        /// <summary>
        /// Appends a sequence of strings to the message, separated by the specified separator.
        /// This method copies each string and the separator to the end.
        /// If the separator is empty, the strings are appended without any separation.
        /// If the values collection is null or empty, no action is taken.
        /// </summary>
        /// <param name="values">
        /// The collection of strings to append to the message.
        /// If null or empty, no action is taken.
        /// </param>
        /// <param name="separator">
        /// An optional separator string to insert between each value.
        /// If null or empty, the values are appended without any separation.
        /// Default is null.
        /// </param>
        public void AppendJoin<TCollection>(TCollection values, string? separator = null)
            where TCollection : IReadOnlyList<string> => Append(values.ToNode(separator));

        /// <summary>
        /// Appends the string representation of the specified enum value to the message.
        /// This method formats the enum value and copies it to the end.
        /// </summary>
        /// <param name="value">
        /// The enum value to append to the message.
        /// </param>
        /// <param name="format">
        /// An optional format string to customize the enum representation.
        /// If not provided, the default format will be used.
        /// </param>
        /// <typeparam name="TEnum">
        /// The type of the enum to append. Must be a struct and an Enum.
        /// </typeparam>
        public void Append<TEnum>(TEnum value, string? format = null)
            where TEnum : struct, Enum => Append(value.ToNode(format));
    }
}