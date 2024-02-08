namespace PomodoroApi.Contracts.Tasks;

public record UpdateTaskRequest(
    string Title,
    ushort CompletedPomodoros,
    ushort EstimatedPomodoros,
    bool Done);