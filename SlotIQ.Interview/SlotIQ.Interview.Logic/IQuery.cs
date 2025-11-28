namespace SlotIQ.Interview.Logic;

/// <summary>
/// Base interface for queries (read operations)
/// </summary>
/// <typeparam name="T">The return type of the query</typeparam>
public interface IQuery<T> where T : class
{
}
