namespace PomodoroApi.Contracts.Tasks;

public record CreateTaskRequest(
    string Title,
    ushort EstimatedPomodoros
);