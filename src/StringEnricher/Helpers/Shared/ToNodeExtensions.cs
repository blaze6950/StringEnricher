using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.Shared;

/// <summary>
/// Extension methods to convert primitive types to their corresponding Node representations.
/// </summary>
public static class ToNodeExtensions
{
    /// <summary>
    /// Converts a boolean value to a BoolNode.
    /// </summary>
    /// <param name="value">
    /// The boolean value to convert.
    /// </param>
    /// <returns>
    /// A BoolNode representing the boolean value.
    /// </returns>
    public static BoolNode ToNode(this bool value) => new(value);

    /// <summary>
    /// Converts an integer value to an IntegerNode.
    /// </summary>
    /// <param name="value">
    /// The integer value to convert.
    /// </param>
    /// <param name="format">
    /// An optional format string that defines how the integer should be formatted.
    /// </param>
    /// <param name="provider">
    /// An optional format provider that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// An IntegerNode representing the integer value.
    /// </returns>
    public static IntegerNode ToNode(this int value, string? format = null, IFormatProvider? provider = null) =>
        new(value, format, provider);

    /// <summary>
    /// Converts a string value to a PlainTextNode.
    /// </summary>
    /// <param name="value">
    /// The string value to convert.
    /// </param>
    /// <returns>
    /// A PlainTextNode representing the string value.
    /// </returns>
    public static PlainTextNode ToNode(this string value) => new(value);

    /// <summary>
    /// Converts a double value to a DoubleNode.
    /// </summary>
    /// <param name="value">
    /// The double value to convert.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string that defines the format of the double.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// A DoubleNode representing the double value.
    /// </returns>
    public static DoubleNode ToNode(this double value, string? format = null, IFormatProvider? provider = null) =>
        new(value, format, provider);

    /// <summary>
    /// Converts a float value to a FloatNode.
    /// </summary>
    /// <param name="value">
    /// The float value to convert.
    /// </param>
    /// <param name="format">
    /// A standard or custom numeric format string that defines the format of the float.
    /// </param>
    /// <param name="provider">
    /// An object that supplies culture-specific formatting information.
    /// </param>
    /// <returns>
    /// A FloatNode representing the float value.
    /// </returns>
    public static FloatNode ToNode(this float value, string? format = null, IFormatProvider? provider = null) =>
        new(value, format, provider);

    /// <summary>
    /// Converts a long value to a LongNode.
    /// </summary>
    /// <param name="value">
    /// The long value to convert.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the long to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the long to a string.
    /// </param>
    /// <returns>
    /// A LongNode representing the long value.
    /// </returns>
    public static LongNode ToNode(this long value, string? format = null, IFormatProvider? provider = null) =>
        new(value, format, provider);

    /// <summary>
    /// Converts a char value to a CharNode.
    /// </summary>
    /// <param name="value">
    /// The char value to convert.
    /// </param>
    /// <returns>
    /// A CharNode representing the char value.
    /// </returns>
    public static CharNode ToNode(this char value) => new(value);

    /// <summary>
    /// Converts a decimal value to a DecimalNode.
    /// </summary>
    /// <param name="value">
    /// The decimal value to convert.
    /// </param>
    /// <param name="format">
    /// The format string to use when converting the decimal to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the decimal to a string.
    /// </param>
    /// <returns>
    /// A DecimalNode representing the decimal value.
    /// </returns>
    public static DecimalNode ToNode(this decimal value, string? format = null, IFormatProvider? provider = null) =>
        new(value, format, provider);

    /// <summary>
    /// Converts a DateTime value to a DateTimeNode.
    /// </summary>
    /// <param name="value">
    /// The DateTime value to convert.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the DateTime to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTime to a string.
    /// </param>
    /// <returns>
    /// A DateTimeNode representing the DateTime value.
    /// </returns>
    public static DateTimeNode ToNode(this DateTime value, string? format = null, IFormatProvider? provider = null) =>
        new(value, format, provider);

    /// <summary>
    /// Converts a DateOnly value to a DateOnlyNode.
    /// </summary>
    /// <param name="value">
    /// The DateOnly value to convert.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the DateOnly to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateOnly to a string.
    /// </param>
    /// <returns>
    /// A DateOnlyNode representing the DateOnly value.
    /// </returns>
    public static DateOnlyNode ToNode(this DateOnly value, string? format = null, IFormatProvider? provider = null) =>
        new(value, format, provider);

    /// <summary>
    /// Converts a DateTimeOffset value to a DateTimeOffsetNode.
    /// </summary>
    /// <param name="value">
    /// The DateTimeOffset value to convert.
    /// </param>
    /// <param name="format">
    /// The format to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <param name="provider">
    /// The format provider to use when converting the DateTimeOffset to a string.
    /// </param>
    /// <returns>
    /// A DateTimeOffsetNode representing the DateTimeOffset value.
    /// </returns>
    public static DateTimeOffsetNode ToNode(this DateTimeOffset value, string? format = null,
        IFormatProvider? provider = null) => new(value, format, provider);

    /// <summary>
    /// Converts a TimeSpan value to a TimeSpanNode.
    /// </summary>
    /// <param name="value">
    /// The TimeSpan value to convert.
    /// </param>
    /// <param name="format">
    /// The format to use. If null, the default format is used.
    /// </param>
    /// <param name="provider">
    /// The format provider to use. If null, the current culture is used.
    /// </param>
    /// <returns>
    /// A TimeSpanNode representing the TimeSpan value.
    /// </returns>
    public static TimeSpanNode ToNode(this TimeSpan value, string? format = null, IFormatProvider? provider = null) =>
        new(value, format, provider);

    /// <summary>
    /// Converts a TimeOnly value to a TimeOnlyNode.
    /// </summary>
    /// <param name="value">
    /// The TimeOnly value to convert.
    /// </param>
    /// <param name="format">
    /// The format string.
    /// </param>
    /// <param name="provider">
    /// The format provider.
    /// </param>
    /// <returns>
    /// A TimeOnlyNode representing the TimeOnly value.
    /// </returns>
    public static TimeOnlyNode ToNode(this TimeOnly value, string? format = null, IFormatProvider? provider = null) =>
        new(value, format, provider);

    /// <summary>
    /// Converts a Guid value to a GuidNode.
    /// </summary>
    /// <param name="value">
    /// The Guid value to convert.
    /// </param>
    /// <param name="format">
    /// An optional format string for the GUID representation (e.g., "D", "N", "B", "P", "X").
    /// If null or empty, the default "D" format will be used.
    /// </param>
    /// <returns>
    /// A GuidNode representing the Guid value.
    /// </returns>
    public static GuidNode ToNode(this Guid value, string? format = null) => new(value, format);
}