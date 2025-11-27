## FR#AUTH-1 – Member Authentication and Login

### Description
This feature enables members to securely log in to the SlotIQ Interview system using their username or email and password. The system authenticates credentials, issues a JWT token for session management, and returns member details and role information. This supports role-based access control (RBAC) and secure access to system features.

### User Roles
- Master Admin
- Practice Admin
- Tech Team Member
- TA Team Admin

---
### Personas and Permissions
- [All Roles] Can log in with valid credentials.
- [System] Enforces role-based access and permissions after login.

---
### Preconditions and Postconditions
- [Preconditions]
    - Member record exists and is active.
    - Username or email and password are provided.
    - Credentials are valid.
- [Postconditions]
    - JWT token is issued for authenticated session.
    - Member details and role are returned in the response.
    - Audit fields are updated (last login, source, etc.).

### Module Name
- Authentication
---
### Fields Level Business Rules
FieldName| Description | Type | Validation | Required | Default Value | Example
-----|-----|---------------|--------|--------|--------|--------
`userNameOrEmail` | Username or email for login | string | Must match existing member record | Yes | None | "john.doe" or "john.doe@aspiresys.com"
`password` | Password for authentication | string | Must match stored (hashed) password | Yes | None | "P@ssw0rd123"

#### System-Level Business Rules
- Passwords must be stored securely (hashed, never plaintext).
- Only active members can log in.
- JWT token must be generated on successful authentication.
- Audit fields (last login, source) must be updated.
- Role and permissions are determined after login.

---
#### API Level Business Rules
Result Name | ResultType | Response and ActionsValidation | Example | Should be logged | Response Category
------------|------------|--------------------------------|-------------------|-------------------|-------------------
AUTH_SUCCESS | Success | Returns JWT token and member details | {"token": "<jwt-token>", "member": { ... }} | No | Informational
INVALID_CREDENTIALS | Failure | Returns error message | {"error": "Invalid username/email or password."} | Yes | Error
USER_NOT_FOUND | Failure | Returns error message | {"error": "User not found."} | Yes | Error
UNPROCESSABLE_ENTITY | Failure | Returns error message | {"error": "Password field is required and cannot be empty."} | Yes | Informational
BAD_REQUEST | Failure | Returns error message | {"error": "Invalid request payload."} | Yes | Informational

---
### Security and Compliance
- [PII handling] Store only necessary PII. No plaintext passwords anywhere.
- [Audit] Record login attempts, timestamp, source application, and key field changes.
- [Access Controls] Enforce per-role permissions after authentication.

---
### Glossary
- [Member] A system user record with a role and optional practice assignment.
- [JWT Token] A secure token used for session management and authorization.
- [Role] Permission set defining what actions a member can perform.
- [Source] Originating application for the operation (e.g., WebApp, API, Admin).

---
