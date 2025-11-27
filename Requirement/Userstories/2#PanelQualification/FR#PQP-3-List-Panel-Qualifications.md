FR#PQP-3 - List Panel Qualifications

Title: View a member's panel qualifications

Description:
As an authorized user, I want to view a member's panel qualifications, so that I can understand their interview capabilities.

Business Scope:
- List all panel qualifications for a member, with optional filtering by active status.
- Only authorized roles can access panel qualification data.

Acceptance Criteria:
- Given I am authorized and provide a valid member ID When I request panel qualifications Then I receive a list of the member's qualifications.
- Given the member does not exist When I request panel qualifications Then I am informed the member cannot be found.
- Given I am not authorized When I request panel qualifications Then I am not allowed to view the data.

Business Rules & Constraints (from FR):
- Only authorized roles can access panel qualification data.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#PQP-3 (FR#PQP-3 – List Panel Qualifications.md)
