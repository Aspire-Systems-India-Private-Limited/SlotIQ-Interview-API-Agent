FR#MAP-5 - Deactivate Member

Title: Deactivate a member

Description:
As an authorized admin, I want to deactivate a member, so that access is revoked without deleting their record.

Business Scope:
- Deactivate (soft delete) a member, preserving their record for audit.
- Only authorized roles can deactivate members within their permitted scope.

Acceptance Criteria:
- Given I am authorized and provide a valid member ID When I deactivate a member Then the member is marked inactive and access is revoked.
- Given the member ID does not exist When I attempt to deactivate Then I am informed the member cannot be found.
- Given I am not authorized to deactivate the member When I attempt to deactivate Then I am not allowed to proceed.

Business Rules & Constraints (from FR):
- Deactivation is a soft delete; member data is retained.
- Audit details must be recorded.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#MAP-5 (FR#MAP-5 – Deactivate Member.md)
