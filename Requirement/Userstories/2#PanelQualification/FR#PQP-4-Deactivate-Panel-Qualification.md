FR#PQP-4 - Deactivate Panel Qualification

Title: Deactivate a member's panel qualification

Description:
As an authorized admin, I want to deactivate a member's panel qualification, so that the member is no longer eligible for that interview panel.

Business Scope:
- Deactivate (soft delete) a panel qualification, preserving the record for audit.
- Only authorized roles can deactivate panel qualifications.

Acceptance Criteria:
- Given I am authorized and provide a valid panel qualification ID When I deactivate a panel qualification Then it is marked inactive and the member is no longer eligible for that panel.
- Given the panel qualification does not exist When I attempt to deactivate Then I am informed it cannot be found.
- Given I am not authorized to deactivate panel qualifications When I attempt to deactivate Then I am not allowed to proceed.

Business Rules & Constraints (from FR):
- Deactivation is a soft delete; qualification data is retained.
- Audit details must be recorded.

Out of Scope:
- Implementation details, UI/UX.

Traceability:
- Source FR: FR#PQP-4 (FR#PQP-4 – Deactivate Panel Qualification.md)
