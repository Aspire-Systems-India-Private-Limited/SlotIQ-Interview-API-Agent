FR#MAP-4 - Get Member By ID

Title: Retrieve member details by ID

Description:
As an authorized user, I want to retrieve a member's details by their unique ID, so that I can view their information as needed.

Business Scope:
- Retrieve member details using a unique identifier.
- Only authorized roles can access member details.

Acceptance Criteria:
- Given I am authorized and provide a valid member ID When I request details Then the member's information is returned.
- Given the member ID does not exist When I request details Then I am informed the member cannot be found.
- Given I am not authorized When I request details Then I am not allowed to view member information.

Business Rules & Constraints (from FR):
- Only authorized roles can access member data.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#MAP-4 (FR#MAP-4 – Get Member By ID.md)
