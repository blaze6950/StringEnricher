namespace StringEnricher.Buffer.States;

/// <summary>
/// A marker interface for buffer states.
/// </summary>
public interface IState<out T>
{
    /// <summary>
    /// The value associated with this state.
    /// </summary>
    public T Value { get; }
}