namespace SlotIQ.Interview.Common.Constants;

/// <summary>
/// Centralized error messages and codes for consistent error handling
/// </summary>
public static class ErrorMessages
{
    // Success Messages
    public const string MemberOnboardSuccess = "User onboarded successfully.";
    public const string MemberUpdateSuccess = "Member details updated successfully.";
    public const string MemberListSuccess = "Members retrieved successfully.";
    
    // Validation Error Messages
    public const string UserNameRequired = "UserName is required.";
    public const string UserNameLength = "UserName must be min 5 chars and max 100 chars.";
    public const string UserNameFormat = "User name should be in Active Directory format.";
    public const string FirstNameRequired = "First name is required.";
    public const string FirstNameLength = "First name must be min 2 chars and max 50 chars.";
    public const string LastNameRequired = "Last name is required.";
    public const string LastNameLength = "Last name must be min 2 chars and max 50 chars.";
    public const string EmailAddressRequired = "EmailAddress is required.";
    public const string EmailAddressDomain = "EmailAddress must be in aspiresys.com domain.";
    public const string PhoneNumberFormat = "Phonenumber must be in valid format.";
    public const string PracticeRequired = "Practice is required.";
    public const string PracticeInvalid = "Practice must be valid PracticeID.";
    public const string RoleRequired = "Role is required.";
    public const string RoleInvalid = "Role must be valid RoleID.";
    public const string SourceRequired = "Source is required.";
    public const string SourceInvalid = "Source must be valid Application SourceID.";
    public const string IsActiveRequired = "IsActive is required.";
    public const string IsActiveInvalid = "IsActive must be valid boolean.";
    
    // Duplicate Entry Errors
    public const string DuplicateUserName = "Duplicate entry found.UserName already exists.";
    public const string DuplicateEmailAddress = "Duplicate entry found.EmailAddress already exists.";
    public const string DuplicatePhoneNumber = "Duplicate entry found.Phonenumber already exists.";
    
    // Resource Not Found Errors
    public const string InvalidPractice = "Resource not found.Invalid Practice";
    public const string InvalidRole = "Resource not found.Invalid Role";
    public const string InvalidSource = "Resource not found.Invalid Source";
    public const string MemberNotFound = "Member not found";
    
    // System Errors
    public const string UserOnboardFailure = "User onboard failed.";
    public const string MemberUpdateFailure = "Failed to update member. Please try again later.";
    public const string SystemError = "Failed to onboard user. Please try again later.";
    public const string ServiceUnavailable = "Service is currently unavailable. Please try again later.";
    public const string UnauthorizedError = "You are not authorized to perform this operation.";
    public const string ForbiddenError = "You are not authorized to perform this operation.";
}

/// <summary>
/// Error codes for API responses
/// </summary>
public static class ErrorCodes
{
    public const string MemberOnboardSuccess = "MEMBER_ONBOARD_SUCCESS";
    public const string MemberUpdateSuccess = "MEMBER_UPDATE_SUCCESS";
    public const string MemberListSuccess = "MEMBER_LIST_SUCCESS";
    public const string UserOnboardFailure = "USER_ONBOARD_FAILURE";
    public const string ValidationError = "VALIDATION_ERROR";
    public const string DuplicateEntryError = "DUPLICATE_ENTRY_ERROR";
    public const string ResourceNotFoundError = "RESOURCE_NOT_FOUND_ERROR";
    public const string SystemError = "SYSTEM_ERROR";
    public const string ServiceUnavailableError = "SERVICE_UNAVAILABLE_ERROR";
    public const string UnauthorizedError = "UNAUTHORIZED_ERROR";
    public const string ForbiddenError = "FORBIDDEN_ERROR";
}
