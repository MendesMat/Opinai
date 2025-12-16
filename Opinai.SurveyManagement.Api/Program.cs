using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Opinai.Shared.Application.Interfaces;
using Opinai.Shared.Infrastructure.Persistence;
using Opinai.SurveyManagement.Application.Interface;
using Opinai.SurveyManagement.Application.Mappings;
using Opinai.SurveyManagement.Application.Services;
using Opinai.SurveyManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// AutoMapper
builder.Services.AddAutoMapper(options =>
{
    options.AddProfile<SurveyProfile>();
    options.AddProfile<QuestionProfile>();
    options.AddProfile<AnswerProfile>();
});

// Application Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<SurveyDbContext>>();
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<SurveyDbContext>());
builder.Services.AddScoped(typeof(ICrudRepository<>), typeof(CrudRepositoryBase<>));
builder.Services.AddScoped<ISurveyRepository, SurveyRepository>();
builder.Services.AddScoped<ISurveyService, SurveyService>();

// Entity Framework
builder.Services.AddDbContext<SurveyDbContext>(options =>
    options.UseInMemoryDatabase("SurveyDb"));

// Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Survey API",
        Version = "v1",
        Description = "API para gerenciamento de pesquisas."
    });
});

// MassTransmit
builder.Services.AddMassTransit(c =>
{
    c.UsingInMemory((context, config) =>
    {
        config.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();

