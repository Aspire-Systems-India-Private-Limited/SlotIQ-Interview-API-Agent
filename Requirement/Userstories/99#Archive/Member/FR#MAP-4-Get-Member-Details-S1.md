FR#MAP-4 - Get Member Details (Master Admin)

Title: View member details

Description:
As a Master Admin, I want to view a member’s details, so that I can understand their attributes and role across the organization.

Business Scope:
- Retrieve key attributes of a member, if permitted by role.

Acceptance Criteria:
- Given I am a Master Admin and provide a valid reference When I view the member Then I see the member’s details.
- Given the member does not exist When I view the member Then I am informed that the member cannot be found.
- Given I provide an invalid reference When I view the member Then I am informed that the reference is invalid.

Business Rules & Constraints (from FR):
- Show only necessary attributes; sensitive fields are not exposed.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#MAP-4 (FR#MAP-4 – Retrieve Selected Member Details.md)
