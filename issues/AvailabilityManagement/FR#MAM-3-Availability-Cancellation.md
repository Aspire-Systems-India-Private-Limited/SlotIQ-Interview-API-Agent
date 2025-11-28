# Issue Template: Availability Cancellation (FR#MAM-3)

### User Story
See: `Requirement/Userstories/3#AvailabilityManagement/FR#MAM-3-Availability-Cancellation.md`

### Functional Requirement
See: `Requirement/Functional/V1/3#AvailabilityManagement/FR#MAM-3 – Availability Cancellation.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: DELETE /availability-plans/{id}
- [ ] Handler: CancelAvailabilityPlanCommandHandler
- [ ] Repository: CancelAvailabilityPlanAsync
- [ ] Entity: AvailabilityPlan
- [ ] SQL: CancelAvailabilityPlan.sql

---

> Use this issue as a checklist and reference for all code and PRs related to cancelling availability plans.
