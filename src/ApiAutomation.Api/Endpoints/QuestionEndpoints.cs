using ApiAutomation.Api.Models;
using ApiAutomation.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiAutomation.Api.Endpoints;

/// <summary>
/// Question and Answer endpoints - uses generic CRUD + domain-specific extensions
/// </summary>
public static class QuestionEndpoints
{
    public static void MapQuestionEndpoints(this WebApplication app)
    {
        // Questions - Standard CRUD via generic extensions
        var questions = app.MapGroup("/api/v1/questions").WithTags("Questions");
        questions.MapCrudEndpoints<Question, CreateQuestionRequest, UpdateQuestionRequest, IQuestionRepository>("Question");
        questions.MapGetByCode<Question, CreateQuestionRequest, UpdateQuestionRequest, IQuestionRepository>("Question");
        questions.MapQuestionSpecificEndpoints();

        // Answers - Standard CRUD via generic extensions
        var answers = app.MapGroup("/api/v1/answers").WithTags("Answers");
        answers.MapCrudEndpoints<Answer, CreateAnswerRequest, UpdateAnswerRequest, IAnswerRepository>("Answer");
        answers.MapAnswerSpecificEndpoints();
    }

    /// <summary>
    /// Domain-specific Question endpoints (Single Responsibility)
    /// </summary>
    private static void MapQuestionSpecificEndpoints(this RouteGroupBuilder group)
    {
        // GET /questions/{id}/answers - Nested resource
        group.MapGet("/{id:guid}/answers", async (
            IQuestionRepository questionRepo,
            IAnswerRepository answerRepo,
            Guid id) =>
        {
            var question = await questionRepo.GetByIdAsync(id);
            if (question is null)
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Question with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound);

            var answers = await answerRepo.GetByQuestionIdAsync(id);
            return Results.Ok(answers.ToList());
        })
        .WithName("GetQuestionAnswers")
        .WithOpenApi(op =>
        {
            op.Summary = "List answers for a question";
            op.Description = "Returns all answer options for a specific question.";
            return op;
        })
        .Produces<List<Answer>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        // GET /questions/mandatory - Filter endpoint
        group.MapGet("/mandatory", async (IQuestionRepository repo) =>
        {
            var items = await repo.GetMandatoryAsync();
            return Results.Ok(items.ToList());
        })
        .WithName("GetMandatoryQuestions")
        .WithOpenApi(op =>
        {
            op.Summary = "List mandatory questions";
            op.Description = "Returns all questions that are mandatory.";
            return op;
        })
        .Produces<List<Question>>(StatusCodes.Status200OK);
    }

    /// <summary>
    /// Domain-specific Answer endpoints (Single Responsibility)
    /// </summary>
    private static void MapAnswerSpecificEndpoints(this RouteGroupBuilder group)
    {
        // POST with validation - requires question to exist
        group.MapPost("/validated", async (
            IQuestionRepository questionRepo,
            IAnswerRepository answerRepo,
            CreateAnswerRequest request) =>
        {
            var question = await questionRepo.GetByIdAsync(request.QuestionId);
            if (question is null)
                return Results.Problem(
                    title: "Bad Request",
                    detail: $"Question with ID '{request.QuestionId}' was not found.",
                    statusCode: StatusCodes.Status400BadRequest);

            var item = await answerRepo.CreateAsync(request);
            return Results.Created($"/api/v1/answers/{item.Id}", item);
        })
        .WithName("CreateAnswerValidated")
        .WithOpenApi(op =>
        {
            op.Summary = "Create answer with validation";
            op.Description = "Creates a new answer after validating the question exists.";
            return op;
        })
        .RequireAuthorization()
        .Produces<Answer>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        // GET by question - filter endpoint
        group.MapGet("/by-question/{questionId:guid}", async (
            IAnswerRepository repo,
            Guid questionId) =>
        {
            var items = await repo.GetByQuestionIdAsync(questionId);
            return Results.Ok(items.ToList());
        })
        .WithName("GetAnswersByQuestion")
        .WithOpenApi(op =>
        {
            op.Summary = "List answers by question";
            op.Description = "Returns all answers for a specific question ID.";
            return op;
        })
        .Produces<List<Answer>>(StatusCodes.Status200OK);
    }
}
