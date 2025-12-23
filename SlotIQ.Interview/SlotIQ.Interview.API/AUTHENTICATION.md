# Authentication and Authorization

## Overview
This API implements JWT-based authentication and role-based authorization as per FR#AUTH-1.

## Authentication
All endpoints (except `/api/auth/login`) require a valid JWT bearer token.

### Login Endpoint
- **URL**: `POST /api/auth/login`
- **Access**: Public (no authentication required)
- **Request Body**:
```json
{
  "userNameOrEmail": "john.doe",
  "password": "P@ssw0rd123"
}
```
- **Success Response** (200 OK):
```json
{
  "token": "eyJhbGci...",
  "member": {
    "memberID": "12345678-1234-1234-1234-123456789012",
    "userName": "john.doe",
    "firstname": "John",
    "lastname": "Doe",
    "emailID": "john.doe@aspiresys.com",
    "phoneNumber": "1234567890",
    "roleName": "MasterAdmin",
    "practiceID": "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee",
    "isActive": true,
    "createdDate": "2025-10-29T09:45:37Z",
    "modifiedDate": "2025-11-28T09:45:37Z",
    "modUser": "admin123",
    "source": "Manual"
  }
}
```

### Error Responses
- **401 Unauthorized**: Invalid username/email or password, or inactive user account
- **404 Not Found**: User not found
- **422 Unprocessable Entity**: Password field is required and cannot be empty
- **400 Bad Request**: Invalid request payload

## Authorization
All protected endpoints require a valid JWT token in the Authorization header:
```
Authorization: Bearer <token>
```

## User Roles
The system supports four roles as defined in `MemberRoleEnum`:

1. **MasterAdmin** (Value: 1)
   - Full system access
   - Can manage all practices and members
   
2. **PracticeAdmin** (Value: 2)
   - Manage specific practice
   - Can manage members within their practice
   
3. **TechTeamMember** (Value: 3)
   - Technical team member access
   - Limited to technical operations
   
4. **TATeamAdmin** (Value: 4)
   - TA team administrative access
   - Limited to TA operations

## Role-Based Authorization Pattern
To restrict endpoints to specific roles, use the `.RequireAuthorization()` extension with role specifications:

```csharp
// Require any authenticated user
app.MapGet("/api/resource", Handler)
    .RequireAuthorization();

// Require specific role
app.MapGet("/api/admin/resource", Handler)
    .RequireAuthorization(policy => policy.RequireRole("MasterAdmin"));

// Require one of multiple roles
app.MapGet("/api/practice/resource", Handler)
    .RequireAuthorization(policy => policy.RequireRole("MasterAdmin", "PracticeAdmin"));
```

## Test Users
The following test users are available in the in-memory repository:

### Master Admin
- **Username**: `john.doe`
- **Email**: `john.doe@aspiresys.com`
- **Password**: `P@ssw0rd123`
- **Role**: MasterAdmin

### Practice Admin
- **Username**: `jane.smith`
- **Email**: `jane.smith@aspiresys.com`
- **Password**: `Test@123`
- **Role**: PracticeAdmin

## JWT Token Configuration
JWT tokens are configured in `appsettings.json`:
- **Secret**: Symmetric key for token signing
- **Issuer**: Token issuer (SlotIQ.Interview.API)
- **Audience**: Token audience (SlotIQ.Interview.Client)
- **ExpirationInMinutes**: Token lifetime (60 minutes in production, 120 in development)

## Security Best Practices
1. **Password Storage**: All passwords are hashed using BCrypt before storage
2. **Token Expiration**: JWT tokens expire after the configured duration
3. **HTTPS**: Always use HTTPS in production
4. **Sensitive Data**: JWT secret should be stored in environment variables or Azure Key Vault in production
5. **Active Users Only**: Only active users can authenticate

## Example: Testing with curl

### Login
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"userNameOrEmail":"john.doe","password":"P@ssw0rd123"}'
```

### Access Protected Endpoint
```bash
TOKEN=$(curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"userNameOrEmail":"john.doe","password":"P@ssw0rd123"}' \
  -s | jq -r '.token')

curl http://localhost:5000/weatherforecast \
  -H "Authorization: Bearer $TOKEN"
```
