namespace PomodoroApi.Services.Tasks;


public record GetTasksResult : StandardServiceResult
{
    public List<Models.Task> Tasks { get; set; } = [];
}

public record GetTaskResult : StandardServiceResult
{
    public Models.Task? Task { get; set; }
}

public record CreatedTaskResult : StandardServiceResult
{
    public required Models.Task Task { get; set; }
}

public record UpdatedTaskResult : StandardServiceResult
{
    public Models.Task? Task { get; set; }
}

public record DeletedTaskResult : StandardServiceResult
{
    public Models.Task? Task { get; set; }
}