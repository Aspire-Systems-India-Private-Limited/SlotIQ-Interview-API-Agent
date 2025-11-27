FR#AMP-2 - Update Availability Plan

Title: Update a member's availability plan

Description:
As an authorized member, I want to update my availability plan, so that my interview availability remains accurate and up to date.

Business Scope:
- Update date, time slot, or status for an existing availability plan.
- Only authorized members can update their own availability plans.
- Changes are tracked for audit purposes.

Acceptance Criteria:
- Given I am authorized and provide valid updates When I update an availability plan Then the changes are saved and confirmed.
- Given the availability plan does not exist When I attempt to update Then I am informed it cannot be found.
- Given the updated plan would duplicate an existing one for the member When I update Then the request is rejected for duplication.
- Given I am not authorized to update availability plans When I attempt to update Then I am not allowed to proceed.

Business Rules & Constraints (from FR):
- Each availability plan must remain unique for a member.
- Audit details must be recorded.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#AMP-2 (FR#AMP-2 – Update Availability Plan.md)
