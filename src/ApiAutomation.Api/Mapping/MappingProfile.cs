using AutoMapper;
using ApiAutomation.Api.Data.Entities;
using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Mapping;

/// <summary>
/// AutoMapper profile for Entity ↔ DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ConfigureQuestionMappings();
        ConfigureAnswerMappings();
    }

    private void ConfigureQuestionMappings()
    {
        // Entity → DTO (for responses)
        CreateMap<QuestionEntity, Question>()
            .ForMember(d => d.StartDate, o => o.MapFrom(s => FormatDate(s.StartDate)))
            .ForMember(d => d.EndDate, o => o.MapFrom(s => FormatDate(s.EndDate)));

        // CreateRequest → Entity
        CreateMap<CreateQuestionRequest, QuestionEntity>()
            .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()))
            .ForMember(d => d.IsActive, o => o.MapFrom(_ => true))
            .ForMember(d => d.StartDate, o => o.MapFrom(s => ParseDate(s.StartDate)))
            .ForMember(d => d.EndDate, o => o.MapFrom(s => ParseDate(s.EndDate)));

        // UpdateRequest → Entity (partial update)
        CreateMap<UpdateQuestionRequest, QuestionEntity>()
            .ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null));
    }

    private void ConfigureAnswerMappings()
    {
        // Entity → DTO
        CreateMap<AnswerEntity, Answer>()
            .ForMember(d => d.CreatedAt, o => o.MapFrom(s => FormatDate(s.CreatedAt)))
            .ForMember(d => d.ModifiedAt, o => o.MapFrom(s => s.ModifiedAt.HasValue ? FormatDate(s.ModifiedAt.Value) : null));

        // CreateRequest → Entity
        CreateMap<CreateAnswerRequest, AnswerEntity>()
            .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()))
            .ForMember(d => d.IsActive, o => o.MapFrom(_ => true));

        // UpdateRequest → Entity (partial update)
        CreateMap<UpdateAnswerRequest, AnswerEntity>()
            .ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null));
    }

    // ISO 8601 date formatting helpers
    private static string FormatDate(DateTime date) =>
        date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

    private static DateTime ParseDate(string date) =>
        DateTime.TryParse(date, out var result) ? result : DateTime.UtcNow;
}
