using FluentAssertions;
using Moq;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.AuthRequest;
using StudyBuddy.API.Services;
using StudyBuddy.API.UnitTests.TestHelpers;

namespace StudyBuddy.API.UnitTests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _service = new AuthService(_userRepositoryMock.Object, MapperTestHelper.CreateMapper());
    }

    [Fact]
    public async Task LoginAsync_WhenEmailIsEmpty_ShouldFail()
    {
        var request = new LoginRequest
        {
            Email = "",
            Password = "123456"
        };

        var result = await _service.LoginAsync(request);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Email is required.");
        result.User.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsEmpty_ShouldFail()
    {
        var request = new LoginRequest
        {
            Email = "test@mail.com",
            Password = ""
        };

        var result = await _service.LoginAsync(request);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Password is required.");
        result.User.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_WhenUserDoesNotExist_ShouldFail()
    {
        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync("test@mail.com"))
            .ReturnsAsync((User?)null);

        var request = new LoginRequest
        {
            Email = " test@mail.com ",
            Password = "123456"
        };

        var result = await _service.LoginAsync(request);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Invalid email or password.");
        result.User.Should().BeNull();

        _userRepositoryMock.Verify(x => x.GetByEmailAsync("test@mail.com"), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsWrong_ShouldFail()
    {
        var user = new User
        {
            UserId = 1,
            UserGuid = Guid.NewGuid(),
            NameSurname = "Test User",
            Email = "test@mail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct-password"),
            University = "KTU",
            CreatedDate = DateTime.Now
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync("test@mail.com"))
            .ReturnsAsync(user);

        var request = new LoginRequest
        {
            Email = "test@mail.com",
            Password = "wrong-password"
        };

        var result = await _service.LoginAsync(request);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Invalid email or password.");
        result.User.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsAreCorrect_ShouldSucceed()
    {
        var userGuid = Guid.NewGuid();

        var user = new User
        {
            UserId = 1,
            UserGuid = userGuid,
            NameSurname = "Test User",
            Email = "test@mail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            University = "KTU",
            Major = "Software Engineering",
            CreatedDate = DateTime.Now
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync("test@mail.com"))
            .ReturnsAsync(user);

        var request = new LoginRequest
        {
            Email = "TEST@MAIL.COM",
            Password = "123456"
        };

        var result = await _service.LoginAsync(request);

        result.Success.Should().BeTrue();
        result.Message.Should().Be("Login successful.");
        result.User.Should().NotBeNull();
        result.User!.UserId.Should().Be(1);
        result.User.UserGuid.Should().Be(userGuid);
        result.User.Email.Should().Be("test@mail.com");

        _userRepositoryMock.Verify(x => x.GetByEmailAsync("test@mail.com"), Times.Once);
    }
}
