FR#MAP-4 - Get Member Details (Practice Admin)

Title: View member details in my practice

Description:
As a Practice Admin, I want to view a member’s details within my practice, so that I can manage staffing and permissions in my scope.

Business Scope:
- Retrieve key attributes of a member within my practice, if permitted by role.

Acceptance Criteria:
- Given I am a Practice Admin and provide a valid reference in my practice When I view the member Then I see the member’s details.
- Given the member is outside my practice When I view the member Then I am not allowed to view details.
- Given the member does not exist When I view the member Then I am informed that the member cannot be found.

Business Rules & Constraints (from FR):
- Show only necessary attributes; visibility restricted to practice scope.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#MAP-4 (FR#MAP-4 – Retrieve Selected Member Details.md)
