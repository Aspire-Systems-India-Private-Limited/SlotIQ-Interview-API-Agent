FR#MAP-2 - Update Member

Title: Update member details

Description:
As an authorized admin, I want to update a member's details, so that member information remains accurate and up to date.

Business Scope:
- Update member details such as name, email, phone, role, and practice assignment.
- Only authorized roles (Master Admin, Practice Admin) can update members within their permitted scope.
- Changes are tracked for audit purposes.

Acceptance Criteria:
- Given I am authorized and provide valid updates When I update a member Then the changes are saved and confirmed.
- Given the member does not exist When I attempt to update Then I am informed the member cannot be found.
- Given the new email or username already exists When I update a member Then the request is rejected for duplication.
- Given I am not authorized to update the member When I attempt to update Then I am not allowed to proceed.

Business Rules & Constraints (from FR):
- Email and username must remain unique and valid.
- Only permitted fields can be updated; audit details must be recorded.

Out of Scope:
- Implementation details, UI/UX, email template design.

Traceability:
- Source FR: FR#MAP-2 (FR#MAP-2 – Update Member Details.md)
