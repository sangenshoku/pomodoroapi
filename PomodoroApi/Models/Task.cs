using PomodoroApi.Contracts.Tasks;
using PomodoroApi.Data;

namespace PomodoroApi.Models;

public class Task
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public ushort CompletedPomodoros { get; set; }
    public ushort EstimatedPomodoros { get; set; }
    public bool Done { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }


    public static Task Create(
        string title,
        ushort completedPomodoros,
        ushort estimatedPomodoros,
        bool done,
        Guid? id = null,
        ApplicationUser? user = null)
    {
        return new Task
        {
            Id = id ?? Guid.NewGuid(),
            Title = title,
            CompletedPomodoros = completedPomodoros,
            EstimatedPomodoros = estimatedPomodoros,
            Done = done,
            User = user
        };
    }

    public static Task From(CreateTaskRequest request)
    {
        return Create(
            request.Title,
            0,
            request.EstimatedPomodoros,
            false,
            null,
            null
        );
    }

    public static Task From(UpdateTaskRequest request)
    {
        return Create(
            request.Title,
            request.CompletedPomodoros,
            request.EstimatedPomodoros,
            request.Done,
            null,
            null
        );
    }

}