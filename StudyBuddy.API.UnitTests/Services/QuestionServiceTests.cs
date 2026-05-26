using FluentAssertions;
using Moq;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.QuestionRequest;
using StudyBuddy.API.Services;
using StudyBuddy.API.UnitTests.TestHelpers;

namespace StudyBuddy.API.UnitTests.Services;

public class QuestionServiceTests
{
    private readonly Mock<IQuestionRepository> _questionRepositoryMock = new();
    private readonly QuestionService _service;

    public QuestionServiceTests()
    {
        _service = new QuestionService(_questionRepositoryMock.Object, MapperTestHelper.CreateMapper());
    }

    [Fact]
    public async Task GetAllQuestionsAsync_ShouldReturnMappedQuestions()
    {
        _questionRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(new List<Question>
            {
                new() { QuestionId = 1, CategoryId = 1, QuestionText = "Subject?", MatchPercent = 1m }
            });

        var result = await _service.GetAllQuestionsAsync();

        result.Should().HaveCount(1);
        result[0].Question.Should().Be("Subject?");
    }

    // [Fact]
    // public async Task GetQuestionByIdAsync_WhenQuestionExists_ShouldReturnQuestion()
    // {
    //     _questionRepositoryMock.Setup(x => x.GetById(1))
    //         .ReturnsAsync(new Question { QuestionId = 1, CategoryId = 1, QuestionText = "Subject?", MatchPercent = 1m });

    //     var result = await _service.GetQuestionByIdAsync(1);

    //     result.Should().NotBeNull();
    //     result!.QuestionId.Should().Be(1);
    // }

    [Fact]
    public async Task CreateQuestionAsync_WhenRequestIsValid_ShouldInsertAndReturnId()
    {
        Question? inserted = null;

        _questionRepositoryMock.Setup(x => x.InsertReturnId(It.IsAny<Question>()))
            .Callback<Question>(q => inserted = q)
            .ReturnsAsync(3);

        var request = new QuestionCreateRequest
        {
            CategoryId = 1,
            Question = "Study style?",
            MatchPercent = 0.7m
        };

        var result = await _service.CreateQuestionAsync(request);

        result.Should().Be(3);
        inserted.Should().NotBeNull();
        inserted!.QuestionText.Should().Be("Study style?");
    }

    [Fact]
    public async Task UpdateQuestionAsync_WhenQuestionDoesNotExist_ShouldReturnFalse()
    {
        _questionRepositoryMock.Setup(x => x.GetById(999))
            .ReturnsAsync((Question?)null);

        var request = new QuestionUpdateRequest { QuestionId = 999, CategoryId = 1, Question = "Missing", MatchPercent = 1m };

        var result = await _service.UpdateQuestionAsync(request);

        result.Should().BeFalse();
        _questionRepositoryMock.Verify(x => x.Update(It.IsAny<Question>()), Times.Never);
    }

    [Fact]
    public async Task DeleteQuestionAsync_WhenQuestionExists_ShouldDeleteAndReturnTrue()
    {
        _questionRepositoryMock.Setup(x => x.GetById(1))
            .ReturnsAsync(new Question { QuestionId = 1 });

        _questionRepositoryMock.Setup(x => x.Delete(1))
            .ReturnsAsync(true);

        var result = await _service.DeleteQuestionAsync(1);

        result.Should().BeTrue();
    }
}
