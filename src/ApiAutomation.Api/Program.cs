using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add Entra ID (Azure AD) authentication
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "AzureAd");

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your Entra ID token"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Public endpoint - no authentication required
app.MapGet("/api/health", () => new HealthResponse(
    Status: "Healthy",
    Timestamp: DateTime.UtcNow,
    Version: "1.0.0"
))
.WithName("GetHealth")
.WithOpenApi()
.Produces<HealthResponse>(StatusCodes.Status200OK);

// Protected endpoint - requires valid Entra ID token
app.MapGet("/api/secure", (HttpContext context) =>
{
    var user = context.User;
    var name = user.FindFirst("name")?.Value ?? user.Identity?.Name ?? "Unknown";
    var objectId = user.FindFirst("oid")?.Value ?? "Unknown";

    return new SecureResponse(
        Message: "You have access!",
        UserName: name,
        ObjectId: objectId,
        Timestamp: DateTime.UtcNow
    );
})
.WithName("GetSecure")
.WithOpenApi()
.RequireAuthorization()
.Produces<SecureResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status401Unauthorized);

app.Run();

public record HealthResponse(string Status, DateTime Timestamp, string Version);
public record SecureResponse(string Message, string UserName, string ObjectId, DateTime Timestamp);
