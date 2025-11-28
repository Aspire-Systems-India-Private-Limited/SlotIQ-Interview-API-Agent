# Issue Template: Deactivate Panel Qualification (FR#PQM-4)

### User Story
See: `Requirement/Userstories/2#PanelQualification/FR#PQP-4-Deactivate-Panel-Qualification.md`

### Functional Requirement
See: `Requirement/Functional/V1/2#PanelQualification/FR#PQM-4 – Panel Qualification Deactivation.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: DELETE /panel-qualifications/{id}
- [ ] Handler: DeactivatePanelQualificationCommandHandler
- [ ] Repository: DeactivatePanelQualificationAsync (soft delete)
- [ ] Entity: PanelQualification
- [ ] SQL: DeactivatePanelQualification.sql

---

> Use this issue as a checklist and reference for all code and PRs related to deactivating a panel qualification.
