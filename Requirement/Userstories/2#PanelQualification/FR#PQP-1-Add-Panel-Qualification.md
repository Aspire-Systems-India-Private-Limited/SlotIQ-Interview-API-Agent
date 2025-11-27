FR#PQP-1 - Add Panel Qualification

Title: Add a panel qualification to a member

Description:
As an authorized admin, I want to add a panel qualification to a member, so that the member can participate in interviews for specific positions and levels.

Business Scope:
- Add a new panel qualification specifying recruitment position and applicable level for a member.
- Only authorized roles can add panel qualifications.
- Each qualification is associated with a member and is tracked for audit.

Acceptance Criteria:
- Given I am authorized and provide valid details When I add a panel qualification Then the qualification is added and an identifying reference is provided.
- Given the member does not exist When I attempt to add a qualification Then I am informed the member cannot be found.
- Given the same qualification already exists for the member When I attempt to add Then the request is rejected for duplication.
- Given I am not authorized to add panel qualifications When I attempt to add Then I am not allowed to proceed.

Business Rules & Constraints (from FR):
- Each panel qualification must be unique for a member (combination of position and level).
- Audit details must be recorded.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#PQP-1 (FR#PQP-1 – Add Panel Qualification.md)
