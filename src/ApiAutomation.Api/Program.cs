using ApiAutomation.Api.Endpoints;
using ApiAutomation.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Entra ID (Azure AD) authentication (ID15 - OAuth 2.0)
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "AzureAd");

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with OpenAPI Info object (ID07, ID23)
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MijnAansluiting Netbeheerders API",
        Description = """
            # API Version - 1.0.0

            API for managing energy grid connections, services, products, and pricing in the Dutch energy sector.
            Built according to MFF-BAS API Ontwerprichtlijnen v5.0.

            ## Features
            - Connection Objects (Aansluitingsobjecten)
            - Grid Operators (Netbeheerders)
            - Disciplines (Electricity, Gas, District Heating)
            - Services and Sub-Services
            - Products
            - Questions and Answers
            - Price Components

            ## Changelog

            ### 1.0.0 (2025-01-06)
            * Initial release
            * Full MFF-BAS v5.0 compliance
            * Microsoft Entra ID authentication
            """,
        Version = "v1.0.0",
        Contact = new OpenApiContact
        {
            Name = "API Support",
            Email = "support@mijnaansluiting.nl"
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        },
        Extensions =
        {
            ["x-releaseDate"] = new Microsoft.OpenApi.Any.OpenApiString("2025-01-06")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your Entra ID JWT token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Add custom header parameters (ID20)
    options.OperationFilter<CustomHeadersOperationFilter>();
});

// Add ProblemDetails for RFC 7807 compliant errors (ID05)
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add correlation ID middleware (ID20)
app.Use(async (context, next) =>
{
    var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                        ?? Guid.NewGuid().ToString();
    context.Response.Headers["X-Correlation-ID"] = correlationId;
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

// Error handling with RFC 7807 (ID05)
app.UseExceptionHandler();
app.UseStatusCodePages();

// GET /api/v1/health - Public endpoint (ID10 status codes)
app.MapGet("/api/v1/health", () =>
{
    return Results.Ok(new HealthResponse(
        Status: "Healthy",
        Timestamp: DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), // ID13 - ISO 8601 UTC
        Version: "1.0.0"
    ));
})
.WithName("GetHealth")
.WithTags("Health")
.WithOpenApi(operation =>
{
    operation.Summary = "Health check endpoint";
    operation.Description = "Returns the health status of the API. No authentication required.";
    return operation;
})
.Produces<HealthResponse>(StatusCodes.Status200OK)
.Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
.Produces<ProblemDetails>(StatusCodes.Status503ServiceUnavailable);

// GET /api/v1/secure - Protected endpoint (ID10 status codes)
app.MapGet("/api/v1/secure", (HttpContext context) =>
{
    var user = context.User;
    var name = user.FindFirst("name")?.Value ?? user.Identity?.Name ?? "Unknown";
    var objectId = user.FindFirst("oid")?.Value ?? "Unknown";

    return Results.Ok(new SecureResponse(
        Message: "You have access!",
        UserName: name,
        ObjectId: objectId,
        Timestamp: DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") // ID13 - ISO 8601 UTC
    ));
})
.WithName("GetSecure")
.WithTags("Authentication")
.WithOpenApi(operation =>
{
    operation.Summary = "Secure endpoint";
    operation.Description = "Returns user information from the Entra ID token. Requires valid Bearer token.";
    return operation;
})
.RequireAuthorization()
.Produces<SecureResponse>(StatusCodes.Status200OK)
.Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
.Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
.Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
.Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
.Produces<ProblemDetails>(StatusCodes.Status503ServiceUnavailable);

// Map all API endpoints
app.MapConnectionObjectEndpoints();
app.MapGridOperatorEndpoints();
app.MapDisciplineEndpoints();
app.MapServiceEndpoints();
app.MapProductEndpoints();
app.MapQuestionEndpoints();
app.MapPriceComponentEndpoints();

app.Run();

// Custom headers operation filter for Swagger (ID20)
public class CustomHeadersOperationFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
{
    public void Apply(OpenApiOperation operation, Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        // Only add if not already present
        if (!operation.Parameters.Any(p => p.Name == "X-Correlation-ID"))
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Correlation-ID",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Unique identifier for correlating requests across services",
                Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
            });
        }

        if (!operation.Parameters.Any(p => p.Name == "X-Request-ID"))
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Request-ID",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Unique identifier for this request",
                Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
            });
        }
    }
}
