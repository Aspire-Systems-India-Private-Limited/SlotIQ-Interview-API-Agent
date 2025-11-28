namespace SlotIQ.Interview.Logic;

/// <summary>
/// Base interface for commands (write operations)
/// </summary>
/// <typeparam name="T">The return type of the command</typeparam>
public interface ICommand<T> where T : class
{
}
