using FluentAssertions;
using Moq;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.AnswerRequest;
using StudyBuddy.API.Services;
using StudyBuddy.API.UnitTests.TestHelpers;

namespace StudyBuddy.API.UnitTests.Services;

public class AnswerServiceTests
{
    private readonly Mock<IAnswerRepository> _answerRepositoryMock = new();
    private readonly AnswerService _service;

    public AnswerServiceTests()
    {
        _service = new AnswerService(_answerRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAnswersAsync_ShouldReturnMappedAnswers()
    {
        _answerRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(new List<Answer>
            {
                new() { AnswerId = 1, UserId = 1, QuestionId = 1, OptionId = 1 }
            });

        var result = await _service.GetAllAnswersAsync();

        result.Should().HaveCount(1);
        result[0].AnswerId.Should().Be(1);
    }

    // [Fact]
    // public async Task GetAnswerByIdAsync_WhenAnswerExists_ShouldReturnAnswer()
    // {
    //     _answerRepositoryMock.Setup(x => x.GetById(1))
    //         .ReturnsAsync(new Answer { AnswerId = 1, UserId = 1, QuestionId = 1, OptionId = 1 });

    //     var result = await _service.GetAnswerByIdAsync(1);

    //     result.Should().NotBeNull();
    //     result!.AnswerId.Should().Be(1);
    // }

    [Fact]
    public async Task CreateAnswerAsync_WhenRequestIsValid_ShouldInsertAndReturnId()
    {
        Answer? inserted = null;

        _answerRepositoryMock.Setup(x => x.InsertReturnId(It.IsAny<Answer>()))
            .Callback<Answer>(a => inserted = a)
            .ReturnsAsync(3);

        var request = new AnswerCreateRequest
        {
            UserId = 1,
            QuestionId = 1,
            OptionId = 2
        };

        var result = await _service.CreateAnswerAsync(request);

        result.Should().Be(3);
        inserted.Should().NotBeNull();
        inserted!.UserId.Should().Be(1);
        inserted.OptionId.Should().Be(2);
    }

    [Fact]
    public async Task UpdateAnswerAsync_WhenAnswerDoesNotExist_ShouldReturnFalse()
    {
        _answerRepositoryMock.Setup(x => x.GetById(999))
            .ReturnsAsync((Answer?)null);

        var request = new AnswerUpdateRequest { AnswerId = 999, UserId = 1, QuestionId = 1, OptionId = 1 };

        var result = await _service.UpdateAnswerAsync(request);

        result.Should().BeFalse();
        _answerRepositoryMock.Verify(x => x.Update(It.IsAny<Answer>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAnswerAsync_WhenAnswerExists_ShouldDeleteAndReturnTrue()
    {
        _answerRepositoryMock.Setup(x => x.GetById(1))
            .ReturnsAsync(new Answer { AnswerId = 1 });

        _answerRepositoryMock.Setup(x => x.Delete(1))
            .ReturnsAsync(true);

        var result = await _service.DeleteAnswerAsync(1);

        result.Should().BeTrue();
    }
}
