FR#MAP-6 - Search Members (Master Admin)

Title: Search members with filters and pagination

Description:
As a Master Admin, I want to search members using filters and free-text queries, so that I can find relevant members efficiently.

Business Scope:
- Search across practices with supported filters and sorting; results are paginated.

Acceptance Criteria:
- Given I am a Master Admin and provide valid filters When I search Then I receive a paginated list of matching members with counts.
- Given I provide invalid filter values or sort fields When I search Then the request is rejected.
- Given there are no matches When I search Then I receive an empty result with correct pagination metadata.

Business Rules & Constraints (from FR):
- Allowed filters and sort fields are defined by policy; visibility aligns with my role.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#MAP-6 (FR#MAP-6 – Search Members with Filters.md)
