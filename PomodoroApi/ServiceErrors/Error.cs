namespace PomodoroApi.ServiceErrors;

public record Error(
    string Code,
    string Description
);