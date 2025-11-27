FR#MAM-3 - Availability Cancellation

Title: Cancel an interview availability plan

Description:
As a Panel Member or Admin, I want to cancel my availability plan, so that I can remove slots that are no longer available for interviews while maintaining an audit trail.

Business Scope:
- Cancel availability plans with proper documentation
- Support different cancellation permissions by role:
  - Master Admin: Can cancel any slot across practices
  - Practice Admin: Can cancel slots within their practice
  - Tech Team Panel Member: Can cancel only their own slots
  - TA Team Admin: Cannot cancel slots

Acceptance Criteria:
- Given I am authorized and provide a valid reason When I cancel an availability plan Then the slot is marked as cancelled and an audit entry is created
- Given the slot is already cancelled When I try to cancel it Then the request is rejected
- Given the slot is in past dates When I try to cancel it Then the request is rejected
- Given the slot has active bookings When I try to cancel it Then I am informed about the conflict
- Given I am not authorized for the member's practice When I try to cancel their slot Then I am not allowed to proceed

Business Rules & Constraints (from FR):
- Must provide a cancellation reason (min 5 chars, max 500 chars)
- Cannot cancel already cancelled slots
- Cannot cancel past availability plans
- Must validate no active bookings exist
- Must maintain cancellation audit trail
- Role-based access control must be enforced
- Audit fields must be updated

Out of Scope:
- Implementation details, UI/UX design
- Email notification templates
- Calendar system integration

Traceability:
- Source FR: FR#MAM-4 (FR#MAM-4 – Availability Cancellation.md)
