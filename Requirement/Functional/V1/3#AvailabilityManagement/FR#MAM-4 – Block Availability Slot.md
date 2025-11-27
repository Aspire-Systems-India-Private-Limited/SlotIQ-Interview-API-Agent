## FR#MAM-4 – Block Availability Slot

### Description
System shall allow TA Team Admins to block an available interview slot for a panel member, preventing other TA Team Admins or users from booking or modifying the slot until it is unblocked or booked.

### User Roles
- TA Team Admin

---
### Personas and Permissions
- [TA Team Admin] Can block any available slot for any panel member within their practice.
- [Master Admin, Practice Admin, Tech Team Panel Member] Cannot block slots.

---
### Preconditions and Postconditions
- [Preconditions]
    - Availability plan exists, is active, and status is "Available"
    - Initiator is a TA Team Admin with permission for the member's practice
    - Slot is not already blocked, booked, or cancelled

- [Postconditions]
    - Availability plan status is set to "Blocked"
    - Only the TA Team Admin who blocked the slot can unblock, book, or cancel it
    - Audit fields are updated (ModifiedDate, ModifiedBy, Source)
    - Notification sent to the panel member (if required)

### Module Name
- Availability Management

---
### Fields level Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`AvailabilityID` | Unique identifier | UUID | Must exist | Yes | None | "550e8400-e29b-41d4-a716-446655440000"
`Status` | Slot status | Enum | Must be "Available" before block | Yes | "Blocked" | "Blocked"
`ModifiedBy` | Modifier | String | Valid TA Team Admin user ID | Yes | None | "taadmin123"
`Source` | Application source | Enum | Valid SourceEnum | Yes | None | "Web"

---
#### System-Level Business Rules
- Only TA Team Admin can block a slot
- Slot must be in "Available" status to be blocked
- Once blocked, only the blocking TA Team Admin can change status (to Booked, Available, or Cancelled)
- Blocked slots are protected from modification by other roles
- Audit trail must record who blocked the slot and when

---

#### API Level Business Rules
Result Name | ResultType | Response and Actions | Example | Should be logged | Response Category
------------|------------|---------------------|---------|-----------------|------------------
AVAILABILITY_BLOCK_SUCCESS | Success | Returns AvailabilityID and success message | {"availabilityID": "...", "successCode": "AVAILABILITY_BLOCK_SUCCESS", "successMessage": "Availability slot blocked successfully"} | No | Informational
AVAILABILITY_BLOCK_FAILURE | Failure | Returns error code and message | {"errorCode": "AVAILABILITY_BLOCK_FAILURE", "errorMessage": "Failed to block availability slot"} | Yes | Error
UNAUTHORIZED_ERROR | Failure | Returns error code and message | {"errorCode": "UNAUTHORIZED_ERROR", "errorMessage": "You are not authorized to perform this operation"} | Yes | Error
INVALID_STATUS_ERROR | Failure | Returns error code and message | {"errorCode": "INVALID_STATUS_ERROR", "errorMessage": "Slot is not available for blocking"} | Yes | Error

### Security and Compliances
- [Access Control] Only TA Team Admin can block slots
- [Audit Trail] Record all block actions
- [Data Integrity] Prevent status changes by unauthorized users

### Glossary
- [Blocked] Status indicating a slot is reserved by TA Team Admin and not available for booking or modification by others
