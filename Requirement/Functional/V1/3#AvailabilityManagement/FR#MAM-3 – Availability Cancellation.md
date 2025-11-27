## FR#MAM-3 – Availability Cancellation for Member Availability Plans

### Description
System shall support cancellation (soft deletion) of a member's availability plan, recording the reason and maintaining an audit trail.

### User Roles
- Master Admin
- Practice Admin
- Tech Team Panel Member

---
### Personas and Permissions
- [Master Admin] Can cancel availability for all members across practices
- [Practice Admin] Can cancel availability only for members within their practice
- [Tech Team Panel Member] Can cancel only their own availability
- [TA Team Admin] Can view availability but cannot cancel

---
### Preconditions and Postconditions
- [Preconditions]
    - Availability plan exists and is active
    - Member exists and is active
    - AvailabilityID is valid
    - Initiator has permission for the target practice or is the owner
    - Cancellation reason is provided
    - No interviews are scheduled in the slot or cancellation is allowed by policy

- [Postconditions]
    - Availability plan is marked as cancelled (status updated, not deleted)
    - Cancellation reason is recorded
    - Audit fields are updated
    - Related bookings are updated or notified as required
    - Cancellation history is logged

### Module Name
- Availability Management

---
### Fields level Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`AvailabilityID` | Availability plan identifier | UUID | Must exist | Yes | None | "550e8400-e29b-41d4-a716-446655440000"
`Reason` | Cancellation reason | String | Min 5 chars, Max 500 chars | Yes | None | "Panel member is unavailable due to emergency"
`ModifiedBy` | User cancelling the availability | String | Must be valid user ID | Yes | None | "admin123"
`Source` | Application source | Enum | Must be valid SourceEnum value | Yes | None | "Web"

---
#### System-Level Business Rules
- `ModifiedDate` must be set to current UTC datetime
- `ModifiedBy` must be set to current user ID
- `Status` must be set to "Cancelled" (AvailabilityStatusEnum)
- Cannot cancel already cancelled or past availability plans
- Must validate no active bookings exist or handle as per policy
- Must maintain cancellation audit trail

---
#### API Level Business Rules
Result Name | ResultType | Response and Actions | Example | Should be logged | Response Category
------------|------------|---------------------|---------|-----------------|------------------
AVAILABILITY_CANCEL_SUCCESS | Success | Returns AvailabilityID and success message | {"availabilityID": "550e8400-e29b-41d4-a716-446655440000", "successCode": "AVAILABILITY_CANCEL_SUCCESS", "successMessage": "Availability plan cancelled successfully"} | Yes | Informational
AVAILABILITY_CANCEL_FAILURE | Failure | Returns error code and message | {"errorCode": "AVAILABILITY_CANCEL_FAILURE", "errorMessage": "Failed to cancel availability plan"} | Yes | Error
UNAUTHORIZED_ERROR | Failure | Returns error code and message | {"errorCode": "UNAUTHORIZED_ERROR", "errorMessage": "You are not authorized to perform this operation"} | Yes | Error
FORBIDDEN_ERROR | Failure | Returns error code and message | {"errorCode": "FORBIDDEN_ERROR", "errorMessage": "Access denied for practice"} | Yes | Error
VALIDATION_ERROR | Failure | Returns error code and message | {"errorCode": "VALIDATION_ERROR", "errorMessage": "Cancellation reason is required"} | Yes | Informational
AVAILABILITY_NOT_FOUND | Failure | Returns error code and message | {"errorCode": "AVAILABILITY_NOT_FOUND", "errorMessage": "Availability plan not found"} | Yes | Error
MEMBER_NOT_FOUND | Failure | Returns error code and message | {"errorCode": "MEMBER_NOT_FOUND", "errorMessage": "Member not found"} | Yes | Error
ALREADY_CANCELLED_ERROR | Failure | Returns error code and message | {"errorCode": "ALREADY_CANCELLED_ERROR", "errorMessage": "Availability plan is already cancelled"} | Yes | Error
PAST_SLOT_ERROR | Failure | Returns error code and message | {"errorCode": "PAST_SLOT_ERROR", "errorMessage": "Cannot cancel past availability slots"} | Yes | Error
ACTIVE_BOOKING_ERROR | Failure | Returns error code and message | {"errorCode": "ACTIVE_BOOKING_ERROR", "errorMessage": "Cannot cancel: Active bookings exist"} | Yes | Error

### Security and Compliances
- [Access Control] Enforce role and practice-based access restrictions
- [Audit Trail] Record all cancellations with reason and user
- [Data Integrity] Validate related bookings and slot status
- [Validation] Ensure proper reason documentation and slot eligibility
- [Notifications] Notify affected parties if bookings are impacted

### Glossary
- [Availability Plan] Scheduled time slot when a panel member is available
- [Cancellation] Soft deletion of an availability plan, status set to "Cancelled"
- [Active Booking] Interview scheduled in the slot
- [Audit Trail] Record of all changes including who, when, and why
- [Practice] Organizational unit used for access control
