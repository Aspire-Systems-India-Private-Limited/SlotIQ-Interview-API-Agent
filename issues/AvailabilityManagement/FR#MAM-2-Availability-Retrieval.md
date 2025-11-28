# Issue Template: Availability Retrieval (FR#MAM-2)

### User Story
See: `Requirement/Userstories/3#AvailabilityManagement/FR#MAM-2-Availability-Retrieval.md`

### Functional Requirement
See: `Requirement/Functional/V1/3#AvailabilityManagement/FR#MAM-2 – Availability Retrieval.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: GET /availability-plans/{id}
- [ ] Handler: GetAvailabilityPlanByIdQueryHandler
- [ ] Repository: GetAvailabilityPlanByIdAsync
- [ ] Entity: AvailabilityPlan
- [ ] SQL: GetAvailabilityPlanById.sql

---

> Use this issue as a checklist and reference for all code and PRs related to retrieving availability plans.
