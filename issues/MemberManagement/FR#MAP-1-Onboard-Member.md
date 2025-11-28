# Issue Template: Member Onboarding (FR#MAP-1)

### User Story
As a Master Admin or Practice Admin, I want to onboard new members to the system so that they can access platform features.

### Functional Requirement
See: `Requirement/Functional/V1/1#MemberManagement/FR#MAP-1 – Onboard Members to the System by Master Admin and Practice Admin.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Coding Standards & References
- Follow all architecture and coding standards in `.github/copilot-instructions.md`
- See layer-specific instructions in `.github/Instructions/`
    - [API Layer](../../.github/Instructions/api.instructions.md)
    - [Logic Layer](../../.github/Instructions/logic.instructions.md)
    - [Data Layer](../../.github/Instructions/data.instructions.md)
    - [Unit Testing](../../.github/Instructions/unittest.instructions.md)
    - [Database](../../.github/Instructions/database.instructions.md)

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: POST /members
- [ ] Handler: CreateMemberCommandHandler
- [ ] Repository: AddMemberAsync
- [ ] Entity: Member
- [ ] SQL: InsertMember.sql

---

> Use this issue as a checklist and reference for all code and PRs related to Member Onboarding.