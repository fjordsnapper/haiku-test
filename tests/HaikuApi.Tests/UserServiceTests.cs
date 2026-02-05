using HaikuApi.Models;
using HaikuApi.Services;

namespace HaikuApi.Tests;

public class UserServiceTests
{
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userService = new UserService();
    }

    [Fact]
    public async Task CreateUser_WithValidData_ReturnsUserWithId()
    {
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        var result = await _userService.CreateUserAsync(user);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
        Assert.Equal("john@example.com", result.Email);
    }

    [Fact]
    public async Task GetUserById_WithValidId_ReturnsUser()
    {
        var user = new User
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane@example.com"
        };

        var createdUser = await _userService.CreateUserAsync(user);
        var result = await _userService.GetUserByIdAsync(createdUser.Id);

        Assert.NotNull(result);
        Assert.Equal(createdUser.Id, result.Id);
        Assert.Equal("Jane", result.FirstName);
    }

    [Fact]
    public async Task GetUserById_WithInvalidId_ReturnsNull()
    {
        var result = await _userService.GetUserByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsAllCreatedUsers()
    {
        var user1 = new User
        {
            FirstName = "User",
            LastName = "One",
            Email = "user1@example.com"
        };

        var user2 = new User
        {
            FirstName = "User",
            LastName = "Two",
            Email = "user2@example.com"
        };

        await _userService.CreateUserAsync(user1);
        await _userService.CreateUserAsync(user2);

        var result = await _userService.GetAllUsersAsync();

        Assert.NotNull(result);
        Assert.True(result.Count() >= 2);
    }

    [Fact]
    public async Task UpdateUser_WithValidData_UpdatesUserSuccessfully()
    {
        var user = new User
        {
            FirstName = "Original",
            LastName = "Name",
            Email = "original@example.com"
        };

        var createdUser = await _userService.CreateUserAsync(user);

        var updatedUser = new User
        {
            FirstName = "Updated",
            LastName = "Name",
            Email = "updated@example.com"
        };

        var result = await _userService.UpdateUserAsync(createdUser.Id, updatedUser);

        Assert.NotNull(result);
        Assert.Equal(createdUser.Id, result.Id);
        Assert.Equal("Updated", result.FirstName);
        Assert.Equal("updated@example.com", result.Email);
    }

    [Fact]
    public async Task UpdateUser_WithInvalidId_ReturnsNull()
    {
        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com"
        };

        var result = await _userService.UpdateUserAsync(999, user);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteUser_WithValidId_DeletesUserSuccessfully()
    {
        var user = new User
        {
            FirstName = "Delete",
            LastName = "Me",
            Email = "delete@example.com"
        };

        var createdUser = await _userService.CreateUserAsync(user);
        var result = await _userService.DeleteUserAsync(createdUser.Id);

        Assert.True(result);

        var deletedUser = await _userService.GetUserByIdAsync(createdUser.Id);
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task DeleteUser_WithInvalidId_ReturnsFalse()
    {
        var result = await _userService.DeleteUserAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task CreateUser_SetsTimestamps()
    {
        var user = new User
        {
            FirstName = "Time",
            LastName = "Test",
            Email = "time@example.com"
        };

        var beforeCreation = DateTime.UtcNow;
        var result = await _userService.CreateUserAsync(user);
        var afterCreation = DateTime.UtcNow;

        Assert.True(result.CreatedAt >= beforeCreation && result.CreatedAt <= afterCreation);
        Assert.True(result.UpdatedAt >= beforeCreation && result.UpdatedAt <= afterCreation);
    }

    [Fact]
    public async Task UpdateUser_UpdatesTimestamp()
    {
        var user = new User
        {
            FirstName = "Original",
            LastName = "User",
            Email = "original@example.com"
        };

        var createdUser = await _userService.CreateUserAsync(user);
        var originalUpdatedAt = createdUser.UpdatedAt;

        await Task.Delay(100);

        var updatedUser = new User
        {
            FirstName = "Modified",
            LastName = "User",
            Email = "modified@example.com"
        };

        var result = await _userService.UpdateUserAsync(createdUser.Id, updatedUser);

        Assert.NotNull(result);
        Assert.True(result.UpdatedAt > originalUpdatedAt);
    }
}
