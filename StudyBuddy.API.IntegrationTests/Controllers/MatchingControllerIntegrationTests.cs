using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using StudyBuddy.API.DTOs.MatchingDto;
using StudyBuddy.API.Enums;
using StudyBuddy.API.IntegrationTests.Fixtures;
using StudyBuddy.API.Requests.MatchingRequest;

namespace StudyBuddy.API.IntegrationTests.Controllers;

public class MatchingControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
	private readonly HttpClient _client;

	public MatchingControllerIntegrationTests(CustomWebApplicationFactory factory)
	{
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task Calculate_WhenRequestIsValid_ShouldReturnOkAndMatchResult()
	{
		var request = new CalculateMatchRequest
		{
			User1Id = 1,
			User2Id = 2,
			SubjectQuestionId = 5,
			SubjectMatchMode = SubjectMatchMode.Preferred,
			MinimumSharedQuestions = 1,
			SaveResult = false
		};

		var response = await _client.PostAsJsonAsync("/api/Matching/Calculate", request);

		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var result = await response.Content.ReadFromJsonAsync<MatchResultDto>();

		result.Should().NotBeNull();
		result!.MatchPercent.Should().Be(0.85m);
		result.Message.Should().Be("Match calculated successfully.");
	}

	[Fact]
	public async Task Calculate_WhenSameUserIdsAreSent_ShouldReturnBadRequest()
	{
		var request = new CalculateMatchRequest
		{
			User1Id = 1,
			User2Id = 1,
			SubjectQuestionId = 5,
			SubjectMatchMode = SubjectMatchMode.Required,
			MinimumSharedQuestions = 1,
			SaveResult = false
		};

		var response = await _client.PostAsJsonAsync("/api/Matching/Calculate", request);

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}
