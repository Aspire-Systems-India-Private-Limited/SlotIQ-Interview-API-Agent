# Functional Requirements - Member Management V1

## 1. Core Member Management

### FR#MAP-1: Member Onboarding
**Description**: System shall allow onboarding new members with role assignments
- Capture member details (username, email, role, practice)
- Validate email domain (@aspiresys.com)
- Enforce password complexity rules
- Assign to practice and role
- Track creation metadata (source, timestamp, creator)

### FR#MAP-2: Member Profile Updates
**Description**: System shall support updating member profile information
- Modify contact details
- Update role assignments
- Change practice affiliations
- Track modification history

### FR#MAP-3: Member Listing and Pagination
**Description**: System shall provide paginated member listing with filters
- Support pagination (configurable page size)
- Enable filtering by role, practice, status
- Allow sorting by multiple fields
- Include active/inactive status

### FR#MAP-4: Member Profile Retrieval
**Description**: System shall allow fetching detailed member information
- Return complete member profile
- Include role and practice details
- Show panel qualifications
- Display availability plans

### FR#MAP-5: Member Deactivation
**Description**: System shall support soft deletion of members
- Record deactivation reason
- Maintain audit trail
- Update related entities' status
- Prevent hard deletions

### FR#MAP-6: Member Search
**Description**: System shall provide advanced search capabilities
- Support free-text search
- Enable multiple filter combinations
- Include role-based search restrictions
- Return paginated results

## 2. Panel Qualification Management

### FR#PQ-1: Panel Qualification Creation
**Description**: System shall allow creation of panel qualifications for members.
- Assign one or more recruitment positions to a member.
- Specify applicable interview levels for each qualification.
- Set qualification effective start and end dates.
- Capture assignment metadata (created by, created date, source).
- Validate that the member exists and is active.
- Prevent duplicate active qualifications for the same member, position, and level.

### FR#PQ-2: Panel Qualification Update
**Description**: System shall support updating existing panel qualifications.
- Modify assigned recruitment positions and interview levels.
- Update effective date ranges.
- Change qualification status (active/inactive).
- Record modification metadata (modified by, modified date).
- Enforce business rules for overlapping qualifications and status transitions.

### FR#PQ-3: Panel Qualification Listing & Retrieval
**Description**: System shall provide listing and detailed retrieval of panel qualifications.
- List all panel qualifications for a member, with support for filtering by status, position, and level.
- Support pagination and sorting by effective date, position, or status.
- Retrieve detailed information for a specific qualification, including member, position, level, effective dates, and status.
- Include related metadata (created/modified by, timestamps).

### FR#PQ-4: Panel Qualification Deactivation
**Description**: System shall support soft deactivation of panel qualifications.
- Allow deactivation of a qualification with a specified reason.
- Update qualification status to inactive (soft delete).
- Maintain audit trail of deactivation (who, when, why).
- Prevent hard deletion of qualification records.
- Update related availabilities and bookings as required.

### FR#PQ-5: Panel Qualification Search
**Description**: System shall provide advanced search capabilities for panel qualifications.
- Support free-text search across member name, position, and level.
- Enable filtering by member, position, level, status, and effective date range.
- Return paginated and sorted results.
- Enforce role-based access restrictions for search results.

### FR#PQ-6: Panel Qualification Audit Trail
**Description**: System shall maintain a complete audit trail for all panel qualification changes.
- Record all creation, update, and deactivation actions.
- Store user, timestamp, and action details for each change.
- Provide API endpoints to retrieve audit history for a qualification.

## 3. Availability Management

### FR#MAV-1: Availability Planning
**Description**: System shall manage panel member availability
- Schedule available time slots
- Set date and time ranges
- Assign initial status
- Track creation metadata

### FR#MAV-2: Availability Updates
**Description**: System shall allow modifying availability plans
- Update time slots
- Change availability status
- Modify date ranges
- Record update history

### FR#MAV-3: Availability Retrieval
**Description**: System shall provide availability information
- Filter by date range
- Filter by status
- Support pagination
- Include detailed slot info

### FR#MAV-4: Availability Cancellation
**Description**: System shall handle availability cancellations
- Record cancellation reason
- Update related bookings
- Maintain audit trail
- Send notifications

### FR#MAV-5: Availability Search
**Description**: System shall support availability searching
- Search by date range
- Filter by member
- Filter by status
- Return available slots
