using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using StudyBuddy.API.DTOs.AuthDto;
using StudyBuddy.API.DTOs.MatchingDto;
using StudyBuddy.API.DTOs.UserDto;
using StudyBuddy.API.Requests.AuthRequest;
using StudyBuddy.API.Requests.MatchingRequest;
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

            var authServiceMock = new Mock<IAuthService>();

            authServiceMock
                .Setup(x => x.LoginAsync(It.Is<LoginRequest>(r => r.Email == "test@mail.com" && r.Password == "123456")))
                .ReturnsAsync(new LoginResponseDto
                {
                    Success = true,
                    Message = "Login successful.",
                    User = new UserBaseDto
                    {
                        UserId = 1,
                        UserGuid = Guid.NewGuid(),
                        NameSurname = "Test User",
                        Email = "test@mail.com",
                        University = "KTU",
                        CreatedDate = DateTime.Now
                    }
                });

            authServiceMock
                .Setup(x => x.LoginAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync(new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password."
                });

            var userServiceMock = new Mock<IUserService>();

            userServiceMock
                .Setup(x => x.CreateUserAsync(It.IsAny<UserCreateRequest>()))
                .ReturnsAsync(1);

            userServiceMock
                .Setup(x => x.GetAllUsersAsync())
                .ReturnsAsync(new List<UserListDto>
                {
                    new()
                    {
                        UserId = 1,
                        UserGuid = Guid.NewGuid(),
                        NameSurname = "Test User",
                        Email = "test@mail.com",
                        University = "KTU",
                        CreatedDate = DateTime.Now
                    }
                });

            userServiceMock
                .Setup(x => x.GetUserByIdAsync(1))
                .ReturnsAsync(new UserBaseDto
                {
                    UserId = 1,
                    UserGuid = Guid.NewGuid(),
                    NameSurname = "Test User",
                    Email = "test@mail.com",
                    University = "KTU",
                    CreatedDate = DateTime.Now
                });

            userServiceMock
                .Setup(x => x.GetUserByIdAsync(It.Is<int>(id => id != 1)))
                .ReturnsAsync((UserBaseDto?)null);

            userServiceMock
                .Setup(x => x.UpdateUserAsync(It.IsAny<UserUpdateRequest>()))
                .ReturnsAsync(true);

            userServiceMock
                .Setup(x => x.DeleteUserAsync(1))
                .ReturnsAsync(true);

            var matchingServiceMock = new Mock<IMatchingService>();

            matchingServiceMock
                .Setup(x => x.CalculateMatchAsync(It.Is<CalculateMatchRequest>(r => r.User1Id == r.User2Id)))
                .ReturnsAsync((CalculateMatchRequest r) => new MatchResultDto
                {
                    User1Id = r.User1Id,
                    User2Id = r.User2Id,
                    MatchPercent = 0m,
                    Message = "A user cannot be matched with themselves."
                });

            matchingServiceMock
                .Setup(x => x.CalculateMatchAsync(It.Is<CalculateMatchRequest>(r => r.User1Id != r.User2Id)))
                .ReturnsAsync((CalculateMatchRequest r) => new MatchResultDto
                {
                    User1Id = Math.Min(r.User1Id, r.User2Id),
                    User2Id = Math.Max(r.User1Id, r.User2Id),
                    MatchPercent = 0.85m,
                    SharedQuestionCount = 3,
                    SubjectMatched = true,
                    SubjectMatchMode = r.SubjectMatchMode.ToString(),
                    SavedToDatabase = false,
                    Message = "Match calculated successfully."
                });

            services.AddSingleton(authServiceMock.Object);
            services.AddSingleton(userServiceMock.Object);
            services.AddSingleton(matchingServiceMock.Object);
        });
    }
}
