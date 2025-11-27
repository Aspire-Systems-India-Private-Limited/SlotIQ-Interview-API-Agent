FR#MAM-2 - Availability Retrieval

Title: View member availability plans with filtering

Description:
As a TA Team Admin, I want to view member availability plans with date range and status filters, so that I can find suitable interview slots for scheduling.

Business Scope:
- View availability plans for allowed members based on role permissions
- Filter plans by date range and status
- Supports all roles with appropriate scope restrictions:
  - TA Team Admin: View all plans for scheduling
  - Master Admin: View all plans across practices
  - Practice Admin: View plans within their practice
  - Tech Team Panel Member: View only their own plans

Acceptance Criteria:
- Given I am authorized and provide valid date range When I request availability plans Then I see all plans within that range for my allowed scope
- Given I provide a status filter When I request plans Then I only see plans matching that status
- Given I exceed the maximum date range (90 days) When I request plans Then I am informed to reduce the range
- Given I provide an invalid date range (end before start) When I request plans Then I am informed the range is invalid
- Given I am not authorized for a member's practice When I request their plans Then I do not see those plans in results

Business Rules & Constraints (from FR):
- Default date range: current date + 30 days if not specified
- Maximum date range span: 90 days
- Results must be sorted by date ascending, then time ascending
- Must enforce practice-based access control
- Must validate date range parameters

Out of Scope:
- Implementation details, UI/UX design
- Real-time availability updates
- Calendar integration features

Traceability:
- Source FR: FR#MAM-2 (FR#MAM-2 – Availability Retrieval.md)
