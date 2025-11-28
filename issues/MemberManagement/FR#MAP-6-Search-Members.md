# Issue Template: Search Members (FR#MAP-6)

### User Story
See: `Requirement/Userstories/1#MemberManagement/FR#MAP-6-Search-Members.md`

### Functional Requirement
See: `Requirement/Functional/V1/1#MemberManagement/FR#MAP-6 – Search Members with Filters.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Coding Standards & References
- Follow all architecture and coding standards in `.github/copilot-instructions.md`
- See layer-specific instructions in `.github/Instructions/`
    - [API Layer](../../.github/Instructions/api.instructions.md)
    - [Logic Layer](../../.github/Instructions/logic.instructions.md)
    - [Data Layer](../../.github/Instructions/data.instructions.md)
    - [Unit Testing](../../.github/Instructions/unittest.instructions.md)
    - [Database](../../.github/Instructions/database.instructions.md)

### Acceptance Criteria
- [ ] Endpoint: GET /members/search?filter=...
- [ ] Handler: SearchMembersQueryHandler
- [ ] Repository: SearchMembersAsync
- [ ] Entity: Member
- [ ] SQL: SearchMembers.sql

---

> Use this issue as a checklist and reference for all code and PRs related to searching members with filters.
