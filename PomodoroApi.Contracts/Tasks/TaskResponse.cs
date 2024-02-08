namespace PomodoroApi.Contracts.Tasks;

public record TaskResponse(
    Guid Id,
    string Title,
    ushort CompletedPomodoros,
    ushort EstimatedPomodoros,
    bool Done,
    DateTime CreatedAt,
    DateTime UpdateAt
);
