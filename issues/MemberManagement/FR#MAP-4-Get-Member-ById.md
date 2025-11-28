# Issue Template: Retrieve Selected Member Details (FR#MAP-4)

### User Story
See: `Requirement/Userstories/1#MemberManagement/FR#MAP-4-Get-Member-ById.md`

### Functional Requirement
See: `Requirement/Functional/V1/1#MemberManagement/FR#MAP-4 – Retrieve Selected Member Details.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: GET /members/{id}
- [ ] Handler: GetMemberByIdQueryHandler
- [ ] Repository: GetMemberByIdAsync
- [ ] Entity: Member
- [ ] SQL: GetMemberById.sql

---

> Use this issue as a checklist and reference for all code and PRs related to retrieving selected member details.
