## FR#MAP-3 – Retrieve All Members with Pagination

### Description
Authorized users can retrieve a paginated list of members with optional sorting and basic filters. Results are limited by the initiator's permissions and practice scope.

### User Roles
- Master Admin
- Practice Admin
---
### Personas and Permissions
- [Master Admin] Can list all members across practices.
- [Practice Admin] Can list members within their own practice only.
- [Tech Team Panel Member] No list permission.
- [TA Team Admin] No list permission.

---
### Preconditions and Postconditions
- [Preconditions]
    - Initiator has permission to view member listings.

- [Postconditions]
    - Paginated result set returned with metadata and items.

### Module Name
- Member Managment 
---
### Fields level  Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`PageNumber` | Page number (1-based) | int | Min 1 | No | 1 | 1
`PageSize` | Items per page | int | Min 1, Max 200 | No | 25 | 25
`SortBy` | Sort field | string | Allowed: UserName, Firstname, Lastname, EmailAddress, RoleName, PracticeName, CreatedDate | No | CreatedDate | "CreatedDate"
`SortOrder` | Sort direction | string | Allowed: ASC, DESC | No | DESC | "DESC"
`IsActive` | Filter by active status | boolean | Optional | No | null | true
`RoleName` | Filter by role | string | Must be valid RoleID if provided | No | null | "Practice Admin"
`PracticeName` | Filter by practice | string | Must be valid PracticeID if provided | No | null | ".NET"
`Source` | Application Source | string | Must be valid Application SourceID | Yes | None | "WebApp"
`UpdatedBy` | Requestor ID | string | Must be current user ID | Yes | None | "user123"
---

#### System-Level Business Rules
- Results constrained by initiator's scope (practice for Practice Admin).
- Pagination must return metadata: `TotalCount`, `PageNumber`, `PageSize`, `HasNext`, `HasPrevious`.
- Sorting must be deterministic and protected against SQL injection via allow-list fields.
---

#### API Level Business Rules
Result Name | ResultType | Response and ActionsValidation | Example | Should be logged | Response Category
------------|------------|--------------------------------|-------------------|-------------------|-------------------
MEMBER_LIST_SUCCESS | Success | Returns paginated members | {"SuccessCode":"MEMBER_LIST_SUCCESS","SuccessMessage":"Members retrieved successfully.","TotalCount":150,"PageNumber":1,"PageSize":25,"Items":[{"MemberID":"...","UserName":"user123"}]} | No | Informational
UNAUTHORIZED_ERROR | Failure | Authentication missing/invalid | {"ErrorCode": "UNAUTHORIZED_ERROR", "ErrorMessage": "Authentication required."} | Yes | Error
FORBIDDEN_ERROR | Failure | Insufficient permissions | {"ErrorCode": "FORBIDDEN_ERROR", "ErrorMessage": "You are not authorized to view members."} | Yes | Error
VALIDATION_ERROR | Failure | Invalid paging/sort/filter | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "PageSize must be between 1 and 200."} | Yes | Informational
SYSTEM_ERROR | Failure | Unhandled exception | {"ErrorCode": "SYSTEM_ERROR", "ErrorMessage": "Failed to list members. Please try again later."} | Yes | Critical
SERVICE_UNAVAILABLE_ERROR | Failure | Dependency down | {"ErrorCode": "SERVICE_UNAVAILABLE_ERROR", "ErrorMessage": "Service is currently unavailable. Please try again later."} | Yes | Critical

###  Secuirty and Compliances 
- [PII handling] Return minimal necessary fields; avoid sensitive data.
- [Audit] Log requester, filters used, pagination, and count returned.
- [Access Controls] Enforce per-role view permissions and practice scoping.

### Glossary
- [Member] A system user record with a role and optional practice assignment.
- [Practice] Organizational unit used to scope certain roles and permissions.
- [Role] Permission set defining what actions a member can perform.
- [Source] Originating application for the operation (e.g., WebApp, API, Admin).
