namespace SlotIQ.Interview.Common.Models;

/// <summary>
/// Represents the result of an operation with success/failure state and optional data or error message
/// </summary>
/// <typeparam name="T">The type of data returned on success</typeparam>
public class Result<T> where T : class
{
    public bool IsSuccess { get; private set; }
    public T? Value { get; private set; }
    public string? Error { get; private set; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, null, error);
}

/// <summary>
/// Represents a paginated result with data, total count, and pagination metadata
/// </summary>
/// <typeparam name="T">The type of items in the paginated result</typeparam>
public class PaginatedResult<T> where T : class
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
