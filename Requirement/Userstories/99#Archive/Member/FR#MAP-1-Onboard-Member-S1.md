FR#MAP-1 - Onboard Member (Master Admin)

Title: Onboard a member across practices

Description:
As a Master Admin, I want to onboard a member, so that the organization can grant access and assign responsibilities without practice limitations.

Business Scope:
- Onboard new members with role and optional practice assignment as allowed to Master Admin.
- Allowed target roles: Master Admin, Practice Admin, Tech Team Panel Member, TA Team Admin.
- Master Admin is not bound to a practice for onboarding.

Acceptance Criteria:
- Given I am a Master Admin and provide valid details When I onboard a member Then the member is created and an identifying reference is provided.
- Given the username already exists When I onboard a member Then the request is rejected for duplication.
- Given the email domain is not permitted by policy When I onboard a member Then the request is rejected.
- Given the target role or practice is invalid When I onboard a member Then I am informed the selection is not allowed.
- Given I am not authenticated or authorized as Master Admin When I attempt onboarding Then I am not allowed to proceed.

Business Rules & Constraints (from FR):
- UserName and EmailAddress must be unique; EmailAddress must be in the approved domain.
- Role determines permissions after onboarding; practice assignment is mandatory where applicable.
- Audit details must be recorded; a welcome communication may be sent per organizational policy.

Out of Scope:
- Implementation details, UI/UX, email template design.

Traceability:
- Source FR: FR#MAP-1 (FR#MAP-1 – Onboard Members to the System by Master Admin and Practice Admin.md)
