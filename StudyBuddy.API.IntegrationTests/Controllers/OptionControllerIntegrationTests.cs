using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using StudyBuddy.API.DTOs.OptionDto;
using StudyBuddy.API.IntegrationTests.Fixtures;
using StudyBuddy.API.Requests.OptionRequest;

namespace StudyBuddy.API.IntegrationTests.Controllers;

public class OptionControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public OptionControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkAndOptions()
    {
        var response = await _client.GetAsync("/api/Option/GetAll");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<OptionListDto>>();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    // [Fact]
    // public async Task GetById_WhenOptionExists_ShouldReturnOk()
    // {
    //     var response = await _client.GetAsync("/api/Option/GetById/1");

    //     response.StatusCode.Should().Be(HttpStatusCode.OK);

    //     var result = await response.Content.ReadFromJsonAsync<OptionBaseDto>();
    //     result.Should().NotBeNull();
    //     result!.OptionId.Should().Be(1);
    // }

    [Fact]
    public async Task GetById_WhenOptionDoesNotExist_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/Option/GetById/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenRequestIsValid_ShouldReturnOk()
    {
        var request = new OptionCreateRequest
        {
            QuestionId = 1,
            Text = "Morning",
            Value = 1m,
            OrderNo = 1
        };

        var response = await _client.PostAsJsonAsync("/api/Option/Create", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_WhenOptionExists_ShouldReturnOk()
    {
        var request = new OptionUpdateRequest
        {
            OptionId = 1,
            QuestionId = 1,
            Text = "Updated option",
            Value = 0.9m,
            OrderNo = 1
        };

        var response = await _client.PutAsJsonAsync("/api/Option/Update", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_WhenOptionExists_ShouldReturnOk()
    {
        var response = await _client.DeleteAsync("/api/Option/Delete/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
