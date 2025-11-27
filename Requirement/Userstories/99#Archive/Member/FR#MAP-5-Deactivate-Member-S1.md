FR#MAP-5 - Deactivate Member (Master Admin)

Title: Deactivate a member

Description:
As a Master Admin, I want to deactivate a member, so that access is revoked without deleting the record.

Business Scope:
- Set a member’s status to inactive and record the reason.

Acceptance Criteria:
- Given I am a Master Admin and provide a valid member reference When I deactivate Then the member becomes inactive and the change is recorded.
- Given the member is already inactive or not found When I deactivate Then I am informed that deactivation cannot proceed.
- Given I attempt to deactivate the last remaining Master Admin When I deactivate Then I am not allowed to proceed.

Business Rules & Constraints (from FR):
- Deactivation is soft; audit must include actor, timestamp, and reason.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#MAP-5 (FR#MAP-5 – Deactivate Member.md)
