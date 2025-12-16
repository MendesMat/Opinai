using MassTransit;
using Microsoft.EntityFrameworkCore;
using Opinai.ResponseManagement.Application.Consumers;
using Opinai.ResponseManagement.Application.Interfaces;
using Opinai.ResponseManagement.Application.Services;
using Opinai.ResponseManagement.Infrastructure.Persistence;
using Opinai.Shared.Application.Interfaces;
using Opinai.Shared.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Application Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<SurveyResponseDbContext>>();
builder.Services.AddScoped<DbContext>(sp => 
    sp.GetRequiredService<SurveyResponseDbContext>());

builder.Services.AddScoped<ISurveyResponseRepository, SurveyResponseRepository>();
builder.Services.AddScoped<ISurveyAvailabilityService, SurveyAvailabilityService>();
builder.Services.AddScoped<ISurveyResponseService, SurveyResponseService>();

// Entity Framework
builder.Services.AddDbContext<SurveyResponseDbContext>(options =>
    options.UseInMemoryDatabase("SurveyResponseDb"));

// Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Response Management API",
        Version = "v1",
        Description = "API para coletar respostas de pesquisas."
    });
});

// MassTransmit
builder.Services.AddMassTransit(c =>
{
    c.AddConsumer<SurveyPublishedConsumer>();
    c.AddConsumer<SurveyFinishedConsumer>();

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
