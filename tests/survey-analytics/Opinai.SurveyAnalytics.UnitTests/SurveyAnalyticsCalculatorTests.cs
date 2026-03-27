using FluentAssertions;
using Opinai.SurveyAnalytics.Domain.Models.Inputs;
using Opinai.SurveyAnalytics.Domain.Services;

namespace Opinai.SurveyAnalytics.UnitTests;

public class SurveyAnalyticsCalculatorTests
{
    [Fact]
    public void Calculate_GivenValidInput_ShouldReturnCorrectPercentages()
    {
        // Arrange
        var surveyId = Guid.NewGuid();
        var input = new SurveyAnalyticsInput(
            surveyId,
            new List<QuestionAnalyticsInput>
            {
                new(0, new List<AnswerAnalyticsInput>
                {
                    new(0, 50),
                    new(1, 50)
                }),
                new(1, new List<AnswerAnalyticsInput>
                {
                    new(0, 25),
                    new(1, 75)
                })
            }
        );

        // Act
        var result = SurveyAnalyticsCalculator.Calculate(input);

        // Assert
        result.Should().NotBeNull();
        result.SurveyId.Should().Be(surveyId);
        result.Questions.Should().HaveCount(2);

        // Asserting question 0 (Total 100, 50% / 50%)
        result.Questions.ElementAt(0).Answers.Should().ContainSingle(a => a.AnswerIndex == 0 && a.Percentage == 50);
        result.Questions.ElementAt(0).Answers.Should().ContainSingle(a => a.AnswerIndex == 1 && a.Percentage == 50);

        // Asserting question 1 (Total 100, 25% / 75%)
        result.Questions.ElementAt(1).Answers.Should().ContainSingle(a => a.AnswerIndex == 0 && a.Percentage == 25);
        result.Questions.ElementAt(1).Answers.Should().ContainSingle(a => a.AnswerIndex == 1 && a.Percentage == 75);
    }

    [Fact]
    public void Calculate_GivenNullInput_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => SurveyAnalyticsCalculator.Calculate(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Calculate_GivenEmptySurveyId_ShouldThrowArgumentException()
    {
        // Arrange
        var input = new SurveyAnalyticsInput(Guid.Empty, new List<QuestionAnalyticsInput> { new(0, new List<AnswerAnalyticsInput>()) });

        // Act
        Action act = () => SurveyAnalyticsCalculator.Calculate(input);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Calculate_GivenNullQuestionsList_ShouldThrowArgumentNullException()
    {
        // Arrange
        var input = new SurveyAnalyticsInput(Guid.NewGuid(), null!);

        // Act
        Action act = () => SurveyAnalyticsCalculator.Calculate(input);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Calculate_GivenEmptyQuestionsList_ShouldThrowArgumentException()
    {
        // Arrange
        var input = new SurveyAnalyticsInput(Guid.NewGuid(), new List<QuestionAnalyticsInput>());

        // Act
        Action act = () => SurveyAnalyticsCalculator.Calculate(input);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Calculate_GivenQuestionWithNullAnswers_ShouldThrowArgumentException()
    {
        // Arrange
        var input = new SurveyAnalyticsInput(Guid.NewGuid(), new List<QuestionAnalyticsInput> { new(0, null!) });

        // Act
        Action act = () => SurveyAnalyticsCalculator.Calculate(input);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
