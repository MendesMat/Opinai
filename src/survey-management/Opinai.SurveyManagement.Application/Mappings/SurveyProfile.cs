using AutoMapper;
using Opinai.SurveyManagement.Application.Dtos.Survey;
using Opinai.SurveyManagement.Domain.Entities;

namespace Opinai.SurveyManagement.Application.Mappings;

public class SurveyProfile : Profile
{
    public SurveyProfile()
    {
        CreateMap<CreateSurveyDto, Survey>()
            .ForMember(s => s.Id, c => c.Ignore());

        CreateMap<UpdateSurveyDto, Survey>()
            .ForMember(s => s.Id, c => c.Ignore());

        CreateMap<Survey, SurveyDto>()
            .ForMember(s => s.Questions, opt => opt
            .MapFrom(src => src.Questions));
    }
}
