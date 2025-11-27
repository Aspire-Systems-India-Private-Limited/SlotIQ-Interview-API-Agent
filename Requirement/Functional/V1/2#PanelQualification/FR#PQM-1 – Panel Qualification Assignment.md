## FR#PQM-1 – Panel Qualification Assignment for Member Panel Qualifications

### Description
System shall manage panel qualifications for members, allowing assignment of recruitment positions and interview levels to qualified members.

### User Roles
- Master Admin
- Practice Admin
---
### Personas and Permissions
- [Master Admin] Can manage panel qualifications for all members across practices
- [Practice Admin] Can manage panel qualifications only for members within their practice
- [Tech Team Panel Member] No panel qualification management permissions
- [TA Team Admin] Can view panel qualifications but cannot modify

---
### Preconditions and Postconditions
- [Preconditions]
    - Member exists and is active
    - Recruitment position is valid
    - Applicable level is valid
    - Initiator has permission for the target practice
    - No duplicate active qualification exists for same position/level

- [Postconditions]
    - Panel qualification is created with specified details
    - Audit fields are set
    - PanelID is returned in response
    - Member's qualification status is updated

### Module Name
- Panel Qualification Management
---
### Fields level Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`PanelID` | Unique identifier for panel qualification | UUID | System generated | Yes | None | "550e8400-e29b-41d4-a716-446655440000"
`MemberID` | Reference to the member | UUID | Must exist in Members table | Yes | None | "123e4567-e89b-12d3-a456-426614174000"
`RecruitmentPosition` | Position qualified to interview for | Enum | Must be valid RecruitmentPositionEnum value | Yes | None | "SSEFullStack"
`ApplicableLevel` | Experience level qualified to assess | Enum | Must be valid ApplicableLevelEnum value | Yes | None | "L2"
`IsActive` | Active status of qualification | Boolean | Must be boolean | Yes | true | true
`CreatedBy` | User creating the qualification | String | Must be valid user ID | Yes | None | "admin123"
`Source` | Application source | Enum | Must be valid SourceEnum value | Yes | None | "Web"

---
#### System-Level Business Rules
- `PanelID` is system-generated UUID
- `CreatedDate` and `ModifiedDate` must be set to current UTC datetime
- `CreatedBy` and `ModifiedBy` must be set to current user ID
- Cannot create duplicate active qualifications for same member/position/level
- Practice-based access control must be enforced
---

#### API Level Business Rules
Result Name | ResultType | Response and Actions | Example | Should be logged | Response Category
------------|------------|---------------------|---------|-----------------|------------------
PANEL_ADD_SUCCESS | Success | Returns PanelID and success message | {"panelID": "550e8400-e29b-41d4-a716-446655440000", "successCode": "PANEL_ADD_SUCCESS", "successMessage": "Panel qualification added successfully"} | No | Informational
PANEL_ADD_FAILURE | Failure | Returns error code and message | {"errorCode": "PANEL_ADD_FAILURE", "errorMessage": "Failed to add panel qualification"} | Yes | Error
UNAUTHORIZED_ERROR | Failure | Returns error code and message | {"errorCode": "UNAUTHORIZED_ERROR", "errorMessage": "You are not authorized to perform this operation"} | Yes | Error
FORBIDDEN_ERROR | Failure | Returns error code and message | {"errorCode": "FORBIDDEN_ERROR", "errorMessage": "Access denied for practice"} | Yes | Error
VALIDATION_ERROR | Failure | Returns error code and message | {"errorCode": "VALIDATION_ERROR", "errorMessage": "Invalid recruitment position"} | Yes | Informational
MEMBER_NOT_FOUND | Failure | Returns error code and message | {"errorCode": "MEMBER_NOT_FOUND", "errorMessage": "Member not found"} | Yes | Error
DUPLICATE_QUALIFICATION | Failure | Returns error code and message | {"errorCode": "DUPLICATE_QUALIFICATION", "errorMessage": "Qualification already exists"} | Yes | Error

### Security and Compliances
- [Access Control] Enforce practice-based access restrictions
- [Audit Trail] Record all qualification assignments and changes
- [Data Integrity] Prevent duplicate active qualifications
- [Validation] Ensure all referenced entities exist and are active

### Glossary
- [Panel Qualification] Assignment of interview position and level to a member
- [Recruitment Position] Type of role the panel member can interview for
- [Applicable Level] Experience level the panel member can assess
- [Practice] Organizational unit used for access control
