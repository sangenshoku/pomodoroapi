namespace PomodoroApi.Services.Tasks;

public interface ITaskService
{
    Task<GetTasksResult> GetTasks();
    Task<GetTaskResult> GetTask(Guid id);
    Task<CreatedTaskResult> CreateTask(Models.Task task);
    Task<UpdatedTaskResult> UpdateTask(Models.Task task, Guid id);
    Task<DeletedTaskResult> DeleteTask(Guid id);
    Task<DeletedTaskResult> DeleteAllTasks();
}