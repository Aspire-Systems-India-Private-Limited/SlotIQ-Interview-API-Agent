FR#MAP-2 - Modify Member Details (Practice Admin)

Title: Modify member details within my practice

Description:
As a Practice Admin, I want to modify member details within my practice, so that our practice records remain accurate.

Business Scope:
- Update permissible fields for members within my practice.
- Cannot modify Master Admins or members outside my practice.

Acceptance Criteria:
- Given I am a Practice Admin and provide valid changes for a member in my practice When I update Then permitted fields are updated and the change is confirmed.
- Given I attempt to change an immutable field (e.g., UserName, MemberID) When I update Then the request is rejected.
- Given I attempt to move a member to another practice When I update Then I am not allowed to proceed.
- Given the new email or phone conflicts with an existing one When I update Then the request is rejected for duplication.

Business Rules & Constraints (from FR):
- Practice-scoped permissions; uniqueness rules apply.
- Audit of modifier, timestamp, and changed fields is required.

Out of Scope:
- Implementation details, UI/UX; deactivation is covered separately.

Traceability:
- Source FR: FR#MAP-2 (FR#MAP-2 – Modify Member Details by Master Admin and Practice Admin.md)
