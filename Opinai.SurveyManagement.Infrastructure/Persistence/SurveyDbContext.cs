using Microsoft.EntityFrameworkCore;
using Opinai.SurveyManagement.Domain.Entities;

namespace Opinai.SurveyManagement.Infrastructure.Persistence;

public class SurveyDbContext(DbContextOptions<SurveyDbContext> options) 
    : DbContext(options)
{
    public DbSet<Survey> Surveys => Set<Survey>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Survey>(s =>
        {
            s.ToTable("Surveys");
            s.HasKey(s => s.Id);

            s.Property(s => s.Title).IsRequired();
            s.Property(s => s.Description).IsRequired();

            s.OwnsMany(s => s.Questions, q =>
            {
                q.ToTable("Questions");
                q.WithOwner().HasForeignKey("SurveyId");

                q.Property<int>("Id");
                q.HasKey("SurveyId", "Id");

                q.Property(q => q.Index).IsRequired();
                q.Property(q => q.Title).IsRequired();

                q.OwnsMany(q => q.Answers, a =>
                {
                    a.ToTable("Answers");
                    a.WithOwner().HasForeignKey("SurveyId", "QuestionId");

                    a.Property<int>("Id");
                    a.HasKey("SurveyId", "QuestionId", "Id");

                    a.Property(a => a.Index).IsRequired();
                    a.Property(a => a.Text).IsRequired();
                });

                q.Navigation(q => q.Answers).AutoInclude();
            });

            s.Navigation(s => s.Questions).AutoInclude();
        });
    }
}
