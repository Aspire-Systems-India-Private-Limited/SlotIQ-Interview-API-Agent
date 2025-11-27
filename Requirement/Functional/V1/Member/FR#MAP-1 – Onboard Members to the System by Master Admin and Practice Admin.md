## 3.1 FR#MAP-1 – Onboarding the Members with Role Master Admin, Practice Admin, Tech Team Panel Member, TA Team Admin

### Description
System can have various users who can perform multiple operations based on their roles. 

### User Roles
- Master Admin
- Practice Admin
---
### Personas and Permissions
- [Master Admin] Can onboard: Master Admin, Practice Admin, Tech Team Panel Member, TA Team Admin. Not bound to any Practice.
- [Practice Admin] Can onboard: Practice Admin (same practice), Tech Team Panel Member, TA Team Admin (same practice).
- [Tech Team Panel Member] No onboarding permissions.
- [TA Team Admin] No onboarding permissions.

---
### Preconditions and Postconditions
- [Preconditions]
    - Role catalog contains the target RoleID.
    - Practice exists and is active when required by role.
    - Initiator has permission to create the target role in the selected scope

- [Postconditions]
    - Member is created with the specified Role  and Practice.
    - Audit fields are set.
    - Welcome email is sent.
    - MemberID is returned in the response.


### Module Name
- Member Managment 
---
### Fields level  Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`UserName` | User name (Active Directory format) will be used for Authentication | string | Must be unique , must be valid Active Directory format,  Min 5 characters, Max 100 characters, | Yes | None | "user123"
`Firstname` | First name of member | string | Min 2 characters, Max 50 characters | Yes | None | "John"
`Lastname` | Last name of the member | string | Min 2 characters, Max 50 characters | Yes | None | "Doe"
`Password` | Password | string | Auto-generated; min 8 characters with alphabets, numbers, and special characters (`@`, `#`, `$`, `-`, `_`);  | Yes | None | "P@ssw0rd"
`Rolename` | Role name it will be enum value such as 'Practice Admin, 'Master Admin, 'Tech Panel Member' , 'TA Team Admin' | string | Must be valid RoleID | Yes | None | "Practice Admin"
`EmailAddress` | Email address for communication | string | Must be valid, unique, and in `aspiresys.com` domain | Yes | None | "user123@aspiresys.com"
`CountryCode` | Phone number country code | string | Optional; if provided, length is 0 to 3 | No | None | "91"
`PhoneNumber` | Phone number of the member | string | Optional; if provided, must be valid and unique | No | None | "1234567890"
`PracticeName` | Practice in which the member belongs to.   | string | Must be valid PracticeID | Yes | None | ".NET", "JLM", "D&A"
`IsActive` | Is active | boolean | Must be `true` | Yes | None | true
`UpdatedBy` | Updated by | string | Must be current user ID | Yes | None | "user123"
`Source` | Application Source the consuming application name 'WebApp', MobileApp', 'API', 'Admin' | string | Must be valid Application SourceID | Yes | None | "Application SourceID"
---

#### System-Level Business Rules
- `MemberID`  MemberID is a system-generated GUID (Globally Unique Identifier) used to uniquely identify each onboarded user.
- `CreatedDate` and `UpdatedDate` must be set to current date and time
- `UpdatedBy` must be set to current user ID
- Password must be auto-generated and meet complexity rules
- Role determines system permissions
- Practice assignment is mandatory
---

