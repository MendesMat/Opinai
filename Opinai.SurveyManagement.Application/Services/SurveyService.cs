using AutoMapper;
using MassTransit;
using Opinai.Messaging.Contracts.Events;
using Opinai.Shared.Application.Interfaces;
using Opinai.Shared.Application.Services;
using Opinai.SurveyManagement.Application.Dtos.Survey;
using Opinai.SurveyManagement.Application.Enums;
using Opinai.SurveyManagement.Application.Interface;
using Opinai.SurveyManagement.Domain;
using Opinai.SurveyManagement.Domain.Entities;

namespace Opinai.SurveyManagement.Application.Services;

public class SurveyService : QueryServiceBase<Survey, SurveyDto>,
    ISurveyService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public SurveyService(
        ICrudRepository<Survey> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPublishEndpoint publishEndpoint)
        : base(repository, unitOfWork, mapper)
    { 
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> CreateAsync(CreateSurveyDto dto)
    {
        var entity = new Survey(dto.Title, dto.Description);

        var questions = dto.Questions.Select(question => 
            new Question(
                question.Title, 
                question.Answers.Select(answer 
                    => new Answer(answer.Text))
            )
        );

        entity.ReplaceQuestions(questions);

        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateSurveyDto dto)
    {
        var entity = await _repository.GetByIdWithTrackingAsync(id);
        if (entity is null) return false;

        entity.UpdateMetadata(dto.Title, dto.Description);

        var questions = dto.Questions.Select(question => 
            new Question(
                question.Title, 
                question.Answers.Select(answer 
                    => new Answer(answer.Text))
            )
        );

        entity.ReplaceQuestions(questions);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdWithTrackingAsync(id);
        if (entity is null) return false;

        _repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<SurveyActionResult> PublishSurveyAsync(Guid id)
    {
        var entity = await _repository.GetByIdWithTrackingAsync(id);
        if (entity is null) return SurveyActionResult.NotFound;

        if(!entity.CanPublish())
            return SurveyActionResult.InvalidState;

        entity.PublishSurvey();
        
        await _unitOfWork.SaveChangesAsync();
        
        await _publishEndpoint.Publish(
            new SurveyPublished(entity.Id));

        return SurveyActionResult.Success;
    }
    
    public async Task<SurveyActionResult> FinishSurveyAsync(Guid id)
    {
        var entity = await _repository.GetByIdWithTrackingAsync(id);
        if (entity is null) return SurveyActionResult.NotFound;

        if (!entity.CanFinish())
            return SurveyActionResult.InvalidState;

        entity.FinishSurvey();
        
        await _unitOfWork.SaveChangesAsync();

        await _publishEndpoint.Publish(
            new SurveyFinished(entity.Id));

        return SurveyActionResult.Success;
    }
}
