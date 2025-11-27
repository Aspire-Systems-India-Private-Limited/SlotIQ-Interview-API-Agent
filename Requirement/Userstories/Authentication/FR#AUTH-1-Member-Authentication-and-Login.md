FR#AUTH-1 - Member Authentication and Login

Title: Log in to the SlotIQ Interview System

Description:
As a Member (Master Admin, Practice Admin, Tech Team Member, or TA Team Admin), I want to securely log in to the SlotIQ Interview system using my username or email and password, so that I can access the system features appropriate to my role.

Business Scope:
- Members can log in using valid credentials (username/email and password).
- Only active members are allowed to log in.
- Upon successful login, a secure session (JWT token) is established and member details are provided.
- Role-based access is enforced after login.
- Audit information (such as last login and source) is updated.

Acceptance Criteria:
- Given a valid and active member with correct credentials, When the member logs in, Then a JWT token and member details are returned and access is granted according to their role.
- Given an inactive member, When attempting to log in, Then the login is rejected and the member is informed they cannot access the system.
- Given an incorrect password or username/email, When attempting to log in, Then the login is rejected with an error message indicating invalid credentials.
- Given a missing password, When attempting to log in, Then the login is rejected with a message that the password is required.
- Given a non-existent username/email, When attempting to log in, Then the login is rejected with a message that the user is not found.
- Given a valid login, When the member logs in, Then the system records the login attempt, timestamp, and source application.

Business Rules & Constraints (from FR):
- Passwords must be stored securely (hashed, never plaintext).
- Only active members can log in.
- JWT token must be generated on successful authentication.
- Audit fields (last login, source) must be updated.
- Role and permissions are determined after login.

Out of Scope:
- Implementation details of password hashing, JWT token structure, or UI/UX design.
- Email templates or notifications unless explicitly stated in the FR.

Traceability:
- Source FR: FR#AUTH-1 (FR#AUTH-1-Member-Authentication-and-Login.md)
