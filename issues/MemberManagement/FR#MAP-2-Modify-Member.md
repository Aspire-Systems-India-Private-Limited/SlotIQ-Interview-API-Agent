# Issue Template: Modify Member Details (FR#MAP-2)

### User Story
See: `Requirement/Userstories/1#MemberManagement/FR#MAP-2-Update-Member.md`

### Functional Requirement
See: `Requirement/Functional/V1/1#MemberManagement/FR#MAP-2 – Modify Member Details by Master Admin and Practice Admin.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: PUT /members/{id}
- [ ] Handler: UpdateMemberCommandHandler
- [ ] Repository: UpdateMemberAsync
- [ ] Entity: Member
- [ ] SQL: UpdateMember.sql

---

> Use this issue as a checklist and reference for all code and PRs related to modifying member details.
