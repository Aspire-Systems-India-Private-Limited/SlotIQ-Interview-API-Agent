## FR#MAM-1 – Availability Planning for Member Interview Slots

### Description
System shall manage availability plans for panel members, allowing them to schedule time slots for conducting interviews.

### User Roles
- Master Admin
- Practice Admin
- Tech Team Panel Member
- TA Team Admin
---
### Personas and Permissions
- [Master Admin] Can manage availability for all members across practices
- [Practice Admin] Can manage availability only for members within their practice
- [Tech Team Panel Member] Can manage only their own availability
- [TA Team Admin] Can view availability but cannot modify

---
### Preconditions and Postconditions
- [Preconditions]
    - Member exists and is active
    - Member has valid panel qualifications
    - Date range is in future
    - Time slots don't overlap with existing slots
    - Initiator has appropriate permissions
    - Maximum 8 hours per day restriction

- [Postconditions]
    - Availability plan is created with specified slots
    - Audit fields are set
    - AvailabilityID is returned in response
    - Initial status is set to "Available"
    - Notification sent to TA Team

### Module Name
- Availability Management
---
### Fields level Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`AvailabilityID` | Unique identifier | UUID | System generated | Yes | None | "550e8400-e29b-41d4-a716-446655440000"
`MemberID` | Member reference | UUID | Must exist in Members | Yes | None | "123e4567-e89b-12d3-a456-426614174000"
`AvailableDate` | Date of availability | Date | Future date only | Yes | None | "2024-03-01"
`AvailableTimeFrom` | Start time | Time | Valid time format | Yes | None | "09:00:00"
`AvailableTimeTo` | End time | Time | Valid time format | Yes | None | "17:00:00"
`Status` | Slot status | Enum | Valid AvailabilityStatusEnum | Yes | Available | "Available"
`CreatedBy` | Creator | String | Valid user ID | Yes | None | "admin123"
`Source` | Application source | Enum | Valid SourceEnum | Yes | None | "Web"

---
#### System-Level Business Rules
- `AvailabilityID` is system-generated UUID
- `CreatedDate` and `ModifiedDate` must be set to current UTC datetime
- `CreatedBy` and `ModifiedBy` must be set to current user ID
- Time slots must be in 60-minute increments
- No overlapping slots allowed for same member
- Maximum 8 hours availability per day
- Slots must be within business hours (9 AM - 6 PM)
- Cannot create slots for past dates
---

#### API Level Business Rules
Result Name | ResultType | Response and Actions | Example | Should be logged | Response Category
------------|------------|---------------------|---------|-----------------|------------------
AVAILABILITY_ADD_SUCCESS | Success | Returns AvailabilityID and success message | {"availabilityID": "550e8400-e29b-41d4-a716-446655440000", "successCode": "AVAILABILITY_ADD_SUCCESS", "successMessage": "Availability plan added successfully"} | No | Informational
AVAILABILITY_ADD_FAILURE | Failure | Returns error code and message | {"errorCode": "AVAILABILITY_ADD_FAILURE", "errorMessage": "Failed to add availability plan"} | Yes | Error
UNAUTHORIZED_ERROR | Failure | Returns error code and message | {"errorCode": "UNAUTHORIZED_ERROR", "errorMessage": "You are not authorized to perform this operation"} | Yes | Error
FORBIDDEN_ERROR | Failure | Returns error code and message | {"errorCode": "FORBIDDEN_ERROR", "errorMessage": "Access denied for practice"} | Yes | Error
VALIDATION_ERROR | Failure | Returns error code and message | {"errorCode": "VALIDATION_ERROR", "errorMessage": "Invalid time slot"} | Yes | Informational
MEMBER_NOT_FOUND | Failure | Returns error code and message | {"errorCode": "MEMBER_NOT_FOUND", "errorMessage": "Member not found"} | Yes | Error
SLOT_OVERLAP_ERROR | Failure | Returns error code and message | {"errorCode": "SLOT_OVERLAP_ERROR", "errorMessage": "Time slot overlaps with existing availability"} | Yes | Error
MAX_HOURS_EXCEEDED | Failure | Returns error code and message | {"errorCode": "MAX_HOURS_EXCEEDED", "errorMessage": "Maximum 8 hours per day exceeded"} | Yes | Error
INVALID_DATE_ERROR | Failure | Returns error code and message | {"errorCode": "INVALID_DATE_ERROR", "errorMessage": "Cannot create availability for past dates"} | Yes | Error

### Security and Compliances
- [Access Control] Enforce role and practice-based access restrictions
- [Audit Trail] Record all availability creation and changes
- [Data Integrity] Prevent overlapping slots and invalid dates
- [Validation] Ensure compliance with business hours and duration rules
- [Notifications] Send alerts to TA team for new availability

### Glossary
- [Availability Plan] Scheduled time slot when a panel member is available for interviews
- [Time Slot] Specific time range within a day
- [Business Hours] Official working hours (9 AM - 6 PM)
- [Panel Member] Member qualified to conduct interviews
- [TA Team] Talent Acquisition team that manages interview scheduling
