using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PomodoroApi.Repositories;



public interface ITaskRepository
{
    Task<Models.Task> CreateTask(Models.Task task);
    Task<Models.Task?> DeleteTask(string id);
}