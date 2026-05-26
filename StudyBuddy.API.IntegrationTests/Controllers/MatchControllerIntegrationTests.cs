using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using StudyBuddy.API.DTOs.MatchDto;
using StudyBuddy.API.IntegrationTests.Fixtures;
using StudyBuddy.API.Requests.MatchRequest;

namespace StudyBuddy.API.IntegrationTests.Controllers;

public class MatchControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MatchControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkAndMatches()
    {
        var response = await _client.GetAsync("/api/Match/GetAll");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<MatchListDto>>();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    // [Fact]
    // public async Task GetById_WhenMatchExists_ShouldReturnOk()
    // {
    //     var response = await _client.GetAsync("/api/Match/GetById/1");

    //     response.StatusCode.Should().Be(HttpStatusCode.OK);

    //     var result = await response.Content.ReadFromJsonAsync<MatchBaseDto>();
    //     result.Should().NotBeNull();
    //     result!.MatchId.Should().Be(1);
    // }

    [Fact]
    public async Task GetById_WhenMatchDoesNotExist_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/Match/GetById/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenRequestIsValid_ShouldReturnOk()
    {
        var request = new MatchCreateRequest
        {
            User1Id = 1,
            User2Id = 2,
            MatchPercent = 0.85m,
            MatchDate = DateTime.Today
        };

        var response = await _client.PostAsJsonAsync("/api/Match/Create", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_WhenMatchExists_ShouldReturnOk()
    {
        var request = new MatchUpdateRequest
        {
            MatchId = 1,
            User1Id = 1,
            User2Id = 2,
            MatchPercent = 0.95m,
            MatchDate = DateTime.Today
        };

        var response = await _client.PutAsJsonAsync("/api/Match/Update", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_WhenMatchExists_ShouldReturnOk()
    {
        var response = await _client.DeleteAsync("/api/Match/Delete/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
