namespace StringEnricher.Buffer.Results;

/// <summary>
/// Represents a result containing a character and its total length.
/// </summary>
public record struct CharWithTotalWrittenCharsResult(bool Success, char Char, int CharsWritten);