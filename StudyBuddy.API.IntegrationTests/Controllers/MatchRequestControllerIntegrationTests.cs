using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using StudyBuddy.API.DTOs.MatchRequestDto;
using StudyBuddy.API.IntegrationTests.Fixtures;
using StudyBuddy.API.Requests.MatchRequestRequest;

namespace StudyBuddy.API.IntegrationTests.Controllers;

public class MatchRequestControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MatchRequestControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkAndMatchRequests()
    {
        var response = await _client.GetAsync("/api/MatchRequest/GetAll");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<MatchRequestListDto>>();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetById_WhenMatchRequestExists_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/MatchRequest/GetById/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<MatchRequestBaseDto>();
        result.Should().NotBeNull();
        result!.MatchRequestId.Should().Be(1);
    }

    [Fact]
    public async Task GetById_WhenMatchRequestDoesNotExist_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/MatchRequest/GetById/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenRequestIsValid_ShouldReturnOk()
    {
        var request = new MatchRequestCreateRequest
        {
            User1Id = 1,
            User2Id = 2,
            Status = 0,
            Message = "Can we study together?",
            CreatedDate = DateTime.Today
        };

        var response = await _client.PostAsJsonAsync("/api/MatchRequest/Create", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_WhenMatchRequestExists_ShouldReturnOk()
    {
        var request = new MatchRequestUpdateRequest
        {
            MatchRequestId = 1,
            User1Id = 1,
            User2Id = 2,
            Status = 1,
            Message = "Accepted",
            CreatedDate = DateTime.Today
        };

        var response = await _client.PutAsJsonAsync("/api/MatchRequest/Update", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_WhenMatchRequestExists_ShouldReturnOk()
    {
        var response = await _client.DeleteAsync("/api/MatchRequest/Delete/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
