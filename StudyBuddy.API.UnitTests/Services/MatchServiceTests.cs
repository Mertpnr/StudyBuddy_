using FluentAssertions;
using Moq;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.MatchRequest;
using StudyBuddy.API.Services;
using StudyBuddy.API.UnitTests.TestHelpers;
using Match = StudyBuddy.API.Model.Match;

namespace StudyBuddy.API.UnitTests.Services;

public class MatchServiceTests
{
    private readonly Mock<IMatchRepository> _matchRepositoryMock = new();
    private readonly MatchService _service;

    public MatchServiceTests()
    {
        _service = new MatchService(_matchRepositoryMock.Object, MapperTestHelper.CreateMapper());
    }

    [Fact]
    public async Task GetAllMatchesAsync_ShouldReturnMappedMatches()
    {
        _matchRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(new List<Match>
            {
                new() { MatchId = 1, User1Id = 1, User2Id = 2, MatchPercent = 0.8m, MatchDate = DateTime.Today }
            });

        var result = await _service.GetAllMatchesAsync();

        result.Should().HaveCount(1);
        result[0].MatchPercent.Should().Be(0.8m);
    }

    [Fact]
    public async Task GetMatchByIdAsync_WhenMatchExists_ShouldReturnMatch()
    {
        _matchRepositoryMock.Setup(x => x.GetById(1))
            .ReturnsAsync(new Match { MatchId = 1, User1Id = 1, User2Id = 2, MatchPercent = 0.8m, MatchDate = DateTime.Today });

        var result = await _service.GetMatchByIdAsync(1);

        result.Should().NotBeNull();
        result!.MatchId.Should().Be(1);
    }

    [Fact]
    public async Task CreateMatchAsync_WhenRequestIsValid_ShouldInsertAndReturnId()
    {
        Match? inserted = null;

        _matchRepositoryMock.Setup(x => x.InsertReturnId(It.IsAny<Match>()))
            .Callback<Match>(m => inserted = m)
            .ReturnsAsync(4);

        var request = new MatchCreateRequest
        {
            User1Id = 1,
            User2Id = 2,
            MatchPercent = 0.9m,
            MatchDate = DateTime.Today
        };

        var result = await _service.CreateMatchAsync(request);

        result.Should().Be(4);
        inserted.Should().NotBeNull();
        inserted!.User1Id.Should().Be(1);
        inserted.User2Id.Should().Be(2);
    }

    [Fact]
    public async Task UpdateMatchAsync_WhenMatchDoesNotExist_ShouldReturnFalse()
    {
        _matchRepositoryMock.Setup(x => x.GetById(999))
            .ReturnsAsync((Match?)null);

        var request = new MatchUpdateRequest { MatchId = 999, User1Id = 1, User2Id = 2, MatchPercent = 0.5m, MatchDate = DateTime.Today };

        var result = await _service.UpdateMatchAsync(request);

        result.Should().BeFalse();
        _matchRepositoryMock.Verify(x => x.Update(It.IsAny<Match>()), Times.Never);
    }

    [Fact]
    public async Task DeleteMatchAsync_WhenMatchExists_ShouldDeleteAndReturnTrue()
    {
        _matchRepositoryMock.Setup(x => x.GetById(1))
            .ReturnsAsync(new Match { MatchId = 1 });

        _matchRepositoryMock.Setup(x => x.Delete(1))
            .ReturnsAsync(true);

        var result = await _service.DeleteMatchAsync(1);

        result.Should().BeTrue();
    }
}
