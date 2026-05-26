using FluentAssertions;
using Moq;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.OptionRequest;
using StudyBuddy.API.Services;
using StudyBuddy.API.UnitTests.TestHelpers;

namespace StudyBuddy.API.UnitTests.Services;

public class OptionServiceTests
{
    private readonly Mock<IOptionRepository> _optionRepositoryMock = new();
    private readonly OptionService _service;

    public OptionServiceTests()
    {
        _service = new OptionService(_optionRepositoryMock.Object, MapperTestHelper.CreateMapper());
    }

    [Fact]
    public async Task GetAllOptionsAsync_ShouldReturnMappedOptions()
    {
        _optionRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(new List<Option>
            {
                new() { OptionId = 1, QuestionId = 1, Text = "Morning", Value = 1m, OrderNo = 1 }
            });

        var result = await _service.GetAllOptionsAsync();

        result.Should().HaveCount(1);
        result[0].Text.Should().Be("Morning");
    }

    [Fact]
    public async Task GetOptionByIdAsync_WhenOptionExists_ShouldReturnOption()
    {
        _optionRepositoryMock.Setup(x => x.GetById(1))
            .ReturnsAsync(new Option { OptionId = 1, QuestionId = 1, Text = "Morning", Value = 1m, OrderNo = 1 });

        var result = await _service.GetOptionByIdAsync(1);

        result.Should().NotBeNull();
        result!.QuestionId.Should().Be(1);
        result.Text.Should().Be("Morning");
        result.Value.Should().Be(1m);
    }

    [Fact]
    public async Task CreateOptionAsync_WhenRequestIsValid_ShouldInsertAndReturnId()
    {
        Option? inserted = null;

        _optionRepositoryMock.Setup(x => x.InsertReturnId(It.IsAny<Option>()))
            .Callback<Option>(o => inserted = o)
            .ReturnsAsync(10);

        var request = new OptionCreateRequest
        {
            QuestionId = 1,
            Text = "Evening",
            Value = 0.5m,
            OrderNo = 2
        };

        var result = await _service.CreateOptionAsync(request);

        result.Should().Be(10);
        inserted.Should().NotBeNull();
        inserted!.Text.Should().Be("Evening");
        inserted.QuestionId.Should().Be(1);
    }

    [Fact]
    public async Task UpdateOptionAsync_WhenOptionDoesNotExist_ShouldReturnFalse()
    {
        _optionRepositoryMock.Setup(x => x.GetById(999))
            .ReturnsAsync((Option?)null);

        var request = new OptionUpdateRequest { OptionId = 999, QuestionId = 1, Text = "Missing", Value = 1m, OrderNo = 1 };

        var result = await _service.UpdateOptionAsync(request);

        result.Should().BeFalse();
        _optionRepositoryMock.Verify(x => x.Update(It.IsAny<Option>()), Times.Never);
    }

    [Fact]
    public async Task DeleteOptionAsync_WhenOptionExists_ShouldDeleteAndReturnTrue()
    {
        _optionRepositoryMock.Setup(x => x.GetById(1))
            .ReturnsAsync(new Option {OptionId = 1, QuestionId = 1, Text = "Math", Value = 1m, OrderNo = 1});

        _optionRepositoryMock.Setup(x => x.Delete(1))
            .ReturnsAsync(true);

        var result = await _service.DeleteOptionAsync(1);

        result.Should().BeTrue();
    }
}
