FR#MAM-1 - Add Availability Plan

Title: Create an availability plan for interviews

Description:
As a Panel Member, I want to add my availability slots for interviews, so that TA Team can schedule interviews during those times.

Business Scope:
- Schedule interview slots within business hours (9 AM - 6 PM)
- Maximum 8 hours availability per day
- All new slots are created with "Available" status
- Only authorized roles can add slots for their scope

Acceptance Criteria:
- Given I am authorized and provide valid slot details When I add an availability plan Then the plan is created with "Available" status
- Given I try to create overlapping slots When I add a plan Then the request is rejected
- Given I exceed 8 hours per day When I add a plan Then the request is rejected
- Given I select non-business hours When I add a plan Then the request is rejected
- Given I select a past date When I add a plan Then the request is rejected

Business Rules & Constraints (from FR):
- Initial status must be "Available"
- Slots must be in 60-minute increments
- No overlapping slots allowed
- Maximum 8 hours per day
- Only future dates are valid
- Audit trail must be maintained

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#MAM-1 (FR#MAM-1 – Availability Planning.md)
