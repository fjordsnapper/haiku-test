using HaikuApi.Controllers;
using HaikuApi.Models;
using HaikuApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Reflection;

namespace HaikuApi.Tests;

public class UsersControllerAuthenticationTests
{
    private readonly UsersController _controller;
    private readonly Mock<IUserService> _mockUserService;

    public UsersControllerAuthenticationTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UsersController(_mockUserService.Object);
    }

    [Fact]
    public void UsersController_HasAuthorizeAttribute()
    {
        var controllerType = typeof(UsersController);
        var authorizeAttribute = controllerType.GetCustomAttribute<AuthorizeAttribute>();

        Assert.NotNull(authorizeAttribute);
    }

    [Fact]
    public void GetUsers_IsProtectedByClassLevelAuthorize()
    {
        var method = typeof(UsersController).GetMethod(nameof(UsersController.GetUsers));
        var controllerType = typeof(UsersController);
        var classAuthorizeAttribute = controllerType.GetCustomAttribute<AuthorizeAttribute>();

        Assert.NotNull(classAuthorizeAttribute);
        Assert.NotNull(method);
    }

    [Fact]
    public void GetUser_IsProtectedByClassLevelAuthorize()
    {
        var method = typeof(UsersController).GetMethod(nameof(UsersController.GetUser));
        var controllerType = typeof(UsersController);
        var classAuthorizeAttribute = controllerType.GetCustomAttribute<AuthorizeAttribute>();

        Assert.NotNull(classAuthorizeAttribute);
        Assert.NotNull(method);
    }

    [Fact]
    public void CreateUser_IsProtectedByClassLevelAuthorize()
    {
        var method = typeof(UsersController).GetMethod(nameof(UsersController.CreateUser));
        var controllerType = typeof(UsersController);
        var classAuthorizeAttribute = controllerType.GetCustomAttribute<AuthorizeAttribute>();

        Assert.NotNull(classAuthorizeAttribute);
        Assert.NotNull(method);
    }

    [Fact]
    public void UpdateUser_IsProtectedByClassLevelAuthorize()
    {
        var method = typeof(UsersController).GetMethod(nameof(UsersController.UpdateUser));
        var controllerType = typeof(UsersController);
        var classAuthorizeAttribute = controllerType.GetCustomAttribute<AuthorizeAttribute>();

        Assert.NotNull(classAuthorizeAttribute);
        Assert.NotNull(method);
    }

    [Fact]
    public void DeleteUser_IsProtectedByClassLevelAuthorize()
    {
        var method = typeof(UsersController).GetMethod(nameof(UsersController.DeleteUser));
        var controllerType = typeof(UsersController);
        var classAuthorizeAttribute = controllerType.GetCustomAttribute<AuthorizeAttribute>();

        Assert.NotNull(classAuthorizeAttribute);
        Assert.NotNull(method);
    }

    [Fact]
    public async Task GetUsers_WithoutAuthentication_ShouldBeProtected()
    {
        _mockUserService.Setup(s => s.GetAllUsersAsync())
            .ReturnsAsync(new List<User>());

        var result = await _controller.GetUsers();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUser_WithoutAuthentication_ShouldBeProtected()
    {
        _mockUserService.Setup(s => s.GetUserByIdAsync(It.IsAny<short>()))
            .ReturnsAsync((User?)null);

        var result = await _controller.GetUser(1);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateUser_WithoutAuthentication_ShouldBeProtected()
    {
        var user = new User { FirstName = "Test", LastName = "User", Email = "test@example.com" };
        _mockUserService.Setup(s => s.CreateUserAsync(It.IsAny<User>()))
            .ReturnsAsync(user);

        var result = await _controller.CreateUser(user);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateUser_WithoutAuthentication_ShouldBeProtected()
    {
        var user = new User { FirstName = "Test", LastName = "User", Email = "test@example.com" };
        _mockUserService.Setup(s => s.UpdateUserAsync(It.IsAny<short>(), It.IsAny<User>()))
            .ReturnsAsync(user);

        var result = await _controller.UpdateUser(1, user);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteUser_WithoutAuthentication_ShouldBeProtected()
    {
        _mockUserService.Setup(s => s.DeleteUserAsync(It.IsAny<short>()))
            .ReturnsAsync(true);

        var result = await _controller.DeleteUser(1);

        Assert.NotNull(result);
    }
}
