FR#PQP-2 - Update Panel Qualification

Title: Update a member's panel qualification

Description:
As an authorized admin, I want to update a member's panel qualification, so that the qualification details remain accurate and up to date.

Business Scope:
- Update recruitment position or applicable level for an existing panel qualification.
- Only authorized roles can update panel qualifications.
- Changes are tracked for audit purposes.

Acceptance Criteria:
- Given I am authorized and provide valid updates When I update a panel qualification Then the changes are saved and confirmed.
- Given the panel qualification does not exist When I attempt to update Then I am informed it cannot be found.
- Given the updated qualification would duplicate an existing one for the member When I update Then the request is rejected for duplication.
- Given I am not authorized to update panel qualifications When I attempt to update Then I am not allowed to proceed.

Business Rules & Constraints (from FR):
- Each panel qualification must remain unique for a member.
- Audit details must be recorded.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#PQP-2 (FR#PQP-2 – Update Panel Qualification.md)
