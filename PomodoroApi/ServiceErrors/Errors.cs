namespace PomodoroApi.ServiceErrors;

public static class Errors
{
    public static class Auth
    {
        public static Error InvalidCredentials = new("auth.invalid_credentials", "Invalid credentials");
    }
}