using MassTransit;
using Microsoft.OpenApi.Models;
using Opinai.SurveyAnalytics.Application.Consumers;
using Opinai.SurveyAnalytics.Application.Interfaces;
using Opinai.SurveyAnalytics.Application.Services;
using Opinai.SurveyAnalytics.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Domain Services
builder.Services.AddScoped<SurveyAnalyticsCalculator>();

// Application Services
builder.Services.AddScoped<ISurveyAnalyticsService, SurveyAnalyticsService>();

// Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Analytics API",
        Version = "v1",
        Description = "API para análise de dados de pesquisas."
    });
});

// MassTransmit
builder.Services.AddMassTransit(c =>
{
    c.AddConsumer<SurveyResultsConsumer>();

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
