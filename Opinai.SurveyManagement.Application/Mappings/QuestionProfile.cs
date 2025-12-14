using AutoMapper;
using Opinai.SurveyManagement.Application.Dtos.Question;
using Opinai.SurveyManagement.Domain;

namespace Opinai.SurveyManagement.Application.Mappings;

public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        CreateMap<Question, QuestionDto>()
            .ForMember(q => q.Answers, opt => opt
            .MapFrom(src => src.Answers));
    }
}
