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


### Coding Standards & References
- Follow all instructions in the relevant AGENT.md files for each layer:
    - [API Layer](../../SlotIQ.Interview/SlotIQ.Interview.API/AGENT.md)
    - [Logic Layer](../../SlotIQ.Interview/SlotIQ.Interview.Logic/AGENT.md)
    - [Data Layer](../../SlotIQ.Interview/SlotIQ.Interview.Data/AGENT.md)
    - [UnitTests Layer](../../SlotIQ.Interview/SlotIQ.Interview.UnitTests/AGENT.md)
    - [Common Layer](../../SlotIQ.Interview/SlotIQ.Interview.Common/AGENT.md)
- Reference these files in all related code, PRs, and reviews.

### Acceptance Criteria
- [ ] Endpoint: PUT /availability-plans/{id}
- [ ] Handler: UpdateAvailabilityPlanCommandHandler
- [ ] Repository: UpdateAvailabilityPlanAsync
- [ ] Entity: AvailabilityPlan
- [ ] SQL: UpdateAvailabilityPlan.sql

---

> Use this issue as a checklist and reference for all code and PRs related to updating availability plans.
