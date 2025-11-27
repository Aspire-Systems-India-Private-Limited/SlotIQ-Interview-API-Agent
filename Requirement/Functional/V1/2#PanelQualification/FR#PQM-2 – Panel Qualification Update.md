## FR#PQM-2 – Panel Qualification Update for Member Panel Qualifications

### Description
System shall support updating existing panel qualifications for members, allowing modifications to position, level, and status while maintaining audit history.

### User Roles
- Master Admin
- Practice Admin
---
### Personas and Permissions
- [Master Admin] Can update panel qualifications for all members across practices
- [Practice Admin] Can update panel qualifications only for members within their practice
- [Tech Team Panel Member] No panel qualification update permissions
- [TA Team Admin] Can view panel qualifications but cannot modify

---
### Preconditions and Postconditions
- [Preconditions]
    - Panel qualification exists and is active
    - Member exists and is active
    - Updated recruitment position is valid
    - Updated applicable level is valid
    - Initiator has permission for the target practice
    - No conflicting active qualification exists

- [Postconditions]
    - Panel qualification is updated with new details
    - Audit fields are updated
    - Update history is recorded
    - Related availability plans are validated

### Module Name
- Panel Qualification Management
---
### Fields level Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`PanelID` | Panel qualification identifier | UUID | Must exist | Yes | None | "550e8400-e29b-41d4-a716-446655440000"
`RecruitmentPosition` | Updated position type | Enum | Must be valid RecruitmentPositionEnum value | Yes | Current Value | "SSEFullStack"
`ApplicableLevel` | Updated experience level | Enum | Must be valid ApplicableLevelEnum value | Yes | Current Value | "L2"
`IsActive` | Active status | Boolean | Must be boolean | Yes | Current Value | true
`ModifiedBy` | User updating the qualification | String | Must be valid user ID | Yes | None | "admin123"
`Source` | Application source | Enum | Must be valid SourceEnum value | Yes | None | "Web"

---
#### System-Level Business Rules
- `ModifiedDate` must be set to current UTC datetime
- `ModifiedBy` must be set to current user ID
- Cannot create duplicate active qualifications for same member/position/level
- Must maintain update history for audit trail
- Practice-based access control must be enforced
- Status changes must validate related availability plans
---

#### API Level Business Rules
Result Name | ResultType | Response and Actions | Example | Should be logged | Response Category
------------|------------|---------------------|---------|-----------------|------------------
PANEL_UPDATE_SUCCESS | Success | Returns PanelID and success message | {"panelID": "550e8400-e29b-41d4-a716-446655440000", "successCode": "PANEL_UPDATE_SUCCESS", "successMessage": "Panel qualification updated successfully"} | No | Informational
PANEL_UPDATE_FAILURE | Failure | Returns error code and message | {"errorCode": "PANEL_UPDATE_FAILURE", "errorMessage": "Failed to update panel qualification"} | Yes | Error
UNAUTHORIZED_ERROR | Failure | Returns error code and message | {"errorCode": "UNAUTHORIZED_ERROR", "errorMessage": "You are not authorized to perform this operation"} | Yes | Error
FORBIDDEN_ERROR | Failure | Returns error code and message | {"errorCode": "FORBIDDEN_ERROR", "errorMessage": "Access denied for practice"} | Yes | Error
VALIDATION_ERROR | Failure | Returns error code and message | {"errorCode": "VALIDATION_ERROR", "errorMessage": "Invalid recruitment position"} | Yes | Informational
PANEL_NOT_FOUND | Failure | Returns error code and message | {"errorCode": "PANEL_NOT_FOUND", "errorMessage": "Panel qualification not found"} | Yes | Error
DUPLICATE_QUALIFICATION | Failure | Returns error code and message | {"errorCode": "DUPLICATE_QUALIFICATION", "errorMessage": "Qualification already exists"} | Yes | Error
MEMBER_NOT_FOUND | Failure | Returns error code and message | {"errorCode": "MEMBER_NOT_FOUND", "errorMessage": "Member not found"} | Yes | Error

### Security and Compliances
- [Access Control] Enforce practice-based access restrictions
- [Audit Trail] Record all qualification updates with before/after values
- [Data Integrity] Prevent duplicate active qualifications
- [Validation] Ensure all referenced entities exist and are active
- [History] Maintain complete modification history

### Glossary
- [Panel Qualification] Assignment of interview position and level to a member
- [Recruitment Position] Type of role the panel member can interview for
- [Applicable Level] Experience level the panel member can assess
- [Practice] Organizational unit used for access control
- [Update History] Record of all changes made to a qualification
