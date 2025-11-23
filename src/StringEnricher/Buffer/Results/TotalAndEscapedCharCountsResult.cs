namespace StringEnricher.Buffer.Results;

/// <summary>
/// Represents the result of counting total and escaped characters.
/// </summary>
public record struct TotalAndEscapedCharCountsResult(int TotalCount, int EscapedCount);