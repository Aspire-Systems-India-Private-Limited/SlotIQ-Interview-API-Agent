# Issue Template: List Panel Qualifications (FR#PQM-3)

### User Story
See: `Requirement/Userstories/2#PanelQualification/FR#PQP-3-List-Panel-Qualifications.md`

### Functional Requirement
See: `Requirement/Functional/V1/2#PanelQualification/FR#PQM-3 – Panel Qualification Listing.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: GET /panel-qualifications
- [ ] Handler: GetPanelQualificationsQueryHandler
- [ ] Repository: GetPanelQualificationsAsync
- [ ] Entity: PanelQualification
- [ ] SQL: GetPanelQualifications.sql

---

> Use this issue as a checklist and reference for all code and PRs related to listing panel qualifications.
