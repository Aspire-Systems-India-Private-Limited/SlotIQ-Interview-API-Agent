## 3.1 FR#MAP-5 – Deactivate Member

### Description
Authorized users can deactivate a member to revoke access without deleting the record. Deactivation sets `IsActive = false` and updates audit fields.

### User Roles
- Master Admin
- Practice Admin
---
### Personas and Permissions
- [Master Admin] Can deactivate any member across practices.
- [Practice Admin] Can deactivate members within their practice. Cannot deactivate Master Admins.
- [Tech Team Panel Member] No deactivation permission.
- [TA Team Admin] No deactivation permission.

---
### Preconditions and Postconditions
- [Preconditions]
    - Target `MemberID` exists and is currently active.
    - Initiator has permission to deactivate the target member.

- [Postconditions]
    - Member `IsActive` is set to `false`.
    - Audit fields updated; optional notification sent.

### Module Name
- Member Managment 
---
### Fields level  Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`MemberID` | Unique identifier of member to deactivate | guid | Must exist and be active | Yes | None | "12345678-1234-1234-1234-123456789012"
`Reason` | Deactivation reason | string | Optional; if provided Max 250 | No | None | "Left organization"
`UpdatedBy` | Updated by | string | Must be current user ID | Yes | None | "user123"
`Source` | Application Source | string | Must be valid Application SourceID | Yes | None | "Admin"
---

#### System-Level Business Rules
- Deactivation is soft: data retained for audit/compliance.
- Prevent self-deactivation for the last remaining Master Admin.
- `UpdatedDate` set to current timestamp.
---

#### API Level Business Rules
Result Name | ResultType | Response and ActionsValidation | Example | Should be logged | Response Category
------------|------------|--------------------------------|-------------------|-------------------|-------------------
MEMBER_DEACTIVATE_SUCCESS | Success | Returns MemberID and success message | {"MemberID": "12345678-1234-1234-1234-123456789012","SuccessCode":"MEMBER_DEACTIVATE_SUCCESS", "SuccessMessage": "Member deactivated successfully."} | Yes | Informational
UNAUTHORIZED_ERROR | Failure | Authentication missing/invalid | {"ErrorCode": "UNAUTHORIZED_ERROR", "ErrorMessage": "Authentication required."} | Yes | Error
FORBIDDEN_ERROR | Failure | Insufficient permissions | {"ErrorCode": "FORBIDDEN_ERROR", "ErrorMessage": "You are not authorized to deactivate this member."} | Yes | Error
RESOURCE_NOT_FOUND_ERROR | Failure | Member not found/ already inactive | {"ErrorCode": "RESOURCE_NOT_FOUND_ERROR", "ErrorMessage": "Member not found or already inactive."} | Yes | Error
SYSTEM_ERROR | Failure | Unhandled exception | {"ErrorCode": "SYSTEM_ERROR", "ErrorMessage": "Failed to deactivate member. Please try again later."} | Yes | Critical
SERVICE_UNAVAILABLE_ERROR | Failure | Dependency down | {"ErrorCode": "SERVICE_UNAVAILABLE_ERROR", "ErrorMessage": "Service is currently unavailable. Please try again later."} | Yes | Critical

###  Secuirty and Compliances 
- [PII handling] Do not expose sensitive data.
- [Audit] Record actor, timestamp, reason, and source.
- [Access Controls] Enforce per-role deactivation rules and practice scoping.

### Glossary
- [Member] A system user record with a role and optional practice assignment.
- [Practice] Organizational unit used to scope certain roles and permissions.
- [Role] Permission set defining what actions a member can perform.
- [Source] Originating application for the operation (e.g., WebApp, API, Admin).
