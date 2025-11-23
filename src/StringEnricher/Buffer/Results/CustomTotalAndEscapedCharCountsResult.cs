namespace StringEnricher.Buffer.Results;

/// <summary>
/// Represents the result of counting total and escaped characters.
/// </summary>
public record struct CustomTotalAndEscapedCharCountsResult(int TotalCount, int EscapedCount, int ToEscapeCount);