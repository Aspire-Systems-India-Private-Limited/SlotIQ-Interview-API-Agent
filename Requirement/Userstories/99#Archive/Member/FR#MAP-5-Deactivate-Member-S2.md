FR#MAP-5 - Deactivate Member (Practice Admin)

Title: Deactivate a member in my practice

Description:
As a Practice Admin, I want to deactivate a member in my practice, so that access is revoked while keeping the record for compliance.

Business Scope:
- Set a member’s status to inactive for a member in my practice and record the reason.

Acceptance Criteria:
- Given I am a Practice Admin and provide a valid member reference in my practice When I deactivate Then the member becomes inactive and the change is recorded.
- Given the target is a Master Admin or outside my practice When I deactivate Then I am not allowed to proceed.
- Given the member is already inactive or not found When I deactivate Then I am informed that deactivation cannot proceed.

Business Rules & Constraints (from FR):
- Practice-scoped permissions; soft deactivation with audit.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#MAP-5 (FR#MAP-5 – Deactivate Member.md)
