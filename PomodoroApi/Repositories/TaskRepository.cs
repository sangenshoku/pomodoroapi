using PomodoroApi.Data;

namespace PomodoroApi.Repositories;

public class TaskRepository(ApplicationDbContext context) : ITaskRepository
{
    public async Task<Models.Task> CreateTask(Models.Task task)
    {
        await context.Tasks.AddAsync(task);
        await context.SaveChangesAsync();
        return task;
    }

    public async Task<Models.Task?> DeleteTask(string id)
    {
        var task = await context.Tasks.FindAsync(id);
        if (task is not null)
        {
            context.Tasks.Remove(task);
            await context.SaveChangesAsync();
        }
        return task;
    }
}