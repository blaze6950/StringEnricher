using StringEnricher.Nodes;
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
    /// Creates a new instance of the <see cref="TgEmojiNode{TInner}"/> struct.
    /// </summary>
    /// <param name="defaultEmoji">
    /// The default emoji to display if the custom emoji is not available.
    /// </param>
    /// <param name="customEmojiId">
    /// The unique identifier of the custom emoji.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="TgEmojiNode{TInner}"/> struct.
    /// </returns>
    public static TgEmojiNode<PlainTextNode> Apply(string defaultEmoji, string customEmojiId) =>
        TgEmojiNode<PlainTextNode>.Apply(defaultEmoji, customEmojiId);

    /// <summary>
    /// Creates a new instance of the <see cref="TgEmojiNode{TInner}"/> struct.
    /// </summary>
    /// <param name="defaultEmoji">
    /// The default emoji to display if the custom emoji is not available.
    /// </param>
    /// <param name="customEmojiId">
    /// The unique identifier of the custom emoji.
    /// </param>
    /// <typeparam name="T">
    /// The type of the inner style that will be wrapped with TG emoji syntax.
    /// </typeparam>
    /// <returns>
    /// The created instance of the <see cref="TgEmojiNode{TInner}"/> struct.
    /// </returns>
    public static TgEmojiNode<T> Apply<T>(T defaultEmoji, T customEmojiId) where T : INode =>
        TgEmojiNode<T>.Apply(defaultEmoji, customEmojiId);

    /// <summary>
    /// Creates a new instance of the <see cref="TgEmojiNode{TInner}"/> struct with integer emoji ID.
    /// </summary>
    /// <param name="defaultEmoji">
    /// The default emoji to display if the custom emoji is not available.
    /// </param>
    /// <param name="customEmojiId">
    /// The unique identifier of the custom emoji as integer.
    /// </param>
    /// <returns>
    /// The created instance of the <see cref="TgEmojiNode{TInner}"/> struct.
    /// </returns>
    public static TgEmojiNode<PlainTextNode> Apply(string defaultEmoji, int customEmojiId) =>
        TgEmojiNode<PlainTextNode>.Apply(defaultEmoji, customEmojiId.ToString());
}