FR#MAP-6 - Search Members

Title: Search members with filters and free-text

Description:
As an authorized user, I want to search for members using filters and free-text, so that I can quickly find relevant members.

Business Scope:
- Search members by role, practice, status, and free-text query.
- Only authorized roles can perform member searches.

Acceptance Criteria:
- Given I am authorized and provide search criteria When I search members Then I receive a list of matching members.
- Given no members match the criteria When I search Then I am informed no results are found.
- Given I am not authorized When I attempt to search Then I am not allowed to perform the search.

Business Rules & Constraints (from FR):
- Only authorized roles can access member data.
- Search supports filters and free-text.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#MAP-6 (FR#MAP-6 – Search Members.md)
