using System.Runtime.CompilerServices;
using StringEnricher.Discord.Nodes.Markdown.Formatting;
using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Discord.Helpers.Markdown;

/// <summary>
/// Provides methods to apply code block styling in Discord markdown format.
/// Example: "```\ncode block\n```"
/// </summary>
public static class CodeBlockMarkdown
{
    /// <summary>
    /// Applies the code block style to the given styled code block.
    /// </summary>
    /// <param name="codeBlock">
    /// The styled code block to be wrapped with code block syntax.
    /// </param>
    /// <typeparam name="T">
    /// The type of the style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> wrapping the provided style.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<T> Apply<T>(T codeBlock) where T : INode =>
        CodeBlockNode<T>.Apply(codeBlock);

    #region Overloads for Common Types

    /// <summary>
    /// Applies the code block style to the given plain text code block.
    /// </summary>
    /// <param name="codeBlock">
    /// The plain text code block to be wrapped with code block syntax.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> wrapping the provided plain text.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<PlainTextNode> Apply(string codeBlock) =>
        CodeBlockNode<PlainTextNode>.Apply(codeBlock);

    /// <summary>
    /// Applies code block style to the given boolean.
    /// </summary>
    /// <param name="boolean">
    /// The boolean to be styled as a code block.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled boolean.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<BoolNode> Apply(bool boolean) =>
        CodeBlockNode<BoolNode>.Apply(boolean);

    /// <summary>
    /// Applies code block style to the given character.
    /// </summary>
    /// <param name="character">
    /// The character to be styled as a code block.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled character.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<CharNode> Apply(char character) =>
        CodeBlockNode<CharNode>.Apply(character);

    /// <summary>
    /// Applies code block style to the given GUID.
    /// </summary>
    /// <param name="guid">
    /// The GUID to be styled as a code block.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled GUID.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<GuidNode> Apply(Guid guid) =>
        CodeBlockNode<GuidNode>.Apply(guid);

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies code block style to the given integer with custom formatting.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as a code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled integer.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<IntegerNode>
        Apply(int integer, string? format = null, IFormatProvider? provider = null) =>
        CodeBlockNode<IntegerNode>.Apply(new IntegerNode(integer, format, provider));

    /// <summary>
    /// Applies code block style to the given long integer with custom formatting.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as a code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled long integer.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<LongNode> Apply(long @long, string? format = null, IFormatProvider? provider = null) =>
        CodeBlockNode<LongNode>.Apply(new LongNode(@long, format, provider));

    /// <summary>
    /// Applies code block style to the given decimal with custom formatting.
    /// </summary>
    /// <param name="decimal">
    /// The decimal to be styled as a code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled decimal.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<DecimalNode> Apply(decimal @decimal, string? format = null,
        IFormatProvider? provider = null) =>
        CodeBlockNode<DecimalNode>.Apply(new DecimalNode(@decimal, format, provider));

    /// <summary>
    /// Applies code block style to the given double with custom formatting.
    /// </summary>
    /// <param name="double">
    /// The double to be styled as a code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled double.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<DoubleNode> Apply(double @double, string? format = null,
        IFormatProvider? provider = null) =>
        CodeBlockNode<DoubleNode>.Apply(new DoubleNode(@double, format, provider));

    /// <summary>
    /// Applies code block style to the given float with custom formatting.
    /// </summary>
    /// <param name="float">
    /// The float to be styled as a code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled float.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<FloatNode>
        Apply(float @float, string? format = null, IFormatProvider? provider = null) =>
        CodeBlockNode<FloatNode>.Apply(new FloatNode(@float, format, provider));

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies code block style to the given DateTime with custom formatting.
    /// </summary>
    /// <param name="dateTime">
    /// The DateTime to be styled as a code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled DateTime.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<DateTimeNode> Apply(DateTime dateTime, string? format = null,
        IFormatProvider? provider = null) =>
        CodeBlockNode<DateTimeNode>.Apply(new DateTimeNode(dateTime, format, provider));

    /// <summary>
    /// Applies code block style to the given DateTimeOffset with custom formatting.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The DateTimeOffset to be styled as a code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled DateTimeOffset.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<DateTimeOffsetNode> Apply(DateTimeOffset dateTimeOffset, string? format = null,
        IFormatProvider? provider = null) =>
        CodeBlockNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(dateTimeOffset, format, provider));

    /// <summary>
    /// Applies code block style to the given DateOnly with custom formatting.
    /// </summary>
    /// <param name="dateOnly">
    /// The DateOnly to be styled as a code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled DateOnly.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<DateOnlyNode> Apply(DateOnly dateOnly, string? format = null,
        IFormatProvider? provider = null) =>
        CodeBlockNode<DateOnlyNode>.Apply(new DateOnlyNode(dateOnly, format, provider));

    /// <summary>
    /// Applies code block style to the given TimeOnly with custom formatting.
    /// </summary>
    /// <param name="timeOnly">
    /// The TimeOnly to be styled as a code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled TimeOnly.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<TimeOnlyNode> Apply(TimeOnly timeOnly, string? format = null,
        IFormatProvider? provider = null) =>
        CodeBlockNode<TimeOnlyNode>.Apply(new TimeOnlyNode(timeOnly, format, provider));

    /// <summary>
    /// Applies code block style to the given TimeSpan with custom formatting.
    /// </summary>
    /// <param name="timeSpan">
    /// The TimeSpan to be styled as a code block.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CodeBlockNode{TInner}"/> containing the styled TimeSpan.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CodeBlockNode<TimeSpanNode> Apply(TimeSpan timeSpan, string? format = null,
        IFormatProvider? provider = null) =>
        CodeBlockNode<TimeSpanNode>.Apply(new TimeSpanNode(timeSpan, format, provider));

    #endregion
}