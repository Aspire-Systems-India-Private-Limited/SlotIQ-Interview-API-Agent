namespace SlotIQ.Interview.Common.Models;

/// <summary>
/// Standard API response wrapper for consistent response structure
/// </summary>
/// <typeparam name="T">The type of data returned in the response</typeparam>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorCode { get; set; }
}
