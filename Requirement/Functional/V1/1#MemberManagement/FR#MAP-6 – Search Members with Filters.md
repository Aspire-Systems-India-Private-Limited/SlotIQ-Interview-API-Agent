## FR#MAP-6 – Search Members with Filters

### Description
Authorized users can search members using multiple filters and free-text queries, with pagination and sorting. Results are constrained by role-based access and practice scope.

### User Roles
- Master Admin
- Practice Admin
---
### Personas and Permissions
- [Master Admin] Can search across practices.
- [Practice Admin] Can search within own practice only.
- [Tech Team Panel Member] No search permission.
- [TA Team Admin] No search permission.

---
### Preconditions and Postconditions
- [Preconditions]
    - Initiator has permission to view member data.

- [Postconditions]
    - Paginated search results returned with metadata and items.

### Module Name
- Member Managment 
---
### Fields level  Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`Query` | Free text query | string | Optional; Min 2, Max 100 | No | null | "john do"
`RoleName` | Filter by role | string | Must be valid RoleID if provided | No | null | "Practice Admin"
`PracticeName` | Filter by practice | string | Must be valid PracticeID if provided | No | null | ".NET"
`IsActive` | Filter by active status | boolean | Optional | No | null | true
`CreatedDateFrom` | Created date from | datetime | Must be <= CreatedDateTo | No | null | "2025-01-01"
`CreatedDateTo` | Created date to | datetime | Must be >= CreatedDateFrom | No | null | "2025-12-31"
`PageNumber` | Page number | int | Min 1 | No | 1 | 1
`PageSize` | Items per page | int | Min 1, Max 200 | No | 25 | 25
`SortBy` | Sort field | string | Allow-list fields | No | CreatedDate | "CreatedDate"
`SortOrder` | Sort direction | string | ASC or DESC | No | DESC | "DESC"
`Source` | Application Source | string | Must be valid Application SourceID | Yes | None | "WebApp"
`UpdatedBy` | Requestor ID | string | Must be current user ID | Yes | None | "user123"
---

#### System-Level Business Rules
- Apply practice scoping for Practice Admin.
- Use safe, indexed filters; parameterized queries only; enforce allow-list for sort fields.
- Return pagination metadata: `TotalCount`, `PageNumber`, `PageSize`, `HasNext`, `HasPrevious`.
---

#### API Level Business Rules
Result Name | ResultType | Response and ActionsValidation | Example | Should be logged | Response Category
------------|------------|--------------------------------|-------------------|-------------------|-------------------
MEMBER_SEARCH_SUCCESS | Success | Returns paginated members matching filters | {"SuccessCode":"MEMBER_SEARCH_SUCCESS","SuccessMessage":"Members retrieved successfully.","TotalCount":12,"PageNumber":1,"PageSize":25,"Items":[{"MemberID":"...","UserName":"user123"}]} | No | Informational
UNAUTHORIZED_ERROR | Failure | Authentication missing/invalid | {"ErrorCode": "UNAUTHORIZED_ERROR", "ErrorMessage": "Authentication required."} | Yes | Error
FORBIDDEN_ERROR | Failure | Insufficient permissions | {"ErrorCode": "FORBIDDEN_ERROR", "ErrorMessage": "You are not authorized to search members."} | Yes | Error
VALIDATION_ERROR | Failure | Invalid filter/sort/paging | {"ErrorCode": "VALIDATION_ERROR", "ErrorMessage": "PageSize must be between 1 and 200."} | Yes | Informational
SYSTEM_ERROR | Failure | Unhandled exception | {"ErrorCode": "SYSTEM_ERROR", "ErrorMessage": "Failed to search members. Please try again later."} | Yes | Critical
SERVICE_UNAVAILABLE_ERROR | Failure | Dependency down | {"ErrorCode": "SERVICE_UNAVAILABLE_ERROR", "ErrorMessage": "Service is currently unavailable. Please try again later."} | Yes | Critical

###  Secuirty and Compliances 
- [PII handling] Return minimal necessary fields; avoid sensitive data.
- [Audit] Log requester, filters and query used, pagination, and count returned.
- [Access Controls] Enforce per-role view permissions and practice scoping.

### Glossary
- [Member] A system user record with a role and optional practice assignment.
- [Practice] Organizational unit used to scope certain roles and permissions.
- [Role] Permission set defining what actions a member can perform.
- [Source] Originating application for the operation (e.g., WebApp, API, Admin).
