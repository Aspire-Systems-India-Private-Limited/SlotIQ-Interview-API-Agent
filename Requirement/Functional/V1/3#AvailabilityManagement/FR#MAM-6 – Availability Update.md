## FR#MAM-2 – Update Availability Plan for Member Interview Slots

### Description
System shall allow updating existing availability plans for panel members to modify their scheduled interview time slots.

### User Roles
- Master Admin
- Practice Admin
- Tech Team Panel Member
- TA Team Admin

---
### Personas and Permissions
- [Master Admin] Can update availability for all members across practices
- [Practice Admin] Can update availability only for members within their practice
- [Tech Team Panel Member] Can update only their own availability
- [TA Team Admin] Can view availability but cannot modify

---
### Preconditions and Postconditions
- [Preconditions]
    - Member exists and is active
    - Availability plan exists and is in 'Available' status
    - Member has valid panel qualifications
    - Updated date range is in future
    - Updated time slots don't overlap with existing slots
    - Initiator has appropriate permissions
    - Maximum 8 hours per day restriction
    - Cannot modify slots that are already blocked or booked

- [Postconditions]
    - Availability plan is updated with new slots
    - Audit fields are updated
    - Status remains "Available"
    - Notification sent to TA Team about update
    - Old slots are properly closed
    - History of changes is maintained

### Module Name
- Availability Management

---
### Fields level Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`AvailabilityID` | Unique identifier | UUID | Must exist in system | Yes | None | "550e8400-e29b-41d4-a716-446655440000"
`MemberID` | Member reference | UUID | Must exist in Members | Yes | None | "123e4567-e89b-12d3-a456-426614174000"
`AvailableDate` | Date of availability | Date | Future date only | Yes | None | "2024-03-01"
`AvailableTimeFrom` | Start time | Time | Valid time format | Yes | None | "09:00:00"
`AvailableTimeTo` | End time | Time | Valid time format | Yes | None | "17:00:00"
`Status` | Slot status | Enum | Valid AvailabilityStatusEnum | Yes | Available | "Available"
`ModifiedBy` | Updater | String | Valid user ID | Yes | None | "admin123"
`Source` | Application source | Enum | Valid SourceEnum | Yes | None | "Web"

---
#### System-Level Business Rules
- Cannot update slots that are already blocked or booked
- `ModifiedDate` must be set to current UTC datetime
- `ModifiedBy` must be set to current user ID
- Time slots must be in 60-minute increments
- No overlapping slots allowed for same member
- Maximum 8 hours availability per day
- Slots must be within business hours (9 AM - 6 PM)
- Cannot update to past dates
- Must maintain history of changes
---

#### API Level Business Rules
Result Name | ResultType | Response and Actions | Example | Should be logged | Response Category
------------|------------|---------------------|---------|-----------------|------------------
AVAILABILITY_UPDATE_SUCCESS | Success | Returns AvailabilityID and success message | {"availabilityID": "550e8400-e29b-41d4-a716-446655440000", "successCode": "AVAILABILITY_UPDATE_SUCCESS", "successMessage": "Availability plan updated successfully"} | No | Informational
AVAILABILITY_UPDATE_FAILURE | Failure | Returns error code and message | {"errorCode": "AVAILABILITY_UPDATE_FAILURE", "errorMessage": "Failed to update availability plan"} | Yes | Error
UNAUTHORIZED_ERROR | Failure | Returns error code and message | {"errorCode": "UNAUTHORIZED_ERROR", "errorMessage": "You are not authorized to perform this operation"} | Yes | Error
FORBIDDEN_ERROR | Failure | Returns error code and message | {"errorCode": "FORBIDDEN_ERROR", "errorMessage": "Access denied for practice"} | Yes | Error
VALIDATION_ERROR | Failure | Returns error code and message | {"errorCode": "VALIDATION_ERROR", "errorMessage": "Invalid time slot"} | Yes | Informational
MEMBER_NOT_FOUND | Failure | Returns error code and message | {"errorCode": "MEMBER_NOT_FOUND", "errorMessage": "Member not found"} | Yes | Error
AVAILABILITY_NOT_FOUND | Failure | Returns error code and message | {"errorCode": "AVAILABILITY_NOT_FOUND", "errorMessage": "Availability plan not found"} | Yes | Error
SLOT_OVERLAP_ERROR | Failure | Returns error code and message | {"errorCode": "SLOT_OVERLAP_ERROR", "errorMessage": "Time slot overlaps with existing availability"} | Yes | Error
MAX_HOURS_EXCEEDED | Failure | Returns error code and message | {"errorCode": "MAX_HOURS_EXCEEDED", "errorMessage": "Maximum 8 hours per day exceeded"} | Yes | Error
INVALID_DATE_ERROR | Failure | Returns error code and message | {"errorCode": "INVALID_DATE_ERROR", "errorMessage": "Cannot update availability to past dates"} | Yes | Error
SLOT_STATUS_ERROR | Failure | Returns error code and message | {"errorCode": "SLOT_STATUS_ERROR", "errorMessage": "Cannot update blocked or booked slots"} | Yes | Error

### Security and Compliances
- [Access Control] Enforce role and practice-based access restrictions
- [Audit Trail] Record all availability updates with change history
- [Data Integrity] Prevent overlapping slots and invalid dates
- [Validation] Ensure compliance with business hours and duration rules
- [Notifications] Send alerts to TA team for availability updates
- [History] Maintain audit trail of all changes

### Glossary
- [Availability Plan] Scheduled time slot when a panel member is available for interviews
- [Time Slot] Specific time range within a day
- [Business Hours] Official working hours (9 AM - 6 PM)
- [Panel Member] Member qualified to conduct interviews
- [TA Team] Talent Acquisition team that manages interview scheduling
- [Available Status] Initial status of a time slot before being blocked or booked