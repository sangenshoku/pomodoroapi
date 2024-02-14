using Microsoft.AspNetCore.Mvc;
using Moq;
using PomodoroApi.Controllers;
using PomodoroApi.Contracts.Auth;
using PomodoroApi.Services.Auth;

namespace PomodoroApi.Tests.Controllers;

public class AuthControllerTest
{
    [Fact]
    public async void Login_ShoulAbleToLogin()
    {
        // Arrange
        var authService = new Mock<IAuthService>();
        var controller = new AuthController(authService.Object);
        var request = new LoginRequest("test@test.com", "P@ssword123");

        authService.Setup(x => x.LoginAsync(request.Email, request.Password))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        // Act
        var result = await controller.Login(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthUserResponse>(okResult.Value);

        Assert.Equal(request.Email, response.Username);
    }

    [Fact]
    public async void Register_ShouldAbleToRegister()
    {
        // Arrange
        var authService = new Mock<IAuthService>();
        var controller = new AuthController(authService.Object);
        var request = new RegisterRequest("test@test.com", "P@ssword123", "P@ssword123");

        authService.Setup(x => x.RegisterAsync(request))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.IdentityResult.Success);

        // Act
        var result = await controller.Register(request);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        var statusCode = statusCodeResult.StatusCode;

        Assert.Equal(201, statusCode);
    }

    [Fact]
    public async void Logout_ShouldAbleToLogout()
    {
        // Arrange
        var authService = new Mock<IAuthService>();
        var controller = new AuthController(authService.Object);

        // Act
        var result = await controller.Logout();

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
    }
}