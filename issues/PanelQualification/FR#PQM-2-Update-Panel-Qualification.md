# Issue Template: Update Panel Qualification (FR#PQM-2)

### User Story
See: `Requirement/Userstories/2#PanelQualification/FR#PQP-2-Update-Panel-Qualification.md`

### Functional Requirement
See: `Requirement/Functional/V1/2#PanelQualification/FR#PQM-2 – Panel Qualification Update.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: PUT /panel-qualifications/{id}
- [ ] Handler: UpdatePanelQualificationCommandHandler
- [ ] Repository: UpdatePanelQualificationAsync
- [ ] Entity: PanelQualification
- [ ] SQL: UpdatePanelQualification.sql

---

> Use this issue as a checklist and reference for all code and PRs related to updating a panel qualification.
