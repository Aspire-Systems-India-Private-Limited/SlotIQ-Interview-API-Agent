FR#MAP-3 - List Members

Title: View and filter members

Description:
As an authorized user, I want to view and filter the list of members, so that I can find and manage members efficiently.

Business Scope:
- List members with pagination, sorting, and filtering by role, practice, and status.
- Only authorized roles can access the member list.

Acceptance Criteria:
- Given I am authorized When I request the member list Then I receive a paginated, filterable list of members.
- Given I apply filters (role, practice, status) When I request the list Then only matching members are shown.
- Given I am not authorized When I request the list Then I am not allowed to view members.

Business Rules & Constraints (from FR):
- Only authorized roles can access member data.
- Pagination and sorting options must be available.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#MAP-3 (FR#MAP-3 – List Members with Filters and Pagination.md)
