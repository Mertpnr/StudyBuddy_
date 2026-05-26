using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using StudyBuddy.API.DTOs.AnswerDto;
using StudyBuddy.API.DTOs.AuthDto;
using StudyBuddy.API.DTOs.CategoryDto;
using StudyBuddy.API.DTOs.MatchDto;
using StudyBuddy.API.DTOs.MatchRequestDto;
using StudyBuddy.API.DTOs.MatchingDto;
using StudyBuddy.API.DTOs.OptionDto;
using StudyBuddy.API.DTOs.QuestionDto;
using StudyBuddy.API.DTOs.UserDto;
using StudyBuddy.API.Enums;
using StudyBuddy.API.Requests.AnswerRequest;
using StudyBuddy.API.Requests.AuthRequest;
using StudyBuddy.API.Requests.CategoryRequest;
using StudyBuddy.API.Requests.MatchRequest;
using StudyBuddy.API.Requests.MatchRequestRequest;
using StudyBuddy.API.Requests.MatchingRequest;
using StudyBuddy.API.Requests.OptionRequest;
using StudyBuddy.API.Requests.QuestionRequest;
using StudyBuddy.API.Requests.UserRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.IntegrationTests.Fixtures;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IAuthService>();
            services.RemoveAll<IUserService>();
            services.RemoveAll<IMatchingService>();
            services.RemoveAll<ICategoryService>();
            services.RemoveAll<IQuestionService>();
            services.RemoveAll<IOptionService>();
            services.RemoveAll<IAnswerService>();
            services.RemoveAll<IMatchService>();
            services.RemoveAll<IMatchRequestService>();

            services.AddSingleton(CreateAuthServiceMock().Object);
            services.AddSingleton(CreateUserServiceMock().Object);
            services.AddSingleton(CreateMatchingServiceMock().Object);
            services.AddSingleton(CreateCategoryServiceMock().Object);
            services.AddSingleton(CreateQuestionServiceMock().Object);
            services.AddSingleton(CreateOptionServiceMock().Object);
            services.AddSingleton(CreateAnswerServiceMock().Object);
            services.AddSingleton(CreateMatchServiceMock().Object);
            services.AddSingleton(CreateMatchRequestServiceMock().Object);
        });
    }

    private static Mock<IAuthService> CreateAuthServiceMock()
    {
        var mock = new Mock<IAuthService>();

        mock.Setup(x => x.LoginAsync(It.Is<LoginRequest>(r =>
                r.Email == "test@mail.com" && r.Password == "123456")))
            .ReturnsAsync(new LoginResponseDto
            {
                Success = true,
                Message = "Login successful.",
                User = TestUsers()[0]
            });

        mock.Setup(x => x.LoginAsync(It.Is<LoginRequest>(r =>
                r.Email != "test@mail.com" || r.Password != "123456")))
            .ReturnsAsync(new LoginResponseDto
            {
                Success = false,
                Message = "Invalid email or password."
            });

        return mock;
    }

    private static Mock<IUserService> CreateUserServiceMock()
    {
        var mock = new Mock<IUserService>();

        mock.Setup(x => x.GetAllUsersAsync())
            .ReturnsAsync(TestUsers().Select(u => new UserListDto
            {
                UserId = u.UserId,
                UserGuid = u.UserGuid,
                NameSurname = u.NameSurname,
                Email = u.Email,
                University = u.University,
                Major = u.Major,
                AboutMe = u.AboutMe,
                CreatedDate = u.CreatedDate,
                UpdatedDate = u.UpdatedDate
            }).ToList());

        mock.Setup(x => x.GetUserByIdAsync(1)).ReturnsAsync(TestUsers()[0]);
        mock.Setup(x => x.GetUserByIdAsync(2)).ReturnsAsync(TestUsers()[1]);
        mock.Setup(x => x.GetUserByIdAsync(It.Is<int>(id => id != 1 && id != 2)))
            .ReturnsAsync((UserBaseDto?)null);

        var userGuid = TestUsers()[0].UserGuid;
        mock.Setup(x => x.GetUserByGuidAsync(userGuid)).ReturnsAsync(TestUsers()[0]);
        mock.Setup(x => x.GetUserByGuidAsync(It.Is<Guid>(g => g != userGuid)))
            .ReturnsAsync((UserBaseDto?)null);

        mock.Setup(x => x.CreateUserAsync(It.IsAny<UserCreateRequest>())).ReturnsAsync(10);
        mock.Setup(x => x.UpdateUserAsync(It.Is<UserUpdateRequest>(r => r.UserId == 1))).ReturnsAsync(true);
        mock.Setup(x => x.UpdateUserAsync(It.Is<UserUpdateRequest>(r => r.UserId != 1))).ReturnsAsync(false);
        mock.Setup(x => x.DeleteUserAsync(1)).ReturnsAsync(true);
        mock.Setup(x => x.DeleteUserAsync(It.Is<int>(id => id != 1))).ReturnsAsync(false);

        return mock;
    }

    private static Mock<IMatchingService> CreateMatchingServiceMock()
    {
        var mock = new Mock<IMatchingService>();

        mock.Setup(x => x.CalculateMatchAsync(It.Is<CalculateMatchRequest>(r => r.User1Id == r.User2Id)))
            .ReturnsAsync((CalculateMatchRequest r) => new MatchResultDto
            {
                User1Id = r.User1Id,
                User2Id = r.User2Id,
                MatchPercent = 0m,
                Message = "A user cannot be matched with themselves."
            });

        mock.Setup(x => x.CalculateMatchAsync(It.Is<CalculateMatchRequest>(r => r.User1Id != r.User2Id)))
            .ReturnsAsync((CalculateMatchRequest r) => new MatchResultDto
            {
                User1Id = Math.Min(r.User1Id, r.User2Id),
                User2Id = Math.Max(r.User1Id, r.User2Id),
                MatchPercent = 0.85m,
                SharedQuestionCount = 3,
                SubjectMatched = true,
                SubjectMatchMode = r.SubjectMatchMode.ToString(),
                SavedToDatabase = r.SaveResult,
                Message = "Match calculated successfully."
            });

        return mock;
    }

    private static Mock<ICategoryService> CreateCategoryServiceMock()
    {
        var mock = new Mock<ICategoryService>();

        mock.Setup(x => x.GetAllCategoriesAsync()).ReturnsAsync(new List<CategoryListDto>
        {
            new() { CategoryName = "Programming" },
            new() { CategoryName = "Mathematics" }
        });

        mock.Setup(x => x.GetCategoryByIdAsync(1))
            .ReturnsAsync(new CategoryBaseDto { CategoryName = "Programming" });

        mock.Setup(x => x.GetCategoryByIdAsync(It.Is<int>(id => id != 1)))
            .ReturnsAsync((CategoryBaseDto?)null);

        mock.Setup(x => x.CreateCategoryAsync(It.IsAny<CategoryCreateRequest>())).ReturnsAsync(3);
        mock.Setup(x => x.UpdateCategoryAsync(It.Is<CategoryUpdateRequest>(r => r.CategoryId == 1))).ReturnsAsync(true);
        mock.Setup(x => x.UpdateCategoryAsync(It.Is<CategoryUpdateRequest>(r => r.CategoryId != 1))).ReturnsAsync(false);
        mock.Setup(x => x.DeleteCategoryAsync(1)).ReturnsAsync(true);
        mock.Setup(x => x.DeleteCategoryAsync(It.Is<int>(id => id != 1))).ReturnsAsync(false);

        return mock;
    }

    private static Mock<IQuestionService> CreateQuestionServiceMock()
    {
        var mock = new Mock<IQuestionService>();

        mock.Setup(x => x.GetAllQuestionsAsync()).ReturnsAsync(new List<QuestionListDto>
        {
            new() { CategoryId = 1, Question = "Preferred subject?", MatchPercent = 1m },
            new() { CategoryId = 1, Question = "Study time?", MatchPercent = 0.5m }
        });

        mock.Setup(x => x.GetQuestionByIdAsync(1))
            .ReturnsAsync(new QuestionBaseDto { CategoryId = 1, Question = "Preferred subject?", MatchPercent = 1m });

        mock.Setup(x => x.GetQuestionByIdAsync(It.Is<int>(id => id != 1)))
            .ReturnsAsync((QuestionBaseDto?)null);

        mock.Setup(x => x.CreateQuestionAsync(It.IsAny<QuestionCreateRequest>())).ReturnsAsync(3);
        mock.Setup(x => x.UpdateQuestionAsync(It.Is<QuestionUpdateRequest>(r => r.QuestionId == 1))).ReturnsAsync(true);
        mock.Setup(x => x.UpdateQuestionAsync(It.Is<QuestionUpdateRequest>(r => r.QuestionId != 1))).ReturnsAsync(false);
        mock.Setup(x => x.DeleteQuestionAsync(1)).ReturnsAsync(true);
        mock.Setup(x => x.DeleteQuestionAsync(It.Is<int>(id => id != 1))).ReturnsAsync(false);

        return mock;
    }

    private static Mock<IOptionService> CreateOptionServiceMock()
    {
        var mock = new Mock<IOptionService>();

        mock.Setup(x => x.GetAllOptionsAsync()).ReturnsAsync(new List<OptionListDto>
        {
            new() { QuestionId = 1, Text = "Programming", Value = 1m, OrderNo = 1 },
            new() { QuestionId = 1, Text = "Math", Value = 0.8m, OrderNo = 2 }
        });

        mock.Setup(x => x.GetOptionByIdAsync(1))
            .ReturnsAsync(new OptionBaseDto { QuestionId = 1, Text = "Programming", Value = 1m, OrderNo = 1 });

        mock.Setup(x => x.GetOptionByIdAsync(It.Is<int>(id => id != 1)))
            .ReturnsAsync((OptionBaseDto?)null);

        mock.Setup(x => x.CreateOptionAsync(It.IsAny<OptionCreateRequest>())).ReturnsAsync(3);
        mock.Setup(x => x.UpdateOptionAsync(It.Is<OptionUpdateRequest>(r => r.OptionId == 1))).ReturnsAsync(true);
        mock.Setup(x => x.UpdateOptionAsync(It.Is<OptionUpdateRequest>(r => r.OptionId != 1))).ReturnsAsync(false);
        mock.Setup(x => x.DeleteOptionAsync(1)).ReturnsAsync(true);
        mock.Setup(x => x.DeleteOptionAsync(It.Is<int>(id => id != 1))).ReturnsAsync(false);

        return mock;
    }

    private static Mock<IAnswerService> CreateAnswerServiceMock()
    {
        var mock = new Mock<IAnswerService>();

        mock.Setup(x => x.GetAllAnswersAsync()).ReturnsAsync(new List<AnswerListDto>
        {
            new() { AnswerId = 1, UserId = 1, QuestionId = 1, OptionId = 1 },
            new() { AnswerId = 2, UserId = 2, QuestionId = 1, OptionId = 2 }
        });

        mock.Setup(x => x.GetAnswerByIdAsync(1))
            .ReturnsAsync(new AnswerBaseDto { UserId = 1, QuestionId = 1, OptionId = 1 });

        mock.Setup(x => x.GetAnswerByIdAsync(It.Is<int>(id => id != 1)))
            .ReturnsAsync((AnswerBaseDto?)null);

        mock.Setup(x => x.CreateAnswerAsync(It.IsAny<AnswerCreateRequest>())).ReturnsAsync(3);
        mock.Setup(x => x.UpdateAnswerAsync(It.Is<AnswerUpdateRequest>(r => r.AnswerId == 1))).ReturnsAsync(true);
        mock.Setup(x => x.UpdateAnswerAsync(It.Is<AnswerUpdateRequest>(r => r.AnswerId != 1))).ReturnsAsync(false);
        mock.Setup(x => x.DeleteAnswerAsync(1)).ReturnsAsync(true);
        mock.Setup(x => x.DeleteAnswerAsync(It.Is<int>(id => id != 1))).ReturnsAsync(false);

        return mock;
    }

    private static Mock<IMatchService> CreateMatchServiceMock()
    {
        var mock = new Mock<IMatchService>();

        mock.Setup(x => x.GetAllMatchesAsync()).ReturnsAsync(new List<MatchListDto>
        {
            new() { User1Id = 1, User2Id = 2, MatchPercent = 0.85m, MatchDate = DateTime.Today }
        });

        mock.Setup(x => x.GetMatchByIdAsync(1))
            .ReturnsAsync(new MatchBaseDto { User1Id = 1, User2Id = 2, MatchPercent = 0.85m, MatchDate = DateTime.Today });

        mock.Setup(x => x.GetMatchByIdAsync(It.Is<int>(id => id != 1)))
            .ReturnsAsync((MatchBaseDto?)null);

        mock.Setup(x => x.CreateMatchAsync(It.IsAny<MatchCreateRequest>())).ReturnsAsync(2);
        mock.Setup(x => x.UpdateMatchAsync(It.Is<MatchUpdateRequest>(r => r.MatchId == 1))).ReturnsAsync(true);
        mock.Setup(x => x.UpdateMatchAsync(It.Is<MatchUpdateRequest>(r => r.MatchId != 1))).ReturnsAsync(false);
        mock.Setup(x => x.DeleteMatchAsync(1)).ReturnsAsync(true);
        mock.Setup(x => x.DeleteMatchAsync(It.Is<int>(id => id != 1))).ReturnsAsync(false);

        return mock;
    }

    private static Mock<IMatchRequestService> CreateMatchRequestServiceMock()
    {
        var mock = new Mock<IMatchRequestService>();

        mock.Setup(x => x.GetAllMatchRequestsAsync()).ReturnsAsync(new List<MatchRequestListDto>
        {
            new() { User1Id = 1, User2Id = 2, Status = 0, Message = "Please study together.", CreatedDate = DateTime.Today }
        });

        mock.Setup(x => x.GetMatchRequestByIdAsync(1))
            .ReturnsAsync(new MatchRequestBaseDto { User1Id = 1, User2Id = 2, Status = 0, Message = "Please study together.", CreatedDate = DateTime.Today });

        mock.Setup(x => x.GetMatchRequestByIdAsync(It.Is<int>(id => id != 1)))
            .ReturnsAsync((MatchRequestBaseDto?)null);

        mock.Setup(x => x.CreateMatchRequestAsync(It.IsAny<MatchRequestCreateRequest>())).ReturnsAsync(2);
        mock.Setup(x => x.UpdateMatchRequestAsync(It.Is<MatchRequestUpdateRequest>(r => r.MatchRequestId == 1))).ReturnsAsync(true);
        mock.Setup(x => x.UpdateMatchRequestAsync(It.Is<MatchRequestUpdateRequest>(r => r.MatchRequestId != 1))).ReturnsAsync(false);
        mock.Setup(x => x.DeleteMatchRequestAsync(1)).ReturnsAsync(true);
        mock.Setup(x => x.DeleteMatchRequestAsync(It.Is<int>(id => id != 1))).ReturnsAsync(false);

        return mock;
    }

    private static List<UserBaseDto> TestUsers()
    {
        return new List<UserBaseDto>
        {
            new()
            {
                UserId = 1,
                UserGuid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                NameSurname = "Test User One",
                Email = "test@mail.com",
                University = "KTU",
                Major = "Software Engineering",
                AboutMe = "Test user",
                CreatedDate = DateTime.Today
            },
            new()
            {
                UserId = 2,
                UserGuid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                NameSurname = "Test User Two",
                Email = "second@mail.com",
                University = "KTU",
                Major = "Mathematics",
                AboutMe = "Second test user",
                CreatedDate = DateTime.Today
            }
        };
    }
}
