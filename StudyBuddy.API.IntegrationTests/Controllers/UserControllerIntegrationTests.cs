using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using StudyBuddy.API.DTOs.UserDto;
using StudyBuddy.API.IntegrationTests.Fixtures;
using StudyBuddy.API.Requests.UserRequest;

namespace StudyBuddy.API.IntegrationTests.Controllers;

public class UserControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
	private readonly HttpClient _client;

	public UserControllerIntegrationTests(CustomWebApplicationFactory factory)
	{
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task GetAll_ShouldReturnOkAndUsers()
	{
		var response = await _client.GetAsync("/api/User/GetAll");

		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var users = await response.Content.ReadFromJsonAsync<List<UserListDto>>();
		users.Should().NotBeNull();
		users.Should().NotBeEmpty();
	}

	[Fact]
	public async Task GetById_WhenUserExists_ShouldReturnOk()
	{
		var response = await _client.GetAsync("/api/User/GetById/1");

		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var user = await response.Content.ReadFromJsonAsync<UserBaseDto>();
		user.Should().NotBeNull();
		user!.UserId.Should().Be(1);
	}

	[Fact]
	public async Task GetById_WhenUserDoesNotExist_ShouldReturnNotFound()
	{
		var response = await _client.GetAsync("/api/User/GetById/999");

		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Create_WhenRequestIsValid_ShouldReturnOk()
	{
		var request = new UserCreateRequest
		{
			NameSurname = "New User",
			Email = "new@mail.com",
			Password = "123456",
			University = "KTU"
		};

		var response = await _client.PostAsJsonAsync("/api/User/Create", request);

		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var body = await response.Content.ReadAsStringAsync();
		body.Should().Contain("User created successfully");
	}

	[Fact]
	public async Task Update_WhenRequestIsValid_ShouldReturnOk()
	{
		var request = new UserUpdateRequest
		{
			UserId = 1,
			NameSurname = "Updated User",
			Email = "updated@mail.com",
			University = "KTU"
		};

		var response = await _client.PutAsJsonAsync("/api/User/Update", request);

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task Delete_WhenUserExists_ShouldReturnOk()
	{
		var response = await _client.DeleteAsync("/api/User/Delete/1");

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}
}
