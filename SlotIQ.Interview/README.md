# SlotIQ.Interview

## Overview

Generated using Scaffold CQRS template version 3.1.

## Technology Stack

- .NET: net9.0
- C#: 12.0
- Database: SqlServer
- ORM: Dapper v2.1.35
- Authentication: JWT Bearer Token (FR#AUTH-1)

## Getting Started

1. Update connection string in appsettings.json (if using database)
2. Configure JWT settings in appsettings.json
3. Build and run the solution
4. Use test credentials to authenticate (see AUTHENTICATION.md)

## Project Structure

- SlotIQ.Interview.Common - Shared types, enums, and models
- SlotIQ.Interview.Data - Data access layer with repositories
- SlotIQ.Interview.Logic - Business logic with CQRS pattern
- SlotIQ.Interview.API - Minimal API endpoints
- SlotIQ.Interview.Integration - External services
- SlotIQ.Interview.UnitTests - Test suite

## Authentication

This API implements JWT-based authentication with role-based authorization. See [AUTHENTICATION.md](SlotIQ.Interview.API/AUTHENTICATION.md) for detailed documentation.

### Quick Start
```bash
# Login to get JWT token
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"userNameOrEmail":"john.doe","password":"P@ssw0rd123"}'

# Use token to access protected endpoints
curl http://localhost:5000/weatherforecast \
  -H "Authorization: Bearer <your-token>"
```

### Test Credentials
- **MasterAdmin**: Username: `john.doe`, Password: `P@ssw0rd123`
- **PracticeAdmin**: Username: `jane.smith`, Password: `Test@123`

## Documentation

- [Authentication & Authorization](SlotIQ.Interview.API/AUTHENTICATION.md) - Security implementation details
- For additional documentation, see the docs/ folder

