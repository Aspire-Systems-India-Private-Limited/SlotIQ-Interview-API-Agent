namespace SlotIQ.Interview.Common.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public string? SuccessCode { get; set; }
    public string? SuccessMessage { get; set; }
}
