FR#MAP-1 - Onboard Member (Practice Admin)

Title: Onboard a member within my practice

Description:
As a Practice Admin, I want to onboard a member within my practice, so that staffing and access are enabled for my practice scope.

Business Scope:
- Onboard new members with practice-scoped permissions.
- Allowed target roles: Practice Admin (same practice), Tech Team Panel Member, TA Team Admin (same practice).
- Practice scoping is enforced; cross-practice onboarding is not allowed.

Acceptance Criteria:
- Given I am a Practice Admin and provide valid details in my practice When I onboard a member Then the member is created and an identifying reference is provided.
- Given the username or email already exists When I onboard a member Then the request is rejected for duplication.
- Given I select a role or practice outside my scope When I onboard a member Then I am not allowed to proceed.
- Given the practice is inactive or invalid When I onboard a member Then the request is rejected.

Business Rules & Constraints (from FR):
- Uniqueness of UserName/EmailAddress; email domain policy applies.
- Practice Admin can only act within their own practice; cannot onboard Master Admins.
- Audit details must be recorded; password is generated per policy.

Out of Scope:
- Implementation details, UI/UX, email template design.

Traceability:
- Source FR: FR#MAP-1 (FR#MAP-1 – Onboard Members to the System by Master Admin and Practice Admin.md)
