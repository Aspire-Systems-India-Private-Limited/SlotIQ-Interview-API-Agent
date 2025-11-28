# Issue Template: Deactivate Member (FR#MAP-5)

### User Story
See: `Requirement/Userstories/1#MemberManagement/FR#MAP-5-Deactivate-Member.md`

### Functional Requirement
See: `Requirement/Functional/V1/1#MemberManagement/FR#MAP-5 – Deactivate Member.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: DELETE /members/{id}
- [ ] Handler: DeactivateMemberCommandHandler
- [ ] Repository: DeactivateMemberAsync (soft delete)
- [ ] Entity: Member
- [ ] SQL: DeactivateMember.sql

---

> Use this issue as a checklist and reference for all code and PRs related to deactivating a member.
