using Microsoft.EntityFrameworkCore;
using Opinai.ResponseManagement.Application.Interfaces;
using Opinai.ResponseManagement.Application.ReadModels;
using Opinai.ResponseManagement.Domain.Entities;

namespace Opinai.ResponseManagement.Infrastructure.Persistence;

public class SurveyResponseRepository(SurveyResponseDbContext context) 
    : ISurveyResponseRepository
{
    private readonly DbSet<SurveyResponse> _set = context.Set<SurveyResponse>();

    public async Task AddAsync(SurveyResponse entity)
        => await _set.AddAsync(entity);

    public async Task<IReadOnlyCollection<SurveyResponseAggregation>>
    GetAggregatedBySurveyAsync(Guid surveyId)
    {
        return await _set
            .AsNoTracking()
            .Where(x => x.SurveyId == surveyId)
            .GroupBy(x => new { x.QuestionIndex, x.AnswerIndex })
            .Select(g => new SurveyResponseAggregation(
                g.Key.QuestionIndex,
                g.Key.AnswerIndex,
                g.LongCount()
            ))
            .ToListAsync();
    }
}
