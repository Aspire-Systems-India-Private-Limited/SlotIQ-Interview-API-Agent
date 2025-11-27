FR#MAP-1 - Onboard Member (Practice Admin)

Title: Onboard a member within a practice

Description:
As a Practice Admin, I want to onboard a member to my practice, so that the practice can assign responsibilities and grant access within its scope.

Business Scope:
- Onboard new members with roles allowed to Practice Admin.
- Allowed target roles: Practice Admin, Tech Team Panel Member, TA Team Admin.
- Practice Admin can only onboard members to their own practice.

Acceptance Criteria:
- Given I am a Practice Admin and provide valid details for my practice When I onboard a member Then the member is created and an identifying reference is provided.
- Given the username already exists When I onboard a member Then the request is rejected for duplication.
- Given the email domain is not permitted by policy When I onboard a member Then the request is rejected.
- Given the target role or practice is invalid or outside my scope When I onboard a member Then I am informed the selection is not allowed.
- Given I am not authenticated or authorized as Practice Admin When I attempt onboarding Then I am not allowed to proceed.

Business Rules & Constraints (from FR):
- UserName and EmailAddress must be unique; EmailAddress must be in the approved domain.
- Role determines permissions after onboarding; practice assignment is mandatory.
- Audit details must be recorded; a welcome communication may be sent per organizational policy.

Out of Scope:
- Implementation details, UI/UX, email template design.

Traceability:
- Source FR: FR#MAP-1 (FR#MAP-1 – Onboard Members to the System by Master Admin and Practice Admin.md)
