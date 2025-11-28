# Issue Template: Retrieve All Members (FR#MAP-3)

### User Story
See: `Requirement/Userstories/1#MemberManagement/FR#MAP-3-List-Members.md`

### Functional Requirement
See: `Requirement/Functional/V1/1#MemberManagement/FR#MAP-3 – Retrieve All Members with Pagination.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: GET /members (with pagination)
- [ ] Handler: GetMembersPagedQueryHandler
- [ ] Repository: GetMembersPagedAsync
- [ ] Entity: Member
- [ ] SQL: GetMembersPaged.sql

---

> Use this issue as a checklist and reference for all code and PRs related to retrieving all members with pagination.
