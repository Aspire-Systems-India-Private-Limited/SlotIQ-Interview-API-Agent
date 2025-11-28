# Issue Template: Add Panel Qualification (FR#PQM-1)

### User Story
See: `Requirement/Userstories/2#PanelQualification/FR#PQP-1-Add-Panel-Qualification.md`

### Functional Requirement
See: `Requirement/Functional/V1/2#PanelQualification/FR#PQM-1 – Panel Qualification Assignment.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: POST /panel-qualifications
- [ ] Handler: AddPanelQualificationCommandHandler
- [ ] Repository: AddPanelQualificationAsync
- [ ] Entity: PanelQualification
- [ ] SQL: InsertPanelQualification.sql

---

> Use this issue as a checklist and reference for all code and PRs related to adding a panel qualification.
