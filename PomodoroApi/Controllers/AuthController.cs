
using Microsoft.AspNetCore.Mvc;
using PomodoroApi.Contracts.Auth;
using PomodoroApi.ServiceErrors;
using PomodoroApi.Services.Auth;

namespace PomodoroApi.Controllers;

public class AuthController(IAuthService authService) : ApiController
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] PomodoroApi.Contracts.Auth.LoginRequest request)
    {
        var result = await authService.LoginAsync(request.Email, request.Password);

        if (!result.Succeeded)
        {
            return Unauthorized(Errors.Auth.InvalidCredentials);
        }

        return Ok(new AuthUserResponse(request.Email));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] PomodoroApi.Contracts.Auth.RegisterRequest request)
    {
        var result = await authService.RegisterAsync(request);

        if (result.Errors.Any())
        {
            return BadRequest(new { Description = "Invalid email or password.", result.Errors });
        }

        return StatusCode(201);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await authService.LogoutAsync();

        return Ok();
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var user = await authService.GetCurrentUserAsync();

        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(new AuthUserResponse(user.Email!));
    }
}