FR#MAP-3 - List Members with Pagination (Master Admin)

Title: Retrieve all members with pagination

Description:
As a Master Admin, I want to view a paginated list of members, so that I can browse and manage membership across the organization.

Business Scope:
- Retrieve members across practices with pagination and optional filters consistent with policy.

Acceptance Criteria:
- Given I am a Master Admin and request a page When I view the list Then I receive members with page information and counts.
- Given I request an out-of-range page When I view the list Then I receive an empty page with correct metadata or guidance.
- Given I provide invalid page size or sorting fields When I view the list Then the request is rejected.

Business Rules & Constraints (from FR):
- Results include pagination metadata and honor allowed sort fields; visibility aligns with my role.

Out of Scope:
- Implementation details, UI/UX; export is not included.

Traceability:
- Source FR: FR#MAP-3 (FR#MAP-3 – Retrieve All Members with Pagination.md)
