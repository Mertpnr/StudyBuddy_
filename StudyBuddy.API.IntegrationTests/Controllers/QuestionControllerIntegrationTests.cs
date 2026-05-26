using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using StudyBuddy.API.DTOs.QuestionDto;
using StudyBuddy.API.IntegrationTests.Fixtures;
using StudyBuddy.API.Requests.QuestionRequest;

namespace StudyBuddy.API.IntegrationTests.Controllers;

public class QuestionControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public QuestionControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkAndQuestions()
    {
        var response = await _client.GetAsync("/api/Question/GetAll");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<QuestionListDto>>();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetById_WhenQuestionExists_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/Question/GetById/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<QuestionBaseDto>();
        result.Should().NotBeNull();
        result!.QuestionId.Should().Be(1);
    }

    [Fact]
    public async Task GetById_WhenQuestionDoesNotExist_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/Question/GetById/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenRequestIsValid_ShouldReturnOk()
    {
        var request = new QuestionCreateRequest
        {
            CategoryId = 1,
            QuestionText = "How do you prefer to study?",
            MatchPercent = 0.5m
        };

        var response = await _client.PostAsJsonAsync("/api/Question/Create", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_WhenQuestionExists_ShouldReturnOk()
    {
        var request = new QuestionUpdateRequest
        {
            QuestionId = 1,
            CategoryId = 1,
            QuestionText = "Updated question",
            MatchPercent = 0.8m
        };

        var response = await _client.PutAsJsonAsync("/api/Question/Update", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_WhenQuestionExists_ShouldReturnOk()
    {
        var response = await _client.DeleteAsync("/api/Question/Delete/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
