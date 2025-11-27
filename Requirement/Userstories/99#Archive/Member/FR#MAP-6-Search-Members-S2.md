FR#MAP-6 - Search Members (Practice Admin)

Title: Search members in my practice with filters and pagination

Description:
As a Practice Admin, I want to search members within my practice using filters and free-text queries, so that I can quickly find relevant members in my scope.

Business Scope:
- Search within my practice with supported filters and sorting; results are paginated.

Acceptance Criteria:
- Given I am a Practice Admin and provide valid filters in my practice When I search Then I receive a paginated list of matching members in my practice with counts.
- Given I provide invalid filter values or sort fields When I search Then the request is rejected.
- Given there are no matches When I search Then I receive an empty result with correct pagination metadata.

Business Rules & Constraints (from FR):
- Allowed filters and sort fields are defined by policy; visibility restricted to my practice.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#MAP-6 (FR#MAP-6 – Search Members with Filters.md)
