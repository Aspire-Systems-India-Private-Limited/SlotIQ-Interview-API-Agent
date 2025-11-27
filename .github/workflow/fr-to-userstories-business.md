---
description: Business-only workflow to convert Functional Requirements (FRs) into user stories stated in business users' language
---

# FR → User Stories (Business-Domain Workflow)

This workflow defines how to transform Functional Requirements located in `Requirement/Functional/V1/` into user stories saved under `Requirement/UserStories/V1/` using only business-domain language. Stories must be understandable by non-technical stakeholders and avoid implementation details.

## Principles
- Business-focused: Use the language of personas (e.g., Master Admin, Practice Admin).
- Outcome-driven: Describe what value/outcome is achieved, not how it is built.
- Atomic: Each story should be independently valuable and completable within a sprint.
- Traceable: Every story must reference its source FR.
- Testable: Acceptance criteria must be observable from a stakeholder perspective.

### Inputs and Outputs
Source FR documents | Target One or more story files per FR saved | 
--------------------|---------------------------------------|
`Requirement/Functional/V1/1#MemberManagement/*.md`| `Requirement/UserStories/1#MemberManagement/`
`Requirement/Functional/V1/2#PanelQualification/*.md` | `Requirement/UserStories/2#PanelQualification/`
`Requirement/Functional/V1/3#AvailabilityManagement/*.md` | `Requirement/UserStories/3#AvailabilityManagement/`
### Content in FR Inputs 
- Personas, policies, and constraints stated in the FRs

## Content in Outputs
- One or more story files per FR saved 
- File naming:
  - Single: `FR#<Tag>-<ShortKebab>.md` (e.g., `FR#MAP-1-Onboard-Member.md`)
  - Multiple: `FR#<Tag>-<ShortKebab>-S1.md`, `FR#<Tag>-<ShortKebab>-S2.md`, ...

## User Story Template (Business-Only)
```
FR#<Tag> - <Short Name>

Title: <Action to achieve business value>

Description:
As a <Persona>, I want to <Action>, so that <Business Benefit>.

Business Scope:
- <Expected outcomes from a business perspective>
- <Key business rules to uphold>
- <Role limitations or permissions from the FR>

Acceptance Criteria:
- Given <precondition> When <action> Then <observable outcome>
- ...

Business Rules & Constraints (from FR):
- <e.g., uniqueness rules, eligibility rules, required fields, SLAs if stated>

Out of Scope:
- <implementation details, UI/UX design, emails/templates unless FR explicitly states>

Traceability:
- Source FR: FR#<Tag> (<Original FR file name>)
```

## Step-by-Step Workflow
1. Identify personas and intent
   - Extract who benefits and what they want to achieve.
   - Write down the business goal in plain language.
2. Determine story split by user intent
   - Split by distinct outcomes (e.g., create, view, enable/disable) rather than by components or layers.
   - Ensure each story is independently valuable and deliverable.
3. Draft the Title and Description
   - Use the “As a … I want … so that …” format.
   - Keep terminology aligned with business roles and policies.
4. Define the Business Scope
   - List the expected outcomes and rules that must hold true when done.
   - Avoid any mention of technical mechanisms or tools.
5. Write Acceptance Criteria (G/W/T)
   - Include happy path and key alternative paths: invalid data, permission denials, duplicate cases, not found.
   - Keep outcomes observable and meaningful for the business.
6. Capture Business Rules & Constraints
   - Transcribe only rules stated or implied by the FR in business terms.
7. Clarify Out of Scope
   - Explicitly exclude implementation details and non-business concerns.
8. Ensure Traceability
   - Reference the FR# tag and the original file name.
9. Name and Save the file(s)
   - Follow the file naming rules under `Requirement/UserStories/Member/`.
10. Peer Review for Business Clarity
   - Confirm the story is understandable without technical translation.
   - Validate acceptance criteria with a business stakeholder.

## Acceptance Criteria Aids (Reusable Patterns)
- Validation
  - Given required fields are missing When submit Then the request is rejected with clear reasons
  - Given a unique field already exists When submit Then the request is rejected for duplication
- Permissions (Roles)
  - Given the user lacks permission When attempt Then the action is not allowed
- Existence
  - Given a referenced item does not exist When proceed Then the stakeholder is informed the item cannot be found
- Success
  - Given a valid request by an allowed role When complete Then the outcome is confirmed with key identifying information

## Definition of Ready (DoR)
- Persona, action, and business value clearly identified
- Acceptance criteria drafted (happy and key edge cases)
- Out of scope clarified

## Definition of Done (DoD)
- Story file created using the template
- Traceability to FR added
- Peer-reviewed for clarity by a business stakeholder

## Example Guidance (from FR#MAP-1)
- Possible story split:
  - S1: Onboard a member by Master Admin (organization-wide permissions)
  - S2: Onboard a member by Practice Admin (practice-scoped permissions)
- Example acceptance criteria snippets:
  - Given a username already exists When onboarding Then the request is rejected for duplication
  - Given an email domain is not allowed by policy When onboarding Then the request is rejected
  - Given the requester has the appropriate role and supplies valid details When onboarding Then the member is successfully onboarded and an identifying reference is provided
