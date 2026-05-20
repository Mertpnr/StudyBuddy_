using FluentAssertions;
using Moq;
using StudyBuddy.API.DTOs.MatchingDto;
using StudyBuddy.API.Enums;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.MatchingRequest;
using StudyBuddy.API.Services;
using Match = StudyBuddy.API.Model.Match;

namespace StudyBuddy.API.UnitTests.Services;

public class MatchingServiceTests
{
    private readonly Mock<IMatchRepository> _matchRepositoryMock = new();
    private readonly MatchingService _service;

    public MatchingServiceTests()
    {
        _service = new MatchingService(_matchRepositoryMock.Object);
    }

    [Fact]
    public async Task CalculateMatchAsync_WhenUsersAreSame_ShouldReturnZeroAndErrorMessage()
    {
        var request = new CalculateMatchRequest
        {
            User1Id = 1,
            User2Id = 1,
            SubjectQuestionId = 5,
            SubjectMatchMode = SubjectMatchMode.Required,
            MinimumSharedQuestions = 1,
            SaveResult = true
        };

        var result = await _service.CalculateMatchAsync(request);

        result.MatchPercent.Should().Be(0m);
        result.Message.Should().Be("A user cannot be matched with themselves.");
        result.SavedToDatabase.Should().BeFalse();

        _matchRepositoryMock.Verify(x => x.GetComparableAnswersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        _matchRepositoryMock.Verify(x => x.Insert(It.IsAny<Match>()), Times.Never);
        _matchRepositoryMock.Verify(x => x.Update(It.IsAny<Match>()), Times.Never);
    }

    [Fact]
    public async Task CalculateMatchAsync_WhenRequiredSubjectDoesNotMatch_ShouldReturnZero()
    {
        _matchRepositoryMock
            .Setup(x => x.AreSubjectsMatchingAsync(1, 2, 5))
            .ReturnsAsync(false);

        var request = new CalculateMatchRequest
        {
            User1Id = 1,
            User2Id = 2,
            SubjectQuestionId = 5,
            SubjectMatchMode = SubjectMatchMode.Required,
            MinimumSharedQuestions = 1,
            SaveResult = true
        };

        var result = await _service.CalculateMatchAsync(request);

        result.User1Id.Should().Be(1);
        result.User2Id.Should().Be(2);
        result.SubjectMatched.Should().BeFalse();
        result.MatchPercent.Should().Be(0m);
        result.Message.Should().Be("Subject matching is required, and the users have different subjects.");
        result.SavedToDatabase.Should().BeFalse();

        _matchRepositoryMock.Verify(x => x.GetComparableAnswersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        _matchRepositoryMock.Verify(x => x.Insert(It.IsAny<Match>()), Times.Never);
    }

    [Fact]
    public async Task CalculateMatchAsync_WhenSharedQuestionsAreBelowMinimum_ShouldReturnZero()
    {
        _matchRepositoryMock
            .Setup(x => x.AreSubjectsMatchingAsync(1, 2, 5))
            .ReturnsAsync(true);

        _matchRepositoryMock
            .Setup(x => x.GetComparableAnswersAsync(1, 2))
            .ReturnsAsync(new List<MatchAnswerCompareDto>
            {
                new() { QuestionId = 1, User1Value = 1m, User2Value = 1m, QuestionWeight = 1m }
            });

        var request = new CalculateMatchRequest
        {
            User1Id = 1,
            User2Id = 2,
            SubjectQuestionId = 5,
            SubjectMatchMode = SubjectMatchMode.Required,
            MinimumSharedQuestions = 2,
            SaveResult = true
        };

        var result = await _service.CalculateMatchAsync(request);

        result.MatchPercent.Should().Be(0m);
        result.SharedQuestionCount.Should().Be(1);
        result.Message.Should().Be("Not enough shared answered questions to calculate a reliable match.");
        result.SavedToDatabase.Should().BeFalse();
    }

    [Fact]
    public async Task CalculateMatchAsync_WhenAnswersAreComparable_ShouldCalculateWeightedMatch()
    {
        _matchRepositoryMock
            .Setup(x => x.AreSubjectsMatchingAsync(1, 2, 5))
            .ReturnsAsync(true);

        _matchRepositoryMock
            .Setup(x => x.GetComparableAnswersAsync(1, 2))
            .ReturnsAsync(new List<MatchAnswerCompareDto>
            {
                new() { QuestionId = 1, User1Value = 1.0m, User2Value = 1.0m, QuestionWeight = 1.0m },
                new() { QuestionId = 2, User1Value = 0.5m, User2Value = 1.0m, QuestionWeight = 1.0m }
            });

        var request = new CalculateMatchRequest
        {
            User1Id = 1,
            User2Id = 2,
            SubjectQuestionId = 5,
            SubjectMatchMode = SubjectMatchMode.Required,
            MinimumSharedQuestions = 1,
            SaveResult = false
        };

        var result = await _service.CalculateMatchAsync(request);

        result.MatchPercent.Should().Be(0.75m);
        result.SharedQuestionCount.Should().Be(2);
        result.SubjectMatched.Should().BeTrue();
        result.SavedToDatabase.Should().BeFalse();
        result.Message.Should().Be("Match calculated successfully.");

        _matchRepositoryMock.Verify(x => x.Insert(It.IsAny<Match>()), Times.Never);
        _matchRepositoryMock.Verify(x => x.Update(It.IsAny<Match>()), Times.Never);
    }

    [Fact]
    public async Task CalculateMatchAsync_WhenSaveResultTrueAndMatchDoesNotExist_ShouldInsertMatch()
    {
        Match? insertedMatch = null;

        _matchRepositoryMock
            .Setup(x => x.AreSubjectsMatchingAsync(1, 2, 5))
            .ReturnsAsync(true);

        _matchRepositoryMock
            .Setup(x => x.GetComparableAnswersAsync(1, 2))
            .ReturnsAsync(new List<MatchAnswerCompareDto>
            {
                new() { QuestionId = 1, User1Value = 1m, User2Value = 1m, QuestionWeight = 1m }
            });

        _matchRepositoryMock
            .Setup(x => x.GetByUsersAsync(1, 2))
            .ReturnsAsync((Match?)null);

        _matchRepositoryMock
            .Setup(x => x.Insert(It.IsAny<Match>()))
            .Callback<Match>(m => insertedMatch = m)
            .ReturnsAsync(1);

        var request = new CalculateMatchRequest
        {
            User1Id = 2,
            User2Id = 1,
            SubjectQuestionId = 5,
            MinimumSharedQuestions = 1,
            SaveResult = true
        };

        var result = await _service.CalculateMatchAsync(request);

        result.SavedToDatabase.Should().BeTrue();
        insertedMatch.Should().NotBeNull();
        insertedMatch!.User1Id.Should().Be(1);
        insertedMatch.User2Id.Should().Be(2);
        insertedMatch.MatchPercent.Should().Be(1m);

        _matchRepositoryMock.Verify(x => x.Insert(It.IsAny<Match>()), Times.Once);
        _matchRepositoryMock.Verify(x => x.Update(It.IsAny<Match>()), Times.Never);
    }
}
