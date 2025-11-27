## 3.1 FR#MAP-4 – Retrieve Selected Member Details

### Description
Authorized users can retrieve the details of a specific member using `MemberID`. Visibility is constrained by role and practice scope.

### User Roles
- Master Admin
- Practice Admin
---
### Personas and Permissions
- [Master Admin] Can view any member details across practices.
- [Practice Admin] Can view details for members within their practice.
- [Tech Team Panel Member] No view permission.
- [TA Team Admin] No view permission.

---
### Preconditions and Postconditions
- [Preconditions]
    - `MemberID` is provided and is a valid GUID.
    - Initiator has permission to view the target member.

- [Postconditions]
    - Member details are returned if found and permitted.

### Module Name
- Member Managment 
---
### Fields level  Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`MemberID` | Unique identifier | guid | Must exist | Yes | None | "12345678-1234-1234-1234-123456789012"
`Source` | Application Source | string | Must be valid Application SourceID | Yes | None | "WebApp"
`UpdatedBy` | Requestor ID | string | Must be current user ID | Yes | None | "user123"
---

#### System-Level Business Rules
- Data access is restricted by initiator role and practice scope.
- Do not expose sensitive or unnecessary fields.
---

#### API Level Business Rules
Result Name | ResultType | Response and ActionsValidation | Example | Should be logged | Response Category
------------|------------|--------------------------------|-------------------|-------------------|-------------------
MEMBER_GET_SUCCESS | Success | Returns member details | {"SuccessCode":"MEMBER_GET_SUCCESS","SuccessMessage":"Member details retrieved successfully.","Member":{"MemberID":"...","UserName":"user123","Firstname":"John"}} | No | Informational
UNAUTHORIZED_ERROR | Failure | Authentication missing/invalid | {"ErrorCode": "UNAUTHORIZED_ERROR", "ErrorMessage": "Authentication required."} | Yes | Error
FORBIDDEN_ERROR | Failure | Insufficient permissions | {"ErrorCode": "FORBIDDEN_ERROR", "ErrorMessage": "You are not authorized to view this member."} | Yes | Error
RESOURCE_NOT_FOUND_ERROR | Failure | Member not found or out of scope | {"ErrorCode": "RESOURCE_NOT_FOUND_ERROR", "ErrorMessage": "Member not found."} | Yes | Error
VALIDATION_ERROR | Failure | Invalid MemberID | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "MemberID must be a valid GUID."} | Yes | Informational
SYSTEM_ERROR | Failure | Unhandled exception | {"ErrorCode": "SYSTEM_ERROR", "ErrorMessage": "Failed to retrieve member. Please try again later."} | Yes | Critical
SERVICE_UNAVAILABLE_ERROR | Failure | Dependency down | {"ErrorCode": "SERVICE_UNAVAILABLE_ERROR", "ErrorMessage": "Service is currently unavailable. Please try again later."} | Yes | Critical

###  Secuirty and Compliances 
- [PII handling] Return only necessary attributes; mask where appropriate.
- [Audit] Log requester, member target, and success/failure.
- [Access Controls] Enforce per-role view permissions and practice scoping.

### Glossary
- [Member] A system user record with a role and optional practice assignment.
- [Practice] Organizational unit used to scope certain roles and permissions.
- [Role] Permission set defining what actions a member can perform.
- [Source] Originating application for the operation (e.g., WebApp, API, Admin).
