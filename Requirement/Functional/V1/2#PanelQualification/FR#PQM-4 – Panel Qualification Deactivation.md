## FR#PQM-4 – Panel Qualification Deactivation for Member Panel Qualifications

### Description
System shall support soft deletion (deactivation) of panel qualifications with reason tracking and audit trail maintenance.

### User Roles
- Master Admin
- Practice Admin
---
### Personas and Permissions
- [Master Admin] Can deactivate panel qualifications for all members across practices
- [Practice Admin] Can deactivate panel qualifications only for members within their practice
- [Tech Team Panel Member] No panel qualification deactivation permissions
- [TA Team Admin] Can view panel qualifications but cannot deactivate

---
### Preconditions and Postconditions
- [Preconditions]
    - Panel qualification exists and is active
    - Member exists and is active
    - Panel qualification ID is valid
    - Initiator has permission for the target practice
    - Deactivation reason is provided
    - No active interview schedules exist for this qualification

- [Postconditions]
    - Panel qualification is marked as inactive
    - Deactivation reason is recorded
    - Audit fields are updated
    - Related availability plans are updated if needed
    - Deactivation history is logged

### Module Name
- Panel Qualification Management
---
### Fields level Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`PanelID` | Panel qualification identifier | UUID | Must exist | Yes | None | "550e8400-e29b-41d4-a716-446655440000"
`Reason` | Deactivation reason | String | Min 5 chars, Max 500 chars | Yes | None | "Panel member no longer available for this position"
`ModifiedBy` | User deactivating the qualification | String | Must be valid user ID | Yes | None | "admin123"
`Source` | Application source | Enum | Must be valid SourceEnum value | Yes | None | "Web"

---
#### System-Level Business Rules
- `ModifiedDate` must be set to current UTC datetime
- `ModifiedBy` must be set to current user ID
- `IsActive` must be set to false
- Cannot deactivate already inactive qualifications
- Must validate no active interview schedules exist
- Must maintain deactivation audit trail
---

#### API Level Business Rules
Result Name | ResultType | Response and Actions | Example | Should be logged | Response Category
------------|------------|---------------------|---------|-----------------|------------------
PANEL_DEACTIVATE_SUCCESS | Success | Returns PanelID and success message | {"panelID": "550e8400-e29b-41d4-a716-446655440000", "successCode": "PANEL_DEACTIVATE_SUCCESS", "successMessage": "Panel qualification deactivated successfully"} | Yes | Informational
PANEL_DEACTIVATE_FAILURE | Failure | Returns error code and message | {"errorCode": "PANEL_DEACTIVATE_FAILURE", "errorMessage": "Failed to deactivate panel qualification"} | Yes | Error
UNAUTHORIZED_ERROR | Failure | Returns error code and message | {"errorCode": "UNAUTHORIZED_ERROR", "errorMessage": "You are not authorized to perform this operation"} | Yes | Error
FORBIDDEN_ERROR | Failure | Returns error code and message | {"errorCode": "FORBIDDEN_ERROR", "errorMessage": "Access denied for practice"} | Yes | Error
VALIDATION_ERROR | Failure | Returns error code and message | {"errorCode": "VALIDATION_ERROR", "errorMessage": "Deactivation reason is required"} | Yes | Informational
PANEL_NOT_FOUND | Failure | Returns error code and message | {"errorCode": "PANEL_NOT_FOUND", "errorMessage": "Panel qualification not found"} | Yes | Error
MEMBER_NOT_FOUND | Failure | Returns error code and message | {"errorCode": "MEMBER_NOT_FOUND", "errorMessage": "Member not found"} | Yes | Error
ACTIVE_SCHEDULES_ERROR | Failure | Returns error code and message | {"errorCode": "ACTIVE_SCHEDULES_ERROR", "errorMessage": "Cannot deactivate: Active interview schedules exist"} | Yes | Error
ALREADY_INACTIVE_ERROR | Failure | Returns error code and message | {"errorCode": "ALREADY_INACTIVE_ERROR", "errorMessage": "Panel qualification is already inactive"} | Yes | Error

### Security and Compliances
- [Access Control] Enforce practice-based access restrictions
- [Audit Trail] Record complete deactivation details including reason
- [Data Integrity] Validate related entity states
- [History] Maintain deactivation history for audit purposes
- [Validation] Ensure proper reason documentation

### Glossary
- [Panel Qualification] Assignment of interview position and level to a member
- [Deactivation] Soft deletion of a panel qualification while maintaining history
- [Active Schedules] Upcoming or pending interview schedules
- [Practice] Organizational unit used for access control
- [Audit Trail] Record of all changes including who, when, and why
