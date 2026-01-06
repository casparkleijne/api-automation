# CLAUDE.md - AI Assistant Guide for api-automation

This document provides context and guidelines for AI assistants working on this repository.

## Project Overview

**Repository:** api-automation
**Framework:** .NET 8 (Minimal API)
**Language:** C#
**Purpose:** MijnAansluiting Netbeheerders API - Energy grid connection management
**Authentication:** Microsoft Entra ID (Azure AD)
**API Standards:** MFF-BAS API Ontwerprichtlijnen v5.0

## Repository Structure

```
api-automation/
├── ApiAutomation.sln                      # Solution file
├── Guides/
│   └── MFF-BAS-API-Ontwerprichtlijnen-v5.0.pdf  # API design guidelines
├── src/
│   └── ApiAutomation.Api/                 # Main API project
│       ├── ApiAutomation.Api.csproj       # Project file
│       ├── Program.cs                     # Application entry point
│       ├── Models/                        # Domain models
│       │   ├── Common.cs                  # Shared models (pagination, responses)
│       │   ├── ConnectionObject.cs        # Connection object models
│       │   ├── GridOperator.cs            # Grid operator models
│       │   ├── Discipline.cs              # Discipline models
│       │   ├── Service.cs                 # Service/SubService models
│       │   ├── Product.cs                 # Product models
│       │   ├── Question.cs                # Question/Answer models
│       │   └── PriceComponent.cs          # Price component models
│       ├── Endpoints/                     # API endpoint definitions
│       │   ├── ConnectionObjectEndpoints.cs
│       │   ├── GridOperatorEndpoints.cs
│       │   ├── DisciplineEndpoints.cs
│       │   ├── ServiceEndpoints.cs
│       │   ├── ProductEndpoints.cs
│       │   ├── QuestionEndpoints.cs
│       │   └── PriceComponentEndpoints.cs
│       ├── Properties/
│       │   └── launchSettings.json        # Development launch settings
│       ├── appsettings.json               # Configuration
│       └── appsettings.Development.json   # Development configuration
├── .gitignore                             # Git ignore rules
└── CLAUDE.md                              # This file
```

## Tech Stack

- **.NET 8** - Latest LTS framework
- **Minimal API** - Lightweight endpoint configuration
- **Microsoft.Identity.Web** - Entra ID authentication (OAuth 2.0)
- **Swagger/OpenAPI 3.0** - API documentation (JSON format)

## MFF-BAS API Guidelines Applied

This API follows the MFF-BAS API Ontwerprichtlijnen v5.0. Key guidelines implemented:

| ID | Guideline | Implementation |
|----|-----------|----------------|
| ID01 | API definition in UK-English | All resources and attributes in English |
| ID02 | Version management | Versioned endpoints `/api/v1/...` |
| ID05 | RFC 7807 error responses | `ProblemDetails` for all errors |
| ID07 | OpenAPI Info object | Title, description, version, x-releaseDate, contact, license |
| ID10 | HTTP status codes | Proper codes per endpoint (200, 201, 204, 400, 401, 403, 404, 409, 500, 503) |
| ID13 | ISO 8601 date/time | UTC format: `yyyy-MM-ddTHH:mm:ss.fffZ` |
| ID15 | OAuth 2.0 authorization | Microsoft Entra ID with JWT Bearer tokens |
| ID20 | HTTP headers | X-Correlation-ID, X-Request-ID support |
| ID23 | OpenAPI Specification | OAS 3.0 in JSON format |

## API Endpoints

### Health & Authentication

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/v1/health` | No | Health check endpoint |
| GET | `/api/v1/secure` | Yes | Protected endpoint (returns user info) |

### Connection Objects (Aansluitingsobjecten)

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/v1/connection-objects` | No | List all connection objects (paginated) |
| GET | `/api/v1/connection-objects/{id}` | No | Get connection object by ID |
| GET | `/api/v1/connection-objects/by-ean/{ean}` | No | Get connection object by EAN code |
| POST | `/api/v1/connection-objects` | Yes | Create new connection object |
| PUT | `/api/v1/connection-objects/{id}` | Yes | Update connection object |
| DELETE | `/api/v1/connection-objects/{id}` | Yes | Delete connection object |

### Grid Operators (Netbeheerders)

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/v1/grid-operators` | No | List all grid operators (paginated) |
| GET | `/api/v1/grid-operators/{id}` | No | Get grid operator by ID |
| GET | `/api/v1/grid-operators/by-code/{code}` | No | Get grid operator by code |
| POST | `/api/v1/grid-operators` | Yes | Create new grid operator |
| PUT | `/api/v1/grid-operators/{id}` | Yes | Update grid operator |
| DELETE | `/api/v1/grid-operators/{id}` | Yes | Delete grid operator |

### Disciplines

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/v1/disciplines` | No | List all disciplines (paginated) |
| GET | `/api/v1/disciplines/{id}` | No | Get discipline by ID |
| GET | `/api/v1/disciplines/by-code/{code}` | No | Get discipline by code (EL, GAS, HEAT) |
| POST | `/api/v1/disciplines` | Yes | Create new discipline |
| PUT | `/api/v1/disciplines/{id}` | Yes | Update discipline |
| DELETE | `/api/v1/disciplines/{id}` | Yes | Delete discipline |

