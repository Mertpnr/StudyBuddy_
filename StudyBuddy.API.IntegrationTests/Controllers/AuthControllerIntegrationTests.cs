using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using StudyBuddy.API.DTOs.AuthDto;
using StudyBuddy.API.IntegrationTests.Fixtures;
using StudyBuddy.API.Requests.AuthRequest;
using StudyBuddy.API.Requests.UserRequest;

namespace StudyBuddy.API.IntegrationTests.Controllers;

public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
	private readonly HttpClient _client;

	public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
	{
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task Register_WhenRequestIsValid_ShouldReturnOk()
	{
		var request = new UserCreateRequest
		{
			NameSurname = "Test User",
			Email = "test@mail.com",
			Password = "123456",
			University = "KTU",
			Major = "Software Engineering"
		};

		var response = await _client.PostAsJsonAsync("/api/Auth/Register", request);

		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var body = await response.Content.ReadAsStringAsync();
		body.Should().Contain("User registered successfully");
	}

	[Fact]
	public async Task Login_WhenCredentialsAreCorrect_ShouldReturnOk()
	{
		var request = new LoginRequest
		{
			Email = "test@mail.com",
			Password = "123456"
		};

		var response = await _client.PostAsJsonAsync("/api/Auth/Login", request);

		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

		result.Should().NotBeNull();
		result!.Success.Should().BeTrue();
		result.User.Should().NotBeNull();
	}

	[Fact]
	public async Task Login_WhenCredentialsAreWrong_ShouldReturnBadRequest()
	{
		var request = new LoginRequest
		{
			Email = "wrong@mail.com",
			Password = "wrong"
		};

		var response = await _client.PostAsJsonAsync("/api/Auth/Login", request);

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}
