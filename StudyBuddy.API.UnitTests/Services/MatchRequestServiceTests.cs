using FluentAssertions;
using Moq;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.MatchRequestRequest;
using StudyBuddy.API.Services;
using StudyBuddy.API.UnitTests.TestHelpers;

namespace StudyBuddy.API.UnitTests.Services;

public class MatchRequestServiceTests
{
    private readonly Mock<IMatchRequestRepository> _matchRequestRepositoryMock = new();
    private readonly MatchRequestService _service;

    public MatchRequestServiceTests()
    {
        _service = new MatchRequestService(_matchRequestRepositoryMock.Object, MapperTestHelper.CreateMapper());
    }

    [Fact]
    public async Task GetAllMatchRequestsAsync_ShouldReturnMappedMatchRequests()
    {
        _matchRequestRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(new List<MatchRequest>
            {
                new() { MatchRequestId = 1, User1Id = 1, User2Id = 2, Status = 0, Message = "Request", CreatedDate = DateTime.Today }
            });

        var result = await _service.GetAllMatchRequestsAsync();

        result.Should().HaveCount(1);
        result[0].MatchRequestId.Should().Be(1);
    }

    [Fact]
    public async Task GetMatchRequestByIdAsync_WhenExists_ShouldReturnMatchRequest()
    {
        _matchRequestRepositoryMock.Setup(x => x.GetById(1))
            .ReturnsAsync(new MatchRequest { MatchRequestId = 1, User1Id = 1, User2Id = 2, Status = 0, Message = "Request", CreatedDate = DateTime.Today });

        var result = await _service.GetMatchRequestByIdAsync(1);

        result.Should().NotBeNull();
        result!.MatchRequestId.Should().Be(1);
    }

    [Fact]
    public async Task CreateMatchRequestAsync_WhenRequestIsValid_ShouldInsertAndReturnId()
    {
        MatchRequest? inserted = null;

        _matchRequestRepositoryMock.Setup(x => x.InsertReturnId(It.IsAny<MatchRequest>()))
            .Callback<MatchRequest>(mr => inserted = mr)
            .ReturnsAsync(5);

        var request = new MatchRequestCreateRequest
        {
            User1Id = 1,
            User2Id = 2,
            Status = 0,
            Message = "Can we study together?",
            CreatedDate = DateTime.Today
        };

        var result = await _service.CreateMatchRequestAsync(request);

        result.Should().Be(5);
        inserted.Should().NotBeNull();
        inserted!.User1Id.Should().Be(1);
        inserted.User2Id.Should().Be(2);
    }

    [Fact]
    public async Task UpdateMatchRequestAsync_WhenDoesNotExist_ShouldReturnFalse()
    {
        _matchRequestRepositoryMock.Setup(x => x.GetById(999))
            .ReturnsAsync((MatchRequest?)null);

        var request = new MatchRequestUpdateRequest
        {
            MatchRequestId = 999,
            User1Id = 1,
            User2Id = 2,
            Status = 1,
            Message = "Accepted",
            CreatedDate = DateTime.Today
        };

        var result = await _service.UpdateMatchRequestAsync(request);

        result.Should().BeFalse();
        _matchRequestRepositoryMock.Verify(x => x.Update(It.IsAny<MatchRequest>()), Times.Never);
    }

    [Fact]
    public async Task DeleteMatchRequestAsync_WhenExists_ShouldDeleteAndReturnTrue()
    {
        _matchRequestRepositoryMock.Setup(x => x.GetById(1))
            .ReturnsAsync(new MatchRequest { MatchRequestId = 1 });

        _matchRequestRepositoryMock.Setup(x => x.Delete(1))
            .ReturnsAsync(true);

        var result = await _service.DeleteMatchRequestAsync(1);

        result.Should().BeTrue();
    }
}
