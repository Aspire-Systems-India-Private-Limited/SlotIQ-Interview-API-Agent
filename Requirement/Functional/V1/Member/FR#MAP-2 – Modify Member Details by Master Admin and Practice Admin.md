## 3.1 FR#MAP-2 – Modify Member Details by Master Admin and Practice Admin

### Description
Authorized users can modify existing member details subject to role-based rules and practice scoping. Sensitive fields like `UserName` and `MemberID` are immutable.

### User Roles
- Master Admin
- Practice Admin
---
### Personas and Permissions
- [Master Admin] Can modify: Practice Admin, Tech Team Panel Member, TA Team Admin, and Master Admin. Not bound to any Practice.
- [Practice Admin] Can modify: Practice Admin (same practice), Tech Team Panel Member, TA Team Admin (same practice). Cannot modify Master Admin or cross-practice members.
- [Tech Team Panel Member] No modification permissions.
- [TA Team Admin] No modification permissions.

---
### Preconditions and Postconditions
- [Preconditions]
    - Target `MemberID` exists and is active (unless updating allowed fields on inactive is explicitly permitted — default: not permitted).
    - Initiator has permission to modify the target member per role and practice scope.
    - Role catalog and practice exist and are active where applicable.

- [Postconditions]
    - Member fields are updated as requested.
    - Audit fields are updated accordingly.
    - A success result is returned with the updated `MemberID` and correlation data.

### Module Name
- Member Managment 
---
### Fields level  Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`MemberID` | Unique identifier of member to modify | guid | Must exist | Yes | None | "12345678-1234-1234-1234-123456789012"
`UserName` | Immutable login identifier | string | Cannot be modified | No | Existing | "user123"
`Firstname` | First name of member | string | Min 2, Max 50 | No | Existing | "John"
`Lastname` | Last name of member | string | Min 2, Max 50 | No | Existing | "Doe"
`EmailAddress` | Email address | string | Valid, unique, domain `aspiresys.com` | No | Existing | "user123@aspiresys.com"
`PhoneNumber` | Phone number | string | Optional; if provided valid and unique | No | Existing | "1234567890"
`Rolename` | Role name | string | Must be valid RoleID; Practice Admin cannot set Master Admin; cross-practice rules apply | No | Existing | "Practice Admin"
`PracticeName` | Practice association | string | Must be valid PracticeID; Practice Admin can only set to own practice | No | Existing | ".NET"
`IsActive` | Active flag | boolean | Cannot be modified here; use Deactivate API | No | Existing | true
`UpdatedBy` | Updated by | string | Must be current user ID | Yes | None | "user123"
`Source` | Application Source | string | Must be valid Application SourceID | Yes | None | "WebApp"
---

#### System-Level Business Rules
- `MemberID` is immutable primary key.
- Changing `Rolename` may affect permissions; validate per initiator role and scope.
- `CreatedDate` remains unchanged; `UpdatedDate` set to current timestamp.
- Changes to `EmailAddress`/`PhoneNumber` must preserve uniqueness.
- Sensitive credentials (e.g., Password) are not exposed or modified via this FR.
---

#### API Level Business Rules
Result Name | ResultType | Response and ActionsValidation | Example | Should be logged | Response Category
------------|------------|--------------------------------|-------------------|-------------------|-------------------
MEMBER_UPDATE_SUCCESS | Success | Returns MemberID and success message | {"MemberID": "12345678-1234-1234-1234-123456789012","SuccessCode":"MEMBER_UPDATE_SUCCESS", "SuccessMessage": "Member details updated successfully."} | No | Informational
UNAUTHORIZED_ERROR | Failure | Missing/invalid authentication | {"ErrorCode": "UNAUTHORIZED_ERROR", "ErrorMessage": "Authentication required."} | Yes | Error
FORBIDDEN_ERROR | Failure | Lacks permission for target member/role/practice | {"ErrorCode": "FORBIDDEN_ERROR", "ErrorMessage": "You are not authorized to modify this member."} | Yes | Error
VALIDATION_ERROR | Failure | Invalid input | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "Firstname must be min 2 and max 50 chars."} | Yes | Informational
RESOURCE_NOT_FOUND_ERROR | Failure | MemberID not found | {"ErrorCode": "RESOURCE_NOT_FOUND_ERROR", "ErrorMessage": "Member not found."} | Yes | Error
DUPLICATE_ENTRY_ERROR | Failure | Unique field conflict | {"ErrorCode": "DUPLICATE_ENTRY_ERROR", "ErrorMessage": "EmailAddress already exists."} | Yes | Error
SYSTEM_ERROR | Failure | Unhandled exception | {"ErrorCode": "SYSTEM_ERROR", "ErrorMessage": "Failed to update member. Please try again later."} | Yes | Critical
SERVICE_UNAVAILABLE_ERROR | Failure | Dependency down | {"ErrorCode": "SERVICE_UNAVAILABLE_ERROR", "ErrorMessage": "Service is currently unavailable. Please try again later."} | Yes | Critical

###  Secuirty and Compliances 
- [PII handling] Update only necessary PII. Never return secrets.
- [Audit] Record modifier, timestamp, changed fields (before/after mask for PII), and source application.
- [Access Controls] Enforce per-role modification permissions and practice scoping.

### Glossary
- [Member] A system user record with a role and optional practice assignment.
- [Practice] Organizational unit used to scope certain roles and permissions.
- [Role] Permission set defining what actions a member can perform.
- [Source] Originating application for the operation (e.g., WebApp, API, Admin).
