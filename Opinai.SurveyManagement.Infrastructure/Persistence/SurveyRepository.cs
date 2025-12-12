using Opinai.Shared.Infrastructure.Persistence;
using Opinai.SurveyManagement.Application.Interface;
using Opinai.SurveyManagement.Domain.Entities;

namespace Opinai.SurveyManagement.Infrastructure.Persistence;

public class SurveyRepository(SurveyDbContext context)
    : CrudRepositoryBase<Survey>(context), ISurveyRepository
{
}
