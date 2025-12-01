# GitHub Issue: Enforce Authentication and Role-Based Authorization (FR#AUTH-1)

## Summary
Implement and enforce authentication and role-based authorization for all API endpoints as per FR#AUTH-1. Ensure only authenticated users with the correct roles (e.g., MasterAdmin, PracticeAdmin, Member) can access relevant endpoints.

## References
- User Story: `Requirement/Userstories/Authentication/FR#AUTH-1-Member-Authentication-and-Login.md`
- Functional Requirement: `Requirement/Functional/V1/4#Authentication/FR#AUTH-1-Member-Authentication-and-Login.md`
- OpenAPI Contract: `Requirement/Technical/Authentication/AuthenticationContracts.openapi.yaml`
- Entity Definition: `Requirement/Technical/Authentication/AuthenticationEntities.openapi.yaml`
- Coding Standards: `.github/copilot-instructions.md`, `.github/Instructions/`

## Implementation Tasks
- [ ] Configure authentication middleware (e.g., JWT Bearer) in API startup
- [ ] Add role-based authorization to all endpoints using [Authorize(Roles = "...")] or equivalent Minimal API pattern
- [ ] Ensure endpoints validate user roles as per requirements
- [ ] Add/Update tests to verify unauthorized/forbidden access is handled correctly
- [ ] Document security requirements in API docs

## Acceptance Criteria
- [ ] All endpoints require authentication
- [ ] Endpoints are accessible only to users with appropriate roles
- [ ] Unauthorized/forbidden requests return correct HTTP status codes
- [ ] Security is covered by automated tests

---

> This issue is for the coding agent to implement and validate authentication and role-based authorization across all endpoints as per FR#AUTH-1.
