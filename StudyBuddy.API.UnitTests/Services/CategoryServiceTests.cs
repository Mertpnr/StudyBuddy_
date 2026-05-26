using FluentAssertions;
using Moq;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.CategoryRequest;
using StudyBuddy.API.Services;
using StudyBuddy.API.UnitTests.TestHelpers;

namespace StudyBuddy.API.UnitTests.Services;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _service = new CategoryService(_categoryRepositoryMock.Object, MapperTestHelper.CreateMapper());
    }

    [Fact]
    public async Task GetAllCategoriesAsync_ShouldReturnMappedCategories()
    {
        _categoryRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(new List<Category>
            {
                new() { CategoryId = 1, CategoryName = "Programming" },
                new() { CategoryId = 2, CategoryName = "Mathematics" }
            });

        var result = await _service.GetAllCategoriesAsync();

        result.Should().HaveCount(2);
        result[0].CategoryName.Should().Be("Programming");
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryExists_ShouldReturnCategory()
    {
        _categoryRepositoryMock.Setup(x => x.GetById(1))
            .ReturnsAsync(new Category { CategoryId = 1, CategoryName = "Programming" });

        var result = await _service.GetCategoryByIdAsync(1);

        result.Should().NotBeNull();
        result!.CategoryId.Should().Be(1);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryDoesNotExist_ShouldReturnNull()
    {
        _categoryRepositoryMock.Setup(x => x.GetById(999))
            .ReturnsAsync((Category?)null);

        var result = await _service.GetCategoryByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateCategoryAsync_WhenRequestIsValid_ShouldInsertAndReturnId()
    {
        Category? inserted = null;

        _categoryRepositoryMock.Setup(x => x.InsertReturnId(It.IsAny<Category>()))
            .Callback<Category>(c => inserted = c)
            .ReturnsAsync(5);

        var request = new CategoryCreateRequest { CategoryName = "Physics" };

        var result = await _service.CreateCategoryAsync(request);

        result.Should().Be(5);
        inserted.Should().NotBeNull();
        inserted!.CategoryName.Should().Be("Physics");
    }

    [Fact]
    public async Task UpdateCategoryAsync_WhenCategoryDoesNotExist_ShouldReturnFalse()
    {
        _categoryRepositoryMock.Setup(x => x.GetById(999))
            .ReturnsAsync((Category?)null);

        var request = new CategoryUpdateRequest { CategoryId = 999, CategoryName = "Missing" };

        var result = await _service.UpdateCategoryAsync(request);

        result.Should().BeFalse();
        _categoryRepositoryMock.Verify(x => x.Update(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task UpdateCategoryAsync_WhenCategoryExists_ShouldUpdateAndReturnTrue()
    {
        _categoryRepositoryMock.Setup(x => x.GetById(1))
            .ReturnsAsync(new Category { CategoryId = 1, CategoryName = "Old" });

        _categoryRepositoryMock.Setup(x => x.Update(It.IsAny<Category>()))
            .ReturnsAsync(true);

        var request = new CategoryUpdateRequest { CategoryId = 1, CategoryName = "New" };

        var result = await _service.UpdateCategoryAsync(request);

        result.Should().BeTrue();
        _categoryRepositoryMock.Verify(x => x.Update(It.Is<Category>(c =>
            c.CategoryId == 1 && c.CategoryName == "New")), Times.Once);
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenCategoryDoesNotExist_ShouldReturnFalse()
    {
        _categoryRepositoryMock.Setup(x => x.GetById(999))
            .ReturnsAsync((Category?)null);

        var result = await _service.DeleteCategoryAsync(999);

        result.Should().BeFalse();
        _categoryRepositoryMock.Verify(x => x.Delete(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenCategoryExists_ShouldDeleteAndReturnTrue()
    {
        _categoryRepositoryMock.Setup(x => x.GetById(1))
            .ReturnsAsync(new Category { CategoryId = 1 });

        _categoryRepositoryMock.Setup(x => x.Delete(1))
            .ReturnsAsync(true);

        var result = await _service.DeleteCategoryAsync(1);

        result.Should().BeTrue();
        _categoryRepositoryMock.Verify(x => x.Delete(1), Times.Once);
    }
}
