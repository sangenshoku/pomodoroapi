namespace PomodoroApi.Contracts.Auth;
public record RegisterRequest(
    string Email,
    string Password);