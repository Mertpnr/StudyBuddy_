using FluentAssertions;
using Moq;
using StudyBuddy.API.DTOs.MatchingDto;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.MatchingRequest;
using StudyBuddy.API.Services;
using Match = StudyBuddy.API.Model.Match;

namespace StudyBuddy.API.UnitTests.Services;

public class MatchingServiceAdditionalTests
{
    private readonly Mock<IMatchRepository> _matchRepositoryMock = new();
    private readonly MatchingService _service;

    public MatchingServiceAdditionalTests()
    {
        _service = new MatchingService(_matchRepositoryMock.Object);
    }

    [Fact]
    public async Task CalculateMatchAsync_WhenSaveResultTrueAndMatchExists_ShouldUpdateExistingMatch()
    {
        Match? updatedMatch = null;

        _matchRepositoryMock.Setup(x => x.GetComparableAnswersAsync(1, 2))
            .ReturnsAsync(new List<MatchAnswerCompareDto>
            {
                new() { QuestionId = 1, User1Value = 1m, User2Value = 1m, QuestionWeight = 1m }
            });

        _matchRepositoryMock.Setup(x => x.GetByUsersAsync(1, 2))
            .ReturnsAsync(new Match
            {
                MatchId = 7,
                User1Id = 1,
                User2Id = 2,
                MatchPercent = 0.2m,
                MatchDate = DateTime.Today.AddDays(-1)
            });

        _matchRepositoryMock.Setup(x => x.Update(It.IsAny<Match>()))
            .Callback<Match>(m => updatedMatch = m)
            .ReturnsAsync(true);

        var request = new CalculateMatchRequest
        {
            User1Id = 1,
            User2Id = 2,
            MinimumSharedQuestions = 1,
            SaveResult = true
        };

        var result = await _service.CalculateMatchAsync(request);

        result.SavedToDatabase.Should().BeTrue();
        result.MatchPercent.Should().Be(0.85m);

        updatedMatch.Should().NotBeNull();
        updatedMatch!.MatchId.Should().Be(7);
        updatedMatch.MatchPercent.Should().Be(0.85m);

        _matchRepositoryMock.Verify(x => x.Update(It.IsAny<Match>()), Times.Once);
        _matchRepositoryMock.Verify(x => x.Insert(It.IsAny<Match>()), Times.Never);
    }

    [Fact]
    public async Task CalculateMatchAsync_WhenNoComparableAnswers_ShouldReturnZero()
    {
        _matchRepositoryMock.Setup(x => x.GetComparableAnswersAsync(1, 2))
            .ReturnsAsync(new List<MatchAnswerCompareDto>());

        var request = new CalculateMatchRequest
        {
            User1Id = 1,
            User2Id = 2,
            MinimumSharedQuestions = 1,
            SaveResult = false
        };

        var result = await _service.CalculateMatchAsync(request);

        result.MatchPercent.Should().Be(0m);
        result.SharedQuestionCount.Should().Be(0);
        result.SavedToDatabase.Should().BeFalse();
        result.Message.Should().Be("Not enough shared answered questions to calculate a reliable match.");
    }
}
