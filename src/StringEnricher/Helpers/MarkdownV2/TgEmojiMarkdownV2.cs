using StringEnricher.Nodes.MarkdownV2.Formatting;
using StringEnricher.Nodes.Shared;

namespace StringEnricher.Helpers.MarkdownV2;

/// <summary>
/// Provides methods to create TG emoji styles in MarkdownV2 format.
/// Example: "![👍](tg://emoji?id=123456)"
/// </summary>
public static class TgEmojiMarkdownV2
{
    /// <summary>
    /// Creates a new instance of the <see cref="TgEmojiNode"/> struct.
    /// </summary>
    /// <param name="defaultEmoji">
    /// The default emoji to display if the custom emoji is not available.
    /// </param>
    /// <param name="customEmojiId">
    /// The unique identifier of the custom emoji.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="TgEmojiNode"/> struct.
    /// </returns>
    public static TgEmojiNode Apply(PlainTextNode defaultEmoji, LongNode customEmojiId) =>
        TgEmojiNode.Apply(defaultEmoji, customEmojiId);

    /// <summary>
    /// Creates a new instance of the <see cref="TgEmojiNode"/> struct.
    /// </summary>
    /// <param name="defaultEmoji">
    /// The default emoji to display if the custom emoji is not available.
    /// </param>
    /// <param name="customEmojiId">
    /// The unique identifier of the custom emoji.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="TgEmojiNode"/> struct.
    /// </returns>
    public static TgEmojiNode Apply(string defaultEmoji, long customEmojiId) =>
        TgEmojiNode.Apply(defaultEmoji, customEmojiId);

    /// <summary>
    /// Creates a new instance of the <see cref="TgEmojiNode"/> struct.
    /// </summary>
    /// <param name="defaultEmoji">
    /// The default emoji to display if the custom emoji is not available.
    /// </param>
    /// <param name="customEmojiId">
    /// The unique identifier of the custom emoji.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="TgEmojiNode"/> struct.
    /// </returns>
    public static TgEmojiNode Apply(PlainTextNode defaultEmoji, long customEmojiId) =>
        TgEmojiNode.Apply(defaultEmoji, customEmojiId);

    /// <summary>
    /// Creates a new instance of the <see cref="TgEmojiNode"/> struct.
    /// </summary>
    /// <param name="defaultEmoji">
    /// The default emoji to display if the custom emoji is not available.
    /// </param>
    /// <param name="customEmojiId">
    /// The unique identifier of the custom emoji.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="TgEmojiNode"/> struct.
    /// </returns>
    public static TgEmojiNode Apply(string defaultEmoji, LongNode customEmojiId) =>
        TgEmojiNode.Apply(defaultEmoji, customEmojiId);
}