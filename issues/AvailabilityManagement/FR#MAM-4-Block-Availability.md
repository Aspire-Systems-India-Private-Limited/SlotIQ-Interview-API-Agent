# Issue Template: Block Availability (FR#MAM-4)

### User Story
See: `Requirement/Userstories/3#AvailabilityManagement/FR#MAM-4-Block-Availability.md`

### Functional Requirement
See: `Requirement/Functional/V1/3#AvailabilityManagement/FR#MAM-4 – Block Availability Slot.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: POST /availability-blocks
- [ ] Handler: BlockAvailabilityCommandHandler
- [ ] Repository: BlockAvailabilityAsync
- [ ] Entity: AvailabilityBlock
- [ ] SQL: BlockAvailability.sql

---

> Use this issue as a checklist and reference for all code and PRs related to blocking availability slots.
