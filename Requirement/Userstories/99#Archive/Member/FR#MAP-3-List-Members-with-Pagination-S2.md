FR#MAP-3 - List Members with Pagination (Practice Admin)

Title: Retrieve members in my practice with pagination

Description:
As a Practice Admin, I want to view a paginated list of members within my practice, so that I can manage staffing in my scope.

Business Scope:
- Retrieve members within my practice with pagination and optional filters consistent with policy.

Acceptance Criteria:
- Given I am a Practice Admin and request a page When I view the list Then I receive members from my practice with page information and counts.
- Given I request an out-of-range page When I view the list Then I receive an empty page with correct metadata or guidance.
- Given I provide invalid page size or sorting fields When I view the list Then the request is rejected.

Business Rules & Constraints (from FR):
- Results include pagination metadata and honor allowed sort fields; visibility is restricted to my practice.

Out of Scope:
- Implementation details, UI/UX; export is not included.

Traceability:
- Source FR: FR#MAP-3 (FR#MAP-3 – Retrieve All Members with Pagination.md)
