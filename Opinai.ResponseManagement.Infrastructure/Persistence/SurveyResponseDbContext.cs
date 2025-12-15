using Microsoft.EntityFrameworkCore;
using Opinai.ResponseManagement.Domain.Entities;

namespace Opinai.ResponseManagement.Infrastructure.Persistence;

public class SurveyResponseDbContext(DbContextOptions<SurveyResponseDbContext> options)
    : DbContext(options)
{
    public DbSet<SurveyResponse> SurveyResponses => Set<SurveyResponse>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<SurveyResponse>(r =>
        {
            r.ToTable("SurveyResponses");
            r.HasKey(x => x.Id);

            r.Property(x => x.SurveyId).IsRequired();
            r.Property(x => x.QuestionIndex).IsRequired();
            r.Property(x => x.AnswerIndex).IsRequired();

            r.HasIndex(x => x.SurveyId);
            r.HasIndex(x => new { x.SurveyId, x.QuestionIndex });
            r.HasIndex(x => new { x.SurveyId, x.QuestionIndex, x.AnswerIndex });
        });
    }
}

