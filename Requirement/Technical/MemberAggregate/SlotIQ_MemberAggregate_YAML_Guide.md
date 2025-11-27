# SlotIQ Member Aggregate: Entity & Contract YAMLs Explained

This document explains, in simple terms, how to describe the core data and API contracts for SlotIQ’s Member Management system using YAML. The goal is to make it easy for anyone—regardless of technical background—to contribute to or review these definitions.

---

## 1. What Are These YAML Files?

- **Entities YAML**: Describes the "things" (like Member, Practice, Role) and their properties (fields) in the system.
- **Contracts YAML**: Describes the "actions" (like create, update, search) you can perform on those things, and what information is needed or returned.

---

## 2. How to Read and Write These YAMLs

- Each "thing" (entity) or "action" (API operation) is described as a block.
- Properties are listed with their names, types (text, number, date, etc.), and sometimes rules (like required, or must match a pattern).
- Narrations (comments) are added with `#` to explain what each part means.

---

## 3. Example: Member Entity (from Entities YAML)

```yaml
# The Member entity represents a user in the SlotIQ system.
Member:
  # Inherits common fields like createdDate, modifiedDate, etc.
  allOf:
    - $ref: '#/components/schemas/BaseEntity'
    - type: object
      properties:
        MemberID:        # Unique ID for the member
          type: string
          format: uuid
        UserName:        # Login name (e.g., john.doe)
          type: string
        FirstName:       # First name of the member
          type: string
        LastName:        # Last name of the member
          type: string
        Password:        # Password (stored securely)
          type: string
        EmailID:         # Email address (must be @aspiresys.com)
          type: string
        PhoneNumber:     # Optional phone number
          type: string
          nullable: true
        RoleID:          # Reference to the member's role
          type: integer
        PracticeID:      # Reference to the practice/department
          type: string
        # ...other properties like panel qualifications, availability, etc.
      required:
        - MemberID
        - UserName
        - FirstName
        - LastName
        - Password
        - RoleID
        - EmailID
        - PracticeID
```

---

## 4. Example: Create Member API (from Contracts YAML)

```yaml
# This describes the request to create a new member.
CreateMemberRequest:
  type: object
  required:
    - userName
    - firstname
    - lastname
    - password
    - roleName
    - emailAddress
    - practiceID
    - isActive
    - updatedBy
    - source
  properties:
    userName:      # Login name
      type: string
    firstname:     # First name
      type: string
    lastname:      # Last name
      type: string
    password:      # Password
      type: string
    roleName:      # Role (e.g., Practice Admin)
      type: string
    emailAddress:  # Email
      type: string
    phoneNumber:   # Optional phone
      type: string
    practiceID:    # Practice/department
      type: string
    isActive:      # Is the member active?
      type: boolean
    updatedBy:     # Who is making this change?
      type: string
    source:        # Where is this request coming from? (Web, API, etc.)
      type: integer
```

---

## 5. Common Patterns

- **Enums**: Lists of allowed values (e.g., Role can be MasterAdmin, PracticeAdmin, etc.).
- **Required fields**: Marked under `required:`—these must be provided.
- **References**: Some fields point to other entities (e.g., PracticeID refers to a Practice).
- **Audit fields**: Every entity has fields like createdDate, modifiedDate, createdBy, etc.

---

## 6. How to Add or Change an Entity or Contract

- To add a new "thing" (like a new type of entity), copy an existing block and update the names and properties.
- To add a new "action" (like a new API operation), describe what information is needed in a request and what is returned in a response.
- Use comments (`#`) to explain the purpose of each field in business terms.

---

## 7. Example: Adding a New Field

Suppose you want to add a "MiddleName" to the Member entity:

```yaml
Member:
  allOf:
    - $ref: '#/components/schemas/BaseEntity'
    - type: object
      properties:
        # ...existing fields...
        MiddleName:    # Middle name of the member (optional)
          type: string
          nullable: true
      # Add to required: if it must always be provided
```

---

## 8. Tips for Non-Tech Contributors

- Focus on what information is needed and what it means, not on technical details.
- Use clear, business-friendly names.
- Add comments to clarify intent.
- If unsure about types, use `type: string` (text) as a safe default.

---

## 9. Where to Find More Examples

- Look at the existing YAML files for patterns.
- Each entity and contract is structured similarly—copy and adapt as needed.

---

## 10. Summary Table

| Section         | Purpose                                      |
|-----------------|----------------------------------------------|
| Entities YAML   | Defines the "things" and their properties    |
| Contracts YAML  | Defines the "actions" and their requirements |

---

By following this guide, anyone can help maintain and extend SlotIQ’s entity and contract definitions in a clear, business-friendly way. If you have questions, add a comment in the YAML or ask a team member for clarification.

---
