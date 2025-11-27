FR#MAM-7 - Book Availability

Title: Book a blocked interview slot

Description:
As a TA Team Admin, I want to book a blocked interview slot, so that I can confirm it for an interview and prevent any further modifications.

Business Scope:
- Book previously blocked slots for confirmed interviews
- Only the TA Team Admin who blocked the slot can book it
- Once booked, slot is locked from modifications except by booking TA Team Admin

Acceptance Criteria:
- Given I am the blocking TA Team Admin and the slot is blocked When I book it Then its status changes to "Booked" and is locked
- Given the slot is not in "Blocked" status When I try to book it Then the request is rejected
- Given I am not the TA Team Admin who blocked the slot When I try to book it Then I am not allowed to proceed
- Given any other user tries to modify a booked slot When they attempt changes Then they are not allowed to proceed

Business Rules & Constraints (from FR):
- Only the blocking TA Team Admin can book slots
- Slot must be in "Blocked" status to be booked
- Only the booking TA Team Admin can modify booked slots
- Audit trail must be maintained

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#MAM-7 (FR#MAM-7 – Book Availability Slot.md)
