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

            s.OwnsMany(s => s.Questions, q =>
            {
                q.ToTable("Questions");
                q.WithOwner().HasForeignKey("SurveyId");

                // Propriedade fantasma
                q.Property<int>("Id");
                q.HasKey("Id");

                q.Property(x => x.Title).IsRequired();

                q.OwnsMany(x => x.Answers, a =>
                {
                    a.ToTable("Answers");
                    a.WithOwner().HasForeignKey("QuestionId");

                    // Propriedade fantasma
                    a.Property<int>("Id");
                    a.HasKey("Id");

                    a.Property(x => x.Text).IsRequired();
                    a.Property(x => x.IsSelected);
                });

                q.Navigation(x => x.Answers).AutoInclude();
            });

            s.Navigation(s => s.Questions).AutoInclude();
        });
    }

}
