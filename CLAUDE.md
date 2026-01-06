# CLAUDE.md - AI Assistant Guide for api-automation

This document provides context and guidelines for AI assistants working on this repository.

## Project Overview

**Repository:** api-automation
**Framework:** .NET 8 (Minimal API)
**Language:** C#
**Purpose:** API automation and integration framework
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
│       ├── Program.cs                     # Application entry point & endpoints
│       ├── Properties/
│       │   └── launchSettings.json        # Development launch settings
│       ├── appsettings.json               # Configuration (includes AzureAd settings)
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
| ID10 | HTTP status codes | Proper codes per endpoint (200, 400, 401, 403, 500, 503) |
| ID13 | ISO 8601 date/time | UTC format: `yyyy-MM-ddTHH:mm:ss.fffZ` |
| ID15 | OAuth 2.0 authorization | Microsoft Entra ID with JWT Bearer tokens |
| ID20 | HTTP headers | X-Correlation-ID, X-Request-ID support |
| ID23 | OpenAPI Specification | OAS 3.0 in JSON format |

## API Endpoints

| Method | Endpoint | Auth | Description | Response |
|--------|----------|------|-------------|----------|
| GET | `/api/v1/health` | No | Health check endpoint | `HealthResponse` |
| GET | `/api/v1/secure` | Yes | Protected endpoint | `SecureResponse` |

### Response Models

```csharp
public record HealthResponse(string Status, string Timestamp, string Version);
public record SecureResponse(string Message, string UserName, string ObjectId, string Timestamp);
```

### Error Responses (RFC 7807)

All errors return `ProblemDetails`:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "Validation error details",
  "instance": "/api/v1/endpoint"
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
curl -H "Authorization: Bearer YOUR_TOKEN" \
     -H "X-Correlation-ID: $(uuidgen)" \
     https://localhost:5001/api/v1/secure
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

## Code Conventions

### MFF-BAS Compliant Endpoint Pattern

```csharp
// ID01: English, ID02: Versioned, ID10: Status codes, ID13: ISO 8601
app.MapGet("/api/v1/example", () =>
{
    return Results.Ok(new ExampleResponse(
        Data: "value",
        Timestamp: DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
    ));
})
.WithName("GetExample")
.WithOpenApi(op =>
{
    op.Summary = "Example endpoint";
    op.Description = "Detailed description in English";
    return op;
})
.Produces<ExampleResponse>(StatusCodes.Status200OK)
.Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
.Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
```

### Adding Protected Endpoints

```csharp
app.MapGet("/api/v1/protected", (HttpContext ctx) =>
{
    var userId = ctx.User.FindFirst("oid")?.Value;
    return Results.Ok(new { UserId = userId });
})
.RequireAuthorization()
.Produces<object>(StatusCodes.Status200OK)
.Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
.Produces<ProblemDetails>(StatusCodes.Status403Forbidden);
```

### String Definitions (ID14)

Always define string constraints:
```csharp
// In JSON Schema / validation
minLength: 1
maxLength: 100
```

## HTTP Headers (ID20)

### Supported Custom Headers

| Header | Direction | Required | Description |
|--------|-----------|----------|-------------|
| X-Correlation-ID | Request/Response | No | Correlate requests across services |
| X-Request-ID | Request | No | Unique request identifier |
| Authorization | Request | Conditional | Bearer token for protected endpoints |

## Configuration

### appsettings.json

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "YOUR_TENANT_ID",
    "ClientId": "YOUR_CLIENT_ID",
    "Audience": "api://YOUR_CLIENT_ID"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

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

### Things to Avoid

- Don't use Dutch in API definitions (ID01)
- Don't return plain text errors (ID05)
- Don't use DateTime without UTC conversion (ID13)
- Don't skip status code definitions (ID10)
- Don't commit secrets or tenant IDs

## Testing

```bash
dotnet new xunit -o tests/ApiAutomation.Api.Tests
dotnet sln add tests/ApiAutomation.Api.Tests
```

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
