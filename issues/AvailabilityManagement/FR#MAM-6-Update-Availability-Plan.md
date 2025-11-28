# Issue Template: Update Availability Plan (FR#MAM-6)

### User Story
See: `Requirement/Userstories/3#AvailabilityManagement/FR#MAM-6-Update-Availability-Plan.md`

### Functional Requirement
See: `Requirement/Functional/V1/3#AvailabilityManagement/FR#MAM-6 – Availability Update.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: PUT /availability-plans/{id}
- [ ] Handler: UpdateAvailabilityPlanCommandHandler
- [ ] Repository: UpdateAvailabilityPlanAsync
- [ ] Entity: AvailabilityPlan
- [ ] SQL: UpdateAvailabilityPlan.sql

---

> Use this issue as a checklist and reference for all code and PRs related to updating availability plans.
