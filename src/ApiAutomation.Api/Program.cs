var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/health", () => new HealthResponse(
    Status: "Healthy",
    Timestamp: DateTime.UtcNow,
    Version: "1.0.0"
))
.WithName("GetHealth")
.WithOpenApi()
.Produces<HealthResponse>(StatusCodes.Status200OK);

app.Run();

public record HealthResponse(string Status, DateTime Timestamp, string Version);
