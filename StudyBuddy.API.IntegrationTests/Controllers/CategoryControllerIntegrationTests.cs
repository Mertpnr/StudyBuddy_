using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using StudyBuddy.API.DTOs.CategoryDto;
using StudyBuddy.API.IntegrationTests.Fixtures;
using StudyBuddy.API.Requests.CategoryRequest;

namespace StudyBuddy.API.IntegrationTests.Controllers;

public class CategoryControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CategoryControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkAndCategories()
    {
        var response = await _client.GetAsync("/api/Category/GetAll");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<CategoryListDto>>();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result![0].CategoryName.Should().Be("Programming");
    }

    [Fact]
    public async Task GetById_WhenCategoryExists_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/Category/GetById/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<CategoryBaseDto>();
        result.Should().NotBeNull();
        result!.CategoryId.Should().Be(1);
    }

    [Fact]
    public async Task GetById_WhenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/Category/GetById/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WhenRequestIsValid_ShouldReturnOk()
    {
        var request = new CategoryCreateRequest { CategoryName = "Physics" };

        var response = await _client.PostAsJsonAsync("/api/Category/Create", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("created", Exactly.Once());
    }

    [Fact]
    public async Task Update_WhenCategoryExists_ShouldReturnOk()
    {
        var request = new CategoryUpdateRequest { CategoryId = 1, CategoryName = "Updated Category" };

        var response = await _client.PutAsJsonAsync("/api/Category/Update", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_WhenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        var request = new CategoryUpdateRequest { CategoryId = 999, CategoryName = "Missing" };

        var response = await _client.PutAsJsonAsync("/api/Category/Update", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WhenCategoryExists_ShouldReturnOk()
    {
        var response = await _client.DeleteAsync("/api/Category/Delete/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_WhenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        var response = await _client.DeleteAsync("/api/Category/Delete/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
