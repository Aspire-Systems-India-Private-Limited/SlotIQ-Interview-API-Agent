# Issue Template: Member Authentication and Login (FR#AUTH-1)

### User Story
See: `Requirement/Userstories/Authentication/FR#AUTH-1-Member-Authentication-and-Login.md`

### Functional Requirement
See: `Requirement/Functional/V1/4#Authentication/FR#AUTH-1-Member-Authentication-and-Login.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/Authentication/AuthenticationContracts.openapi.yaml`
- Entity Definition: `Requirement/Technical/Authentication/AuthenticationEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: POST /auth/login
- [ ] Handler: MemberLoginCommandHandler
- [ ] Repository: ValidateMemberCredentialsAsync
- [ ] Entity: MemberAuthentication
- [ ] SQL: ValidateMemberCredentials.sql

---

> Use this issue as a checklist and reference for all code and PRs related to member authentication and login.
