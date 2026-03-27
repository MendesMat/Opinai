using AutoMapper;
using FluentAssertions;
using MassTransit;
using Moq;
using Opinai.Shared.Application.Interfaces;
using Opinai.SurveyManagement.Application.Dtos.Question;
using Opinai.SurveyManagement.Application.Dtos.Survey;
using Opinai.SurveyManagement.Application.Services;
using Opinai.SurveyManagement.Domain.Entities;

namespace Opinai.SurveyManagement.UnitTests;

public class SurveyServiceTests
{
    private readonly Mock<ICrudRepository<Survey>> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly SurveyService _sut; // System Under Test

    public SurveyServiceTests()
    {
        _repositoryMock = new Mock<ICrudRepository<Survey>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();

        _sut = new SurveyService(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _publishEndpointMock.Object);
    }

    [Fact]
    public async Task CreateAsync_GivenValidDto_ShouldAddEntityAndSaveChanges()
    {
        // Arrange
        var dto = new CreateSurveyDto(
            "Test Survey",
            "Survey Description",
            new List<QuestionDto>()
        );

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Survey>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var resultId = await _sut.CreateAsync(dto);

        // Assert
        resultId.Should().NotBeEmpty();
        _repositoryMock.Verify(r => r.AddAsync(It.Is<Survey>(s => s.Title == "Test Survey")), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_GivenNullDto_ShouldThrowArgumentNullException()
    {
        // Act
        Func<Task> act = async () => await _sut.CreateAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_GivenNullDto_ShouldThrowArgumentNullException()
    {
        // Act
        Func<Task> act = async () => await _sut.UpdateAsync(Guid.NewGuid(), null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_GivenNonExistingId_ShouldReturnFalse()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdWithTrackingAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Survey?)null);

        // Act
        var result = await _sut.UpdateAsync(Guid.NewGuid(), new UpdateSurveyDto("A", "B", Opinai.SurveyManagement.Domain.Enums.Status.Draft, new List<QuestionDto>()));

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_GivenExistingId_ShouldUpdateAndReturnTrue()
    {
        // Arrange
        var survey = new Survey("Old Title", "Old Desc");
        _repositoryMock.Setup(r => r.GetByIdWithTrackingAsync(It.IsAny<Guid>()))
            .ReturnsAsync(survey);

        var dto = new UpdateSurveyDto("New Title", "New Desc", Opinai.SurveyManagement.Domain.Enums.Status.Draft, new List<QuestionDto>());

        // Act
        var result = await _sut.UpdateAsync(Guid.NewGuid(), dto);

        // Assert
        result.Should().BeTrue();
        survey.Title.Should().Be("New Title");
        survey.Description.Should().Be("New Desc");
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_GivenNonExistingId_ShouldReturnFalse()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdWithTrackingAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Survey?)null);

        // Act
        var result = await _sut.DeleteAsync(Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
        _repositoryMock.Verify(r => r.Delete(It.IsAny<Survey>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_GivenExistingId_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        var survey = new Survey("Title", "Desc");
        _repositoryMock.Setup(r => r.GetByIdWithTrackingAsync(It.IsAny<Guid>()))
            .ReturnsAsync(survey);

        // Act
        var result = await _sut.DeleteAsync(Guid.NewGuid());

        // Assert
        result.Should().BeTrue();
        _repositoryMock.Verify(r => r.Delete(survey), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task PublishSurveyAsync_GivenNonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdWithTrackingAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Survey?)null);

        // Act
        var result = await _sut.PublishSurveyAsync(Guid.NewGuid());

        // Assert
        result.Should().Be(Opinai.SurveyManagement.Application.Enums.SurveyActionResult.NotFound);
    }

    [Fact]
    public async Task PublishSurveyAsync_GivenSurveyWithoutQuestions_ShouldReturnInvalidState()
    {
        // Arrange
        var survey = new Survey("Title", "Desc");
        _repositoryMock.Setup(r => r.GetByIdWithTrackingAsync(It.IsAny<Guid>()))
            .ReturnsAsync(survey);

        // Act
        var result = await _sut.PublishSurveyAsync(Guid.NewGuid());

        // Assert
        result.Should().Be(Opinai.SurveyManagement.Application.Enums.SurveyActionResult.InvalidState);
    }

    [Fact]
    public async Task PublishSurveyAsync_GivenValidSurvey_ShouldPublishAndReturnSuccess()
    {
        // Arrange
        var survey = new Survey("Title", "Desc");
        survey.ReplaceQuestions(new[] { new Opinai.SurveyManagement.Domain.ValueObjects.Question("Q", new[] { new Opinai.SurveyManagement.Domain.ValueObjects.Answer("A") }) });
        
        _repositoryMock.Setup(r => r.GetByIdWithTrackingAsync(It.IsAny<Guid>()))
            .ReturnsAsync(survey);

        // Act
        var result = await _sut.PublishSurveyAsync(Guid.NewGuid());

        // Assert
        result.Should().Be(Opinai.SurveyManagement.Application.Enums.SurveyActionResult.Success);
        survey.Status.Should().Be(Opinai.SurveyManagement.Domain.Enums.Status.Published);
        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<Opinai.Messaging.Contracts.Events.SurveyPublished>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task FinishSurveyAsync_GivenNonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdWithTrackingAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Survey?)null);

        // Act
        var result = await _sut.FinishSurveyAsync(Guid.NewGuid());

        // Assert
        result.Should().Be(Opinai.SurveyManagement.Application.Enums.SurveyActionResult.NotFound);
    }

    [Fact]
    public async Task FinishSurveyAsync_GivenSurveyNotPublished_ShouldReturnInvalidState()
    {
        // Arrange
        var survey = new Survey("Title", "Desc");
        // by default it is Draft
        _repositoryMock.Setup(r => r.GetByIdWithTrackingAsync(It.IsAny<Guid>()))
            .ReturnsAsync(survey);

        // Act
        var result = await _sut.FinishSurveyAsync(Guid.NewGuid());

        // Assert
        result.Should().Be(Opinai.SurveyManagement.Application.Enums.SurveyActionResult.InvalidState);
    }

    [Fact]
    public async Task FinishSurveyAsync_GivenPublishedSurvey_ShouldFinishAndReturnSuccess()
    {
        // Arrange
        var survey = new Survey("Title", "Desc");
        survey.ReplaceQuestions(new[] { new Opinai.SurveyManagement.Domain.ValueObjects.Question("Q", new[] { new Opinai.SurveyManagement.Domain.ValueObjects.Answer("A") }) });
        survey.PublishSurvey(); // Now it's published

        _repositoryMock.Setup(r => r.GetByIdWithTrackingAsync(It.IsAny<Guid>()))
            .ReturnsAsync(survey);

        // Act
        var result = await _sut.FinishSurveyAsync(Guid.NewGuid());

        // Assert
        result.Should().Be(Opinai.SurveyManagement.Application.Enums.SurveyActionResult.Success);
        survey.Status.Should().Be(Opinai.SurveyManagement.Domain.Enums.Status.Finished);
        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<Opinai.Messaging.Contracts.Events.SurveyFinished>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
