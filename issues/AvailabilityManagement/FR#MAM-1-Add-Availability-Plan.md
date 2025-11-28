# Issue Template: Add Availability Plan (FR#MAM-1)

### User Story
See: `Requirement/Userstories/3#AvailabilityManagement/FR#MAM-1-Add-Availability-Plan.md`

### Functional Requirement
See: `Requirement/Functional/V1/3#AvailabilityManagement/FR#MAM-1 – Availability Planning.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: POST /availability-plans
- [ ] Handler: AddAvailabilityPlanCommandHandler
- [ ] Repository: AddAvailabilityPlanAsync
- [ ] Entity: AvailabilityPlan
- [ ] SQL: InsertAvailabilityPlan.sql

---

> Use this issue as a checklist and reference for all code and PRs related to adding an availability plan.
