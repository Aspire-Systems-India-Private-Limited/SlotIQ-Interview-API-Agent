# Issue Template: Book Availability (FR#MAM-5)

### User Story
See: `Requirement/Userstories/3#AvailabilityManagement/FR#MAM-5-Book-Availability.md`

### Functional Requirement
See: `Requirement/Functional/V1/3#AvailabilityManagement/FR#MAM-5 – Book Availability Slot.md`

### Technical References
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`

### Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

### Acceptance Criteria
- [ ] Endpoint: POST /availability-bookings
- [ ] Handler: BookAvailabilityCommandHandler
- [ ] Repository: BookAvailabilityAsync
- [ ] Entity: AvailabilityBooking
- [ ] SQL: BookAvailability.sql

---

> Use this issue as a checklist and reference for all code and PRs related to booking availability slots.