#### API Level Business Rules
Result Name | ResultType | Response and ActionsValidation | Example | Should be logged | Response Category
------------|------------|--------------------------------|-------------------|-------------------|-------------------
MEMBER_ONBOARD_SUCCESS | Success | Returns MemberID and success message | {"MemberID": "12345678-1234-1234-1234-123456789012","SuccessCode":"MEMBER_ONBOARD_SUCCESS", "SuccessMessage": "User onboarded successfully."} | No | Informational
USER_ONBOARD_FAILURE | Failure | Returns error code and message | {"ErrorCode": "USER_ONBOARD_FAILURE", "ErrorMessage": "User onboard failed."} | Yes | Error
UNAUTHORIZED_ERROR | Failure | Returns error code and message | {"ErrorCode": "UNAUTHORIZED_ERROR", "ErrorMessage": "You are not authorized to perform this operation."} | Yes | Error
ACCESSDENIED_ERROR | Failure | Returns error code and message | {"ErrorCode": "FORBIDDEN_ERROR", "ErrorMessage": "You are not authorized to perform this operation."} | Yes | Error
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "UserName is required."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "UserName must by min 5 chars and max 100 chars."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "User name should be in Active Directory format."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "First name is required."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "First name must by min 2 chars and max 50 chars."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "Last name is required."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "Last name must by min 2 chars and max 50 chars."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "EmailAddress is required."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "EmailAddress must be in aspiresys.com domain."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "Phonenumber must be in valid format."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "Practice is required."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "Practice must be valid PracticeID."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "Role  is required."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "Role must be valid RoleID."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "Source is required."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "Source must be valid Application SourceID."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "IsActive is required."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "IsActive must be valid boolean."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "UpdatedBy is required."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "UpdatedBy must be valid guid."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "UpdatedDate is required."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "UpdatedDate must be valid datetime."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "CreatedDate is required."} | Yes | Informational
"VALIDATION_ERROR" | Failure | Returns error code and message | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "CreatedDate must be valid datetime."} | Yes | Informational
RESOURCE_NOT_FOUND_ERROR | Failure | Returns error code and message | {"ErrorCode": "RESOURCE_NOT_FOUND_ERROR", "ErrorMessage": "Resource not found.Invalid Practice"} | Yes | Error
RESOURCE_NOT_FOUND_ERROR | Failure | Returns error code and message | {"ErrorCode": "RESOURCE_NOT_FOUND_ERROR", "ErrorMessage": "Resource not found.Invalid Role"} | Yes | Error
RESOURCE_NOT_FOUND_ERROR | Failure | Returns error code and message | {"ErrorCode": "RESOURCE_NOT_FOUND_ERROR", "ErrorMessage": "Resource not found.Invalid Source"} | Yes | Error
DUPLICATE_ENTRY_ERROR | Failure | Returns error code and message | {"ErrorCode": "DUPLICATE_ENTRY_ERROR", "ErrorMessage": "Duplicate entry found.UserName already exists."} | Yes | Error
DUPLICATE_ENTRY_ERROR | Failure | Returns error code and message | {"ErrorCode": "DUPLICATE_ENTRY_ERROR", "ErrorMessage": "Duplicate entry found.EmailAddress already exists."} | Yes | Error
DUPLICATE_ENTRY_ERROR | Failure | Returns error code and message | {"ErrorCode": "DUPLICATE_ENTRY_ERROR", "ErrorMessage": "Duplicate entry found.Phonenumber already exists."} | Yes | Error
"SYSTEM_ERROR" | Failure | Returns error code and message | {"ErrorCode": "SYSTEM_ERROR", "ErrorMessage": "Failed to onboard user. Please try again later."} | Yes | Critical
"SERVICE_UNAVAILABLE_ERROR" | Failure | Returns error code and message | {"ErrorCode": "SERVICE_UNAVAILABLE_ERROR", "ErrorMessage": "Service is currently unavailable. Please try again later."} | Yes | Critical

###  Secuirty and Compliances 
- [PII handling] Store only necessary PII. No plaintext passwords anywhere.
- [Audit] Record creator, timestamp, source application, and key field changes.
- [Access Controls] Enforce per-role creation permissions and practice scoping.

### Glossary

- [Member] A system user record with a role and optional practice assignment.
- [Practice] Organizational unit used to scope certain roles and permissions.
- [Role] Permission set defining what actions a member can perform.
- [Source] Originating application for the operation (e.g., WebApp, API, Admin).