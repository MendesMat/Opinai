using AutoMapper;
using Opinai.Shared.Application.Interfaces;
using Opinai.Shared.Application.Services;
using Opinai.SurveyManagement.Application.Dtos.Survey;
using Opinai.SurveyManagement.Application.Interface;
using Opinai.SurveyManagement.Domain;
using Opinai.SurveyManagement.Domain.Entities;

namespace Opinai.SurveyManagement.Application.Services;

public class SurveyService(ICrudRepository<Survey> repository, IUnitOfWork unitOfWork, IMapper mapper)
    : QueryServiceBase<Survey, SurveyDto>(repository, unitOfWork, mapper),
    ISurveyService
{
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

        entity.UpdateMetadata(dto.Title, dto.Description, dto.Status);

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
}
