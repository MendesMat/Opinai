using Microsoft.AspNetCore.Mvc;
using Opinai.Shared.Api;
using Opinai.SurveyManagement.Application.Dtos.Survey;
using Opinai.SurveyManagement.Application.Interface;

namespace Opinai.SurveyManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveyController(ISurveyService service)
        : CrudControllerBase<SurveyDto, CreateSurveyDto, UpdateSurveyDto>(service)
{ }
