using FluentAssertions;
using Moq;
using Opinai.ResponseManagement.Application.Services;
using Opinai.ResponseManagement.Application.Interfaces;
using Opinai.Shared.Application.Interfaces;
using Opinai.ResponseManagement.Application.Dtos;

namespace Opinai.ResponseManagement.UnitTests;

public class SurveyResponseServiceTests
{
    private readonly Mock<ISurveyResponseRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly SurveyResponseService _sut;

    public SurveyResponseServiceTests()
    {
        _repositoryMock = new Mock<ISurveyResponseRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _sut = new SurveyResponseService(
            _repositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task AddSurveyResponseAsync_GivenNullDto_ShouldThrowArgumentNullException()
    {
        // Act
        Func<Task> act = async () => await _sut.AddSurveyResponseAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddSurveyResponseAsync_GivenValidDto_ShouldReturnSuccess()
    {
        // Arrange
        var dto = new SurveyResponseDto(
            Guid.NewGuid(),
            new List<QuestionAnswerDto>
            {
                new(0, 1)
            }
        );

        _repositoryMock.Setup(r => r.AddRangeAsync(It.IsAny<Opinai.ResponseManagement.Domain.Entities.SurveyResponse>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _sut.AddSurveyResponseAsync(dto);

        // Assert
        result.Should().Be(Opinai.ResponseManagement.Application.Enums.SurveyResponseResult.Success);
        _repositoryMock.Verify(r => r.AddRangeAsync(It.IsAny<Opinai.ResponseManagement.Domain.Entities.SurveyResponse>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddSurveyResponseAsync_GivenNullAnswersList_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dto = new SurveyResponseDto(Guid.NewGuid(), null!);

        // Act
        Func<Task> act = async () => await _sut.AddSurveyResponseAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddSurveyResponseAsync_GivenEmptyAnswersList_ShouldThrowArgumentException()
    {
        // Arrange
        var dto = new SurveyResponseDto(Guid.NewGuid(), new List<QuestionAnswerDto>());

        // Act
        Func<Task> act = async () => await _sut.AddSurveyResponseAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddSurveyResponseAsync_GivenEmptySurveyId_ShouldThrowArgumentException()
    {
        // Arrange
        var dto = new SurveyResponseDto(Guid.Empty, new List<QuestionAnswerDto> { new(0, 1) });

        // Act
        Func<Task> act = async () => await _sut.AddSurveyResponseAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task BuildSurveyResultsAsync_GivenEmptySurveyId_ShouldThrowArgumentException()
    {
        // Act
        Func<Task> act = async () => await _sut.BuildSurveyResultsAsync(Guid.Empty);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }
}
