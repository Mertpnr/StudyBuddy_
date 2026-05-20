using FluentAssertions;
using Moq;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.UserRequest;
using StudyBuddy.API.Services;

namespace StudyBuddy.API.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly UserService _service;

    public UserServiceTests()
    {
        _service = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_WhenNameIsEmpty_ShouldThrowException()
    {
        var request = new UserCreateRequest
        {
            NameSurname = "",
            Email = "test@mail.com",
            Password = "123456",
            University = "KTU"
        };

        var action = async () => await _service.CreateUserAsync(request);

        await action.Should().ThrowAsync<Exception>()
            .WithMessage("Name surname is required.");
    }

    [Fact]
    public async Task CreateUserAsync_WhenEmailIsEmpty_ShouldThrowException()
    {
        var request = new UserCreateRequest
        {
            NameSurname = "Test User",
            Email = "",
            Password = "123456",
            University = "KTU"
        };

        var action = async () => await _service.CreateUserAsync(request);

        await action.Should().ThrowAsync<Exception>()
            .WithMessage("Email is required.");
    }

    [Fact]
    public async Task CreateUserAsync_WhenPasswordIsEmpty_ShouldThrowException()
    {
        var request = new UserCreateRequest
        {
            NameSurname = "Test User",
            Email = "test@mail.com",
            Password = "",
            University = "KTU"
        };

        var action = async () => await _service.CreateUserAsync(request);

        await action.Should().ThrowAsync<Exception>()
            .WithMessage("Password is required.");
    }

    [Fact]
    public async Task CreateUserAsync_WhenUniversityIsEmpty_ShouldThrowException()
    {
        var request = new UserCreateRequest
        {
            NameSurname = "Test User",
            Email = "test@mail.com",
            Password = "123456",
            University = ""
        };

        var action = async () => await _service.CreateUserAsync(request);

        await action.Should().ThrowAsync<Exception>()
            .WithMessage("University is required.");
    }

    [Fact]
    public async Task CreateUserAsync_WhenEmailAlreadyExists_ShouldThrowException()
    {
        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync("test@mail.com"))
            .ReturnsAsync(new User { UserId = 5, Email = "test@mail.com" });

        var request = new UserCreateRequest
        {
            NameSurname = "Test User",
            Email = " TEST@MAIL.COM ",
            Password = "123456",
            University = "KTU"
        };

        var action = async () => await _service.CreateUserAsync(request);

        await action.Should().ThrowAsync<Exception>()
            .WithMessage("A user with this email already exists.");
    }

    [Fact]
    public async Task CreateUserAsync_WhenRequestIsValid_ShouldNormalizeEmailHashPasswordAndInsertUser()
    {
        User? insertedUser = null;

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync("test@mail.com"))
            .ReturnsAsync((User?)null);

        _userRepositoryMock
            .Setup(x => x.InsertReturnId(It.IsAny<User>()))
            .Callback<User>(u => insertedUser = u)
            .ReturnsAsync(10);

        var request = new UserCreateRequest
        {
            NameSurname = "Test User",
            Email = " TEST@MAIL.COM ",
            Password = "123456",
            University = "KTU",
            Major = "Software Engineering",
            AboutMe = "Hello"
        };

        var result = await _service.CreateUserAsync(request);

        result.Should().Be(10);
        insertedUser.Should().NotBeNull();
        insertedUser!.Email.Should().Be("test@mail.com");
        insertedUser.NameSurname.Should().Be("Test User");
        insertedUser.UserGuid.Should().NotBe(Guid.Empty);
        insertedUser.PasswordHash.Should().NotBeNullOrWhiteSpace();
        insertedUser.PasswordHash.Should().NotBe("123456");
        BCrypt.Net.BCrypt.Verify("123456", insertedUser.PasswordHash).Should().BeTrue();

        _userRepositoryMock.Verify(x => x.InsertReturnId(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUserDoesNotExist_ShouldReturnFalse()
    {
        _userRepositoryMock
            .Setup(x => x.GetById(99))
            .ReturnsAsync((User?)null);

        var request = new UserUpdateRequest
        {
            UserId = 99,
            NameSurname = "Updated User",
            Email = "updated@mail.com",
            University = "KTU"
        };

        var result = await _service.UpdateUserAsync(request);

        result.Should().BeFalse();
        _userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserDoesNotExist_ShouldReturnFalse()
    {
        _userRepositoryMock
            .Setup(x => x.GetById(1))
            .ReturnsAsync((User?)null);

        var result = await _service.DeleteUserAsync(1);

        result.Should().BeFalse();
        _userRepositoryMock.Verify(x => x.Delete(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserExists_ShouldDeleteAndReturnTrue()
    {
        _userRepositoryMock
            .Setup(x => x.GetById(1))
            .ReturnsAsync(new User { UserId = 1 });

        _userRepositoryMock
            .Setup(x => x.Delete(1))
            .ReturnsAsync(true);

        var result = await _service.DeleteUserAsync(1);

        result.Should().BeTrue();
        _userRepositoryMock.Verify(x => x.Delete(1), Times.Once);
    }
}
