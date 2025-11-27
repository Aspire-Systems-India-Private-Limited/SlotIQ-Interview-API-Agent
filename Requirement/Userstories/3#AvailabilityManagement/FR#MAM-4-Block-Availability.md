FR#MAM-4 - Block Availability

Title: Block an available interview slot

Description:
As a TA Team Admin, I want to block an available interview slot, so that I can reserve it for potential interview scheduling and prevent other TA Team Admins from booking it.

Business Scope:
- Block available slots for potential interviews
- Only TA Team Admin can block slots within their practice
- Once blocked, only the blocking TA Team Admin can modify it

Acceptance Criteria:
- Given I am a TA Team Admin and the slot is available When I block it Then its status changes to "Blocked" and only I can modify it
- Given the slot is not in "Available" status When I try to block it Then the request is rejected
- Given I am not the TA Team Admin for this practice When I try to block a slot Then I am not allowed to proceed
- Given another TA Team Admin tries to modify my blocked slot When they attempt changes Then they are not allowed to proceed

Business Rules & Constraints (from FR):
- Only TA Team Admins can block slots
- Slot must be in "Available" status to be blocked
- Only the blocking TA Team Admin can modify blocked slots
- Audit trail must be maintained

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#MAM-4 (FR#MAM-4 – Block Availability Slot.md)
