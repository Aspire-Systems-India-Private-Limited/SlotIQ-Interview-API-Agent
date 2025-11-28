# GitHub Issue: Panel Qualification Management (Consolidated)

## Summary
Implement all Panel Qualification management operations as per requirements. This includes add, update, list, and deactivate panel qualifications, following SlotIQ architecture and coding standards.

## References
- User Stories: `Requirement/Userstories/2#PanelQualification/`
- Functional Requirements: `Requirement/Functional/V1/2#PanelQualification/`
- OpenAPI Contract: `Requirement/Technical/MemberAggregate/MemberAggregateContracts.openapi.yml`
- Entity Definition: `Requirement/Technical/MemberAggregate/MemberAggregateEntities.openapi.yaml`
- Coding Standards: `.github/copilot-instructions.md`, `.github/Instructions/`

## Implementation Scope
- [ ] API Endpoints (Minimal API)
- [ ] Logic Layer (CQRS handlers, DTOs)
- [ ] Data Layer (Entities, Repositories, SQL)
- [ ] Unit Tests

## Operations Checklist
- [ ] Add Panel Qualification
    - Endpoint: POST /panel-qualifications
    - Handler: AddPanelQualificationCommandHandler
    - Repository: AddPanelQualificationAsync
    - SQL: InsertPanelQualification.sql
- [ ] Update Panel Qualification
    - Endpoint: PUT /panel-qualifications/{id}
    - Handler: UpdatePanelQualificationCommandHandler
    - Repository: UpdatePanelQualificationAsync
    - SQL: UpdatePanelQualification.sql
- [ ] List Panel Qualifications
    - Endpoint: GET /panel-qualifications
    - Handler: ListPanelQualificationsQueryHandler
    - Repository: GetPanelQualificationsAsync
    - SQL: GetPanelQualifications.sql
- [ ] Deactivate Panel Qualification
    - Endpoint: DELETE /panel-qualifications/{id}
    - Handler: DeactivatePanelQualificationCommandHandler
    - Repository: DeactivatePanelQualificationAsync (soft delete)
    - SQL: DeactivatePanelQualification.sql

## Acceptance Criteria
- [ ] All endpoints require authentication and role-based authorization
- [ ] All operations follow SlotIQ coding standards and architecture
- [ ] Unit tests cover all business logic and data access
- [ ] Documentation and OpenAPI specs are updated

---

> Use this consolidated issue to track all code and PRs related to Panel Qualification management operations.
