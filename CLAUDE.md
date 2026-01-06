# CLAUDE.md - AI Assistant Guide for api-automation

This document provides context and guidelines for AI assistants working on this repository.

## Project Overview

**Repository:** api-automation
**Framework:** .NET 8 (Minimal API)
**Language:** C#
**Purpose:** API automation and integration framework

## Repository Structure

```
api-automation/
├── ApiAutomation.sln                      # Solution file
├── src/
│   └── ApiAutomation.Api/                 # Main API project
│       ├── ApiAutomation.Api.csproj       # Project file
│       ├── Program.cs                     # Application entry point & endpoints
│       ├── appsettings.json               # Configuration
│       └── appsettings.Development.json   # Development configuration
├── .gitignore                             # Git ignore rules
└── CLAUDE.md                              # This file
```

## Tech Stack

- **.NET 8** - Latest LTS framework
- **Minimal API** - Lightweight endpoint configuration
- **Swagger/OpenAPI** - API documentation (enabled in Development)

## API Endpoints

| Method | Endpoint      | Description           | Response          |
|--------|---------------|-----------------------|-------------------|
| GET    | `/api/health` | Health check endpoint | `HealthResponse`  |

### Response Models

```csharp
public record HealthResponse(string Status, DateTime Timestamp, string Version);
```

## Development Workflow

### Prerequisites

- .NET 8 SDK

### Getting Started

```bash
# Clone the repository
git clone <repository-url>
cd api-automation

# Restore dependencies
dotnet restore

# Run the API
dotnet run --project src/ApiAutomation.Api

# Or run with hot reload
dotnet watch --project src/ApiAutomation.Api
```

### Build Commands

```bash
# Build solution
dotnet build

# Run tests (when added)
dotnet test

# Publish for production
dotnet publish -c Release
```

### Default URLs

- **HTTP:** http://localhost:5000
- **HTTPS:** https://localhost:5001
- **Swagger UI:** https://localhost:5001/swagger (Development only)

## Code Conventions

### C# Style Guidelines

- Use `record` types for DTOs and response models
- Use Minimal API pattern for endpoints
- Enable nullable reference types
- Use implicit usings

### Adding New Endpoints

Add endpoints in `Program.cs` using the Minimal API pattern:

```csharp
app.MapGet("/api/example", () => new { Message = "Hello" })
   .WithName("GetExample")
   .WithOpenApi();
```

### Project Organization

When the project grows, consider:
- `Models/` - Request/Response DTOs
- `Services/` - Business logic
- `Endpoints/` - Endpoint definitions (using Carter or manual grouping)

## Configuration

### appsettings.json

Configuration follows standard ASP.NET Core patterns:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Environment Variables

Standard ASP.NET Core environment variables apply:
- `ASPNETCORE_ENVIRONMENT` - Development/Staging/Production
- `ASPNETCORE_URLS` - Override default URLs

## AI Assistant Guidelines

### When Working on This Repository

1. **Read before modifying**: Always read existing code before making changes
2. **Follow Minimal API patterns**: Keep endpoints in `Program.cs` or use endpoint grouping
3. **Use records for DTOs**: Prefer `record` over `class` for data transfer objects
4. **Keep it simple**: Minimal API is meant to be lightweight
5. **Add OpenAPI metadata**: Use `.WithName()` and `.WithOpenApi()` for documentation

### Common Tasks

#### Adding a New Endpoint
1. Add the endpoint mapping in `Program.cs`
2. Create response/request records if needed
3. Add `.WithName()` and `.WithOpenApi()` for Swagger
4. Test the endpoint

#### Adding a Service
1. Create service interface and implementation
2. Register in DI: `builder.Services.AddScoped<IMyService, MyService>()`
3. Inject into endpoints

### Things to Avoid

- Don't add unnecessary abstractions for simple endpoints
- Don't commit `appsettings.local.json` or secrets
- Don't skip OpenAPI metadata on public endpoints

## Testing

Tests should be added in a separate test project:

```bash
# Create test project (when needed)
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
