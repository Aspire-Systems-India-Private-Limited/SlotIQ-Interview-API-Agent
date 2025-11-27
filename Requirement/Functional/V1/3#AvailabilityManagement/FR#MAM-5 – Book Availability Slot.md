## FR#MAM-5 – Book Availability Slot

### Description
System shall allow TA Team Admins to book a blocked availability slot for a panel member when an interview is confirmed, and restrict further modifications to the slot except by the booking TA Team Admin.

### User Roles
- TA Team Admin

---
### Personas and Permissions
- [TA Team Admin] Can book a slot that is currently in "Blocked" status and only if they are the one who blocked it.
- [Master Admin, Practice Admin, Tech Team Panel Member] Cannot book slots.

---
### Preconditions and Postconditions
- [Preconditions]
    - Availability plan exists, is active, and status is "Blocked"
    - Initiator is the same TA Team Admin who blocked the slot
    - Slot is not already booked or cancelled

- [Postconditions]
    - Availability plan status is set to "Booked"
    - Only the booking TA Team Admin can cancel or revert the slot
    - Audit fields are updated (ModifiedDate, ModifiedBy, Source)
    - Notification sent to the panel member and candidate (if required)

### Module Name
- Availability Management

---
### Fields level Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`AvailabilityID` | Unique identifier | UUID | Must exist | Yes | None | "550e8400-e29b-41d4-a716-446655440000"
`Status` | Slot status | Enum | Must be "Blocked" before booking | Yes | "Booked" | "Booked"
`ModifiedBy` | Modifier | String | Valid TA Team Admin user ID | Yes | None | "taadmin123"
`Source` | Application source | Enum | Valid SourceEnum | Yes | None | "Web"

---
#### System-Level Business Rules
- Only the TA Team Admin who blocked the slot can book it
- Slot must be in "Blocked" status to be booked
- Once booked, only the booking TA Team Admin can cancel or revert the slot
- Booked slots are protected from modification by other roles
- Audit trail must record who booked the slot and when

---

#### API Level Business Rules
Result Name | ResultType | Response and Actions | Example | Should be logged | Response Category
------------|------------|---------------------|---------|-----------------|------------------
AVAILABILITY_BOOK_SUCCESS | Success | Returns AvailabilityID and success message | {"availabilityID": "...", "successCode": "AVAILABILITY_BOOK_SUCCESS", "successMessage": "Availability slot booked successfully"} | No | Informational
AVAILABILITY_BOOK_FAILURE | Failure | Returns error code and message | {"errorCode": "AVAILABILITY_BOOK_FAILURE", "errorMessage": "Failed to book availability slot"} | Yes | Error
UNAUTHORIZED_ERROR | Failure | Returns error code and message | {"errorCode": "UNAUTHORIZED_ERROR", "errorMessage": "You are not authorized to perform this operation"} | Yes | Error
INVALID_STATUS_ERROR | Failure | Returns error code and message | {"errorCode": "INVALID_STATUS_ERROR", "errorMessage": "Slot is not blocked for booking"} | Yes | Error

### Security and Compliances
- [Access Control] Only the blocking TA Team Admin can book the slot
- [Audit Trail] Record all booking actions
- [Data Integrity] Prevent status changes by unauthorized users

### Glossary
- [Booked] Status indicating a slot is confirmed for an interview and locked for further changes except by the booking TA Team Admin