### Services (Diensten)

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/v1/services` | No | List all services (paginated) |
| GET | `/api/v1/services/{id}` | No | Get service by ID |
| GET | `/api/v1/services/{id}/sub-services` | No | List sub-services for a service |
| POST | `/api/v1/services` | Yes | Create new service |
| PUT | `/api/v1/services/{id}` | Yes | Update service |
| DELETE | `/api/v1/services/{id}` | Yes | Delete service |

### Sub-Services (SubDiensten)

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/v1/sub-services` | No | List all sub-services (paginated) |
| GET | `/api/v1/sub-services/{id}` | No | Get sub-service by ID |
| POST | `/api/v1/sub-services` | Yes | Create new sub-service |
| PUT | `/api/v1/sub-services/{id}` | Yes | Update sub-service |
| DELETE | `/api/v1/sub-services/{id}` | Yes | Delete sub-service |

### Products

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/v1/products` | No | List all products (paginated, filterable) |
| GET | `/api/v1/products/{id}` | No | Get product by ID |
| GET | `/api/v1/products/by-code/{code}` | No | Get product by code |
| POST | `/api/v1/products` | Yes | Create new product |
| PUT | `/api/v1/products/{id}` | Yes | Update product |
| DELETE | `/api/v1/products/{id}` | Yes | Delete product |

### Questions (Vragen)

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/v1/questions` | No | List all questions (paginated) |
| GET | `/api/v1/questions/{id}` | No | Get question by ID |
| GET | `/api/v1/questions/{id}/answers` | No | List answers for a question |
| POST | `/api/v1/questions` | Yes | Create new question |
| PUT | `/api/v1/questions/{id}` | Yes | Update question |
| DELETE | `/api/v1/questions/{id}` | Yes | Delete question |

### Answers (Antwoorden)

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/v1/answers` | No | List all answers (paginated) |
| GET | `/api/v1/answers/{id}` | No | Get answer by ID |
| POST | `/api/v1/answers` | Yes | Create new answer |
| PUT | `/api/v1/answers/{id}` | Yes | Update answer |
| DELETE | `/api/v1/answers/{id}` | Yes | Delete answer |

### Price Components (PrijsComponenten)

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/v1/price-components` | No | List all price components (paginated) |
| GET | `/api/v1/price-components/{id}` | No | Get price component by ID |
| GET | `/api/v1/price-components/by-code/{code}` | No | Get price component by code |
| POST | `/api/v1/price-components` | Yes | Create new price component |
| PUT | `/api/v1/price-components/{id}` | Yes | Update price component |
| DELETE | `/api/v1/price-components/{id}` | Yes | Delete price component |

## Domain Models

### Core Entities

```csharp
// Connection Object - represents an energy grid connection point
public record ConnectionObject(
    Guid Id, string? Ean, string? PostalCode, int? HouseNumber,
    string? HouseNumberAddition, string? Street, string? City,
    string? Country, string? Status, Guid? GridOperatorId,
    string CreatedAt, string? ModifiedAt
);

// Grid Operator - manages energy infrastructure
public record GridOperator(
    Guid Id, string Code, string Name, string? Description,
    string? EanPrefix, string? Email, string? Phone, string? Website,
    bool IsActive, string CreatedAt, string? ModifiedAt
);

// Discipline - energy type (Electricity, Gas, District Heating)
public record Discipline(
    Guid Id, string Code, string Name, string? Description,
    bool IsActive, string CreatedAt, string? ModifiedAt
);

// Service - offered by grid operators
public record Service(
    Guid Id, string Code, string Name, string? Description,
    Guid? DisciplineId, bool IsActive, string CreatedAt, string? ModifiedAt
);

// Product - specific offerings
public record Product(
    Guid Id, string Code, string Name, string? Description,
    Guid? DisciplineId, Guid? ServiceId, Guid? SubServiceId,
    Guid? GridOperatorId, string? ProductType, bool IsActive,
    string? EffectiveFrom, string? EffectiveTo,
    string CreatedAt, string? ModifiedAt
);

// Price Component - pricing for products/services
public record PriceComponent(
    Guid Id, string Code, string Name, string? Description,
    Guid? ProductId, Guid? ServiceId, Guid? GridOperatorId,
    decimal Amount, string Currency, string? Unit,
    decimal? VatPercentage, bool VatIncluded, string? PriceType,
    string? EffectiveFrom, string? EffectiveTo,
    bool IsActive, string CreatedAt, string? ModifiedAt
);
```

