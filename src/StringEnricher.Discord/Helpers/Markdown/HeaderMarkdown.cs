using System.Runtime.CompilerServices;
using StringEnricher.Discord.Nodes.Markdown.Formatting;
using StringEnricher.Nodes;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Discord.Helpers.Markdown;

/// <summary>
/// Provides methods to apply header styling in Discord markdown format.
/// Example: "# Header", "## Subheader", "### Smaller header"
/// </summary>
public static class HeaderMarkdown
{
    /// <summary>
    /// Applies header style to the given style.
    /// </summary>
    /// <param name="style">
    /// The inner style to be wrapped with header Discord markdown syntax.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that implements <see cref="INode"/>.
    /// </typeparam>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> wrapping the provided inner style.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<T> Apply<T>(T style, int level = 1) where T : INode =>
        HeaderNode<T>.Apply(style, level);

    #region Overloads for Common Types

    /// <summary>
    /// Applies header style to the given text.
    /// </summary>
    /// <param name="text">
    /// The text to be wrapped with header Discord markdown syntax.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> wrapping the provided text.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<PlainTextNode> Apply(string text, int level = 1) =>
        HeaderNode<PlainTextNode>.Apply(new PlainTextNode(text), level);

    /// <summary>
    /// Applies header style to the given boolean.
    /// </summary>
    /// <param name="boolean">
    /// The boolean to be styled as header.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the styled boolean.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<BoolNode> Apply(bool boolean, int level = 1) =>
        HeaderNode<BoolNode>.Apply(new BoolNode(boolean), level);

    /// <summary>
    /// Applies header style to the given character.
    /// </summary>
    /// <param name="character">
    /// The character to be styled as header.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the styled character.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<CharNode> Apply(char character, int level = 1) =>
        HeaderNode<CharNode>.Apply(new CharNode(character), level);

    /// <summary>
    /// Applies header style to the given GUID.
    /// </summary>
    /// <param name="guid">
    /// The GUID to be styled as header.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the styled GUID.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<GuidNode> Apply(Guid guid, int level = 1) =>
        HeaderNode<GuidNode>.Apply(new GuidNode(guid), level);

    #endregion

    #region Overloads for Numeric Types with Formatting Support

    /// <summary>
    /// Applies header style to the given integer with custom formatting.
    /// </summary>
    /// <param name="integer">
    /// The integer to be styled as header.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the integer to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the integer to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the styled integer.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<IntegerNode> Apply(int integer, int level = 1, string? format = null,
        IFormatProvider? provider = null) =>
        HeaderNode<IntegerNode>.Apply(new IntegerNode(integer, format, provider), level);

    /// <summary>
    /// Applies header style to the given long integer with custom formatting.
    /// </summary>
    /// <param name="long">
    /// The long integer to be styled as header.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the styled long integer.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<LongNode> Apply(long @long, int level = 1, string? format = null,
        IFormatProvider? provider = null) =>
        HeaderNode<LongNode>.Apply(new LongNode(@long, format, provider), level);

    /// <summary>
    /// Applies header style to the given decimal with custom formatting.
    /// </summary>
    /// <param name="decimal">
    /// The decimal to be styled as header.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the styled decimal.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<DecimalNode> Apply(decimal @decimal, int level = 1, string? format = null,
        IFormatProvider? provider = null) =>
        HeaderNode<DecimalNode>.Apply(new DecimalNode(@decimal, format, provider), level);

    /// <summary>
    /// Applies header style to the given double with custom formatting.
    /// </summary>
    /// <param name="double">
    /// The double to be styled as header.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the double to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the double to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the styled double.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<DoubleNode> Apply(double @double, int level = 1, string? format = null,
        IFormatProvider? provider = null) =>
        HeaderNode<DoubleNode>.Apply(new DoubleNode(@double, format, provider), level);

    /// <summary>
    /// Applies header style to the given float with custom formatting.
    /// </summary>
    /// <param name="float">
    /// The float to be styled as header.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the float to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the float to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the styled float.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<FloatNode> Apply(float @float, int level = 1, string? format = null,
        IFormatProvider? provider = null) =>
        HeaderNode<FloatNode>.Apply(new FloatNode(@float, format, provider), level);

    #endregion

    #region Overloads for Date/Time Types with Formatting Support

    /// <summary>
    /// Applies header style to the given DateTime with custom formatting.
    /// </summary>
    /// <param name="dateTime">
    /// The DateTime to be styled as header.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the styled DateTime.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<DateTimeNode> Apply(DateTime dateTime, int level = 1, string? format = null,
        IFormatProvider? provider = null) =>
        HeaderNode<DateTimeNode>.Apply(new DateTimeNode(dateTime, format, provider), level);

    /// <summary>
    /// Applies header style to the given DateTimeOffset with custom formatting.
    /// </summary>
    /// <param name="dateTimeOffset">
    /// The DateTimeOffset to be styled as header.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the styled DateTimeOffset.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<DateTimeOffsetNode> Apply(DateTimeOffset dateTimeOffset, int level = 1,
        string? format = null, IFormatProvider? provider = null) =>
        HeaderNode<DateTimeOffsetNode>.Apply(new DateTimeOffsetNode(dateTimeOffset, format, provider), level);

    /// <summary>
    /// Applies header style to the given DateOnly with custom formatting.
    /// </summary>
    /// <param name="dateOnly">
    /// The DateOnly to be styled as header.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the styled DateOnly.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<DateOnlyNode> Apply(DateOnly dateOnly, int level = 1, string? format = null,
        IFormatProvider? provider = null) =>
        HeaderNode<DateOnlyNode>.Apply(new DateOnlyNode(dateOnly, format, provider), level);

    /// <summary>
    /// Applies header style to the given TimeOnly with custom formatting.
    /// </summary>
    /// <param name="timeOnly">
    /// The TimeOnly to be styled as header.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeOnly to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the styled TimeOnly.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<TimeOnlyNode> Apply(TimeOnly timeOnly, int level = 1, string? format = null,
        IFormatProvider? provider = null) =>
        HeaderNode<TimeOnlyNode>.Apply(new TimeOnlyNode(timeOnly, format, provider), level);

    /// <summary>
    /// Applies header style to the given TimeSpan with custom formatting.
    /// </summary>
    /// <param name="timeSpan">
    /// The TimeSpan to be styled as header.
    /// </param>
    /// <param name="level">
    /// The header level (1-3, where 1 is #, 2 is ##, 3 is ###). Default is 1.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the TimeSpan to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the TimeSpan to a string.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="HeaderNode{TInner}"/> containing the styled TimeSpan.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HeaderNode<TimeSpanNode> Apply(TimeSpan timeSpan, int level = 1, string? format = null,
        IFormatProvider? provider = null) =>
        HeaderNode<TimeSpanNode>.Apply(new TimeSpanNode(timeSpan, format, provider), level);

    #endregion
}