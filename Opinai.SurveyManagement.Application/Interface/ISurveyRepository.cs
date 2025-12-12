using Opinai.Shared.Application.Interfaces;
using Opinai.SurveyManagement.Domain.Entities;

namespace Opinai.SurveyManagement.Application.Interface;

public interface ISurveyRepository : ICrudRepository<Survey>
{
}
