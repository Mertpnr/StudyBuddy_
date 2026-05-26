using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using StudyBuddy.API.DTOs.AnswerDto;
using StudyBuddy.API.IntegrationTests.Fixtures;
using StudyBuddy.API.Requests.AnswerRequest;

namespace StudyBuddy.API.IntegrationTests.Controllers;

public class AnswersControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AnswersControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkAndAnswers()
    {
        var response = await _client.GetAsync("/api/Answers/GetAll");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<AnswerListDto>>();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetById_WhenAnswerExists_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/Answers/GetById/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<AnswerBaseDto>();
        result.Should().NotBeNull();
        result!.AnswerId.Should().Be(1);
    }

    [Fact]
    public async Task GetById_WhenAnswerDoesNotExist_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/Answers/GetById/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenRequestIsValid_ShouldReturnOk()
    {
        var request = new AnswerCreateRequest
        {
            UserId = 1,
            QuestionId = 1,
            OptionId = 1
        };

        var response = await _client.PostAsJsonAsync("/api/Answers/Create", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_WhenAnswerExists_ShouldReturnOk()
    {
        var request = new AnswerUpdateRequest
        {
            AnswerId = 1,
            UserId = 1,
            QuestionId = 1,
            OptionId = 2
        };

        var response = await _client.PutAsJsonAsync("/api/Answers/Update", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_WhenAnswerExists_ShouldReturnOk()
    {
        var response = await _client.DeleteAsync("/api/Answers/Delete/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
