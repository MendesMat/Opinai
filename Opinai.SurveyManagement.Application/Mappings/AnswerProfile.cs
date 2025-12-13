using AutoMapper;
using Opinai.SurveyManagement.Application.Dtos.Answer;
using Opinai.SurveyManagement.Domain;

namespace Opinai.SurveyManagement.Application.Mappings;

public class AnswerProfile : Profile
{
    public AnswerProfile()
    {
        CreateMap<CreateAnswerDto, Answer>();
        CreateMap<UpdateAnswerDto, Answer>();
        CreateMap<Answer, AnswerDto>();
    }
}
