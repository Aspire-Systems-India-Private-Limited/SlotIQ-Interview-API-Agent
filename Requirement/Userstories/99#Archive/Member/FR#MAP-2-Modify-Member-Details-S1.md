FR#MAP-2 - Modify Member Details (Master Admin)

Title: Modify member details across practices

Description:
As a Master Admin, I want to modify member details, so that records remain accurate across the organization.

Business Scope:
- Update permissible fields for any member across practices.
- Immutable: UserName, MemberID; IsActive is handled by a separate deactivation process.

Acceptance Criteria:
- Given I am a Master Admin and provide valid changes When I update a member Then permitted fields are updated and the change is confirmed.
- Given I attempt to change an immutable field When I update a member Then the request is rejected.
- Given a new email or phone conflicts with an existing one When I update a member Then the request is rejected for duplication.
- Given the target member does not exist or is out of scope When I update a member Then I am informed the member cannot be found.

Business Rules & Constraints (from FR):
- Changes must respect role and practice rules; uniqueness constraints remain enforced.
- Audit details must be recorded for who changed what and when.

Out of Scope:
- Implementation details, UI/UX; password resets are not part of this story.

Traceability:
- Source FR: FR#MAP-2 (FR#MAP-2 – Modify Member Details by Master Admin and Practice Admin.md)
