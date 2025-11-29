namespace SlotIQ.Interview.Common.Constants;

public static class ErrorMessages
{
    // General errors
    public const string DatabaseError = "A database error occurred. Please try again later.";
    public const string OperationFailed = "The operation failed. Please try again.";
    public const string InvalidPageNumber = "Page number must be greater than 0.";
    public const string InvalidPageSize = "Page size must be between 1 and 50.";

    // Member-specific errors
    public const string MemberNotFound = "Member not found.";
    public const string DuplicateUserName = "A member with this username already exists.";
    public const string DuplicateEmailAddress = "A member with this email address already exists.";
    public const string InvalidEmailDomain = "Email address must be in the aspiresys.com domain.";
    public const string InvalidRole = "The specified role is not valid.";
    public const string InvalidPractice = "The specified practice is not valid.";

    // Validation errors
    public static string Required(string fieldName) => $"{fieldName} is required.";
    public static string MaxLength(string fieldName, int maxLength) => $"{fieldName} must not exceed {maxLength} characters.";
    public static string MinLength(string fieldName, int minLength) => $"{fieldName} must be at least {minLength} characters.";
    public const string InvalidEmail = "Invalid email address format.";
}