### Pagination

All list endpoints support pagination:
- `page` - Page number (1-based, default: 1)
- `pageSize` - Items per page (default: 20, max: 100)

Response format:
```json
{
  "items": [...],
  "page": 1,
  "pageSize": 20,
  "totalCount": 100,
  "totalPages": 5
}
```

### Error Responses (RFC 7807)

All errors return `ProblemDetails`:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "detail": "Connection object with ID '...' was not found.",
  "instance": "/api/v1/connection-objects/..."
}
```

## Authentication

### Microsoft Entra ID (Azure AD) - ID15

This API uses OAuth 2.0 with Microsoft Entra ID for authentication.

### Azure Configuration Required

1. **Register an App in Azure Portal:**
   - Go to Azure Portal > Microsoft Entra ID > App registrations
   - Create a new registration
   - Note the `Application (client) ID` and `Directory (tenant) ID`

2. **Configure the API:**
   - Go to "Expose an API"
   - Set Application ID URI (e.g., `api://YOUR_CLIENT_ID`)

3. **Update appsettings.json:**
```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "YOUR_TENANT_ID",
    "ClientId": "YOUR_CLIENT_ID",
    "Audience": "api://YOUR_CLIENT_ID"
  }
}
```

### Getting a Token

```bash
az login
az account get-access-token --resource api://YOUR_CLIENT_ID
```

### Calling Protected Endpoints

```bash
# Create a new connection object
curl -X POST https://localhost:5001/api/v1/connection-objects \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -H "X-Correlation-ID: $(uuidgen)" \
  -d '{"postalCode":"1234AB","houseNumber":1}'
```

## Development Workflow

### Prerequisites

- .NET 8 SDK
- Azure subscription (for Entra ID)

### Getting Started

```bash
# Clone and restore
git clone <repository-url>
cd api-automation
dotnet restore

# Configure AzureAd in appsettings.json

# Run in Development mode (enables Swagger)
dotnet watch --project src/ApiAutomation.Api
```

### Build Commands

```bash
dotnet build              # Build solution
dotnet test               # Run tests
dotnet publish -c Release # Publish for production
```

### Default URLs

- **HTTP:** http://localhost:5000
- **HTTPS:** https://localhost:5001
- **Swagger UI:** http://localhost:5000/swagger (Development only)

## HTTP Headers (ID20)

### Supported Custom Headers

| Header | Direction | Required | Description |
|--------|-----------|----------|-------------|
| X-Correlation-ID | Request/Response | No | Correlate requests across services |
| X-Request-ID | Request | No | Unique request identifier |
| Authorization | Request | Conditional | Bearer token for protected endpoints |

## AI Assistant Guidelines

### MFF-BAS Compliance Checklist

When adding or modifying endpoints:

1. **ID01** - Use UK-English for all API definitions
2. **ID02** - Include version in URL path (`/api/v1/...`)
3. **ID05** - Return `ProblemDetails` for errors (RFC 7807)
4. **ID07** - Ensure OpenAPI Info object is complete
5. **ID10** - Define all applicable HTTP status codes
6. **ID13** - Use ISO 8601 UTC format for timestamps
7. **ID14** - Define minLength/maxLength for strings
8. **ID15** - Use OAuth 2.0 / Entra ID for auth
9. **ID20** - Support X-Correlation-ID header
10. **ID23** - Export OpenAPI spec in JSON format

### Adding New Endpoints

1. Create model in `Models/` folder
2. Create endpoint file in `Endpoints/` folder with extension method
3. Register in `Program.cs` with `app.MapXxxEndpoints()`
4. Follow existing patterns for pagination, error handling, and OpenAPI docs

### Things to Avoid

- Don't use Dutch in API definitions (ID01)
- Don't return plain text errors (ID05)
- Don't use DateTime without UTC conversion (ID13)
- Don't skip status code definitions (ID10)
- Don't commit secrets or tenant IDs

## Branch Naming Convention

- Feature branches: `feature/<description>`
- Bug fixes: `fix/<description>`
- Claude AI branches: `claude/<session-id>`

## Commit Message Format

```
<type>: <short description>

[optional body]
```

Types: `feat`, `fix`, `docs`, `test`, `refactor`, `chore`

## References

- [MFF-BAS API Ontwerprichtlijnen v5.0](Guides/MFF-BAS-API-Ontwerprichtlijnen-v5.0.pdf)
- [API Strategie Nederlandse Overheid](https://docs.geostandaarden.nl/api/API-Strategie/)
- [RFC 7807 - Problem Details](https://www.rfc-editor.org/rfc/rfc7807)
- [OpenAPI Specification](https://www.openapis.org/)
