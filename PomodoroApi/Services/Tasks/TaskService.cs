using Microsoft.EntityFrameworkCore;
using PomodoroApi.Contracts.Tasks;
using PomodoroApi.Data;
using PomodoroApi.Services.Users;

namespace PomodoroApi.Services.Tasks;

public class TaskService(ApplicationDbContext context, IUserService userService) : ITaskService
{
    public async Task<GetTasksResult> GetTasks()
    {
        var user = await userService.GetCurrentUser();
        var tasks = await context.Tasks.Where(task => task.User == user).ToListAsync();

        return new GetTasksResult
        {
            Result = ResultType.Success,
            Tasks = tasks
        };
    }

    public async Task<GetTaskResult> GetTask(Guid id)
    {
        var user = await userService.GetCurrentUser();
        var task = await context.Tasks.FirstOrDefaultAsync(task => task.User == user && task.Id == id);

        return new GetTaskResult
        {
            Result = task is not null ? ResultType.Success : ResultType.Failure,
            Task = task
        };
    }

    public async Task<CreatedTaskResult> CreateTask(Models.Task task)
    {
        var user = await userService.GetCurrentUser();

        if (user is not null)
        {
            task.User = user;
        }

        await context.Tasks.AddAsync(task);
        await context.SaveChangesAsync();

        return new CreatedTaskResult()
        {
            Result = ResultType.Success,
            Task = task,
        };
    }

    public async Task<UpdatedTaskResult> UpdateTask(Models.Task task, Guid id)
    {
        var storedTask = await context.Tasks.FindAsync(id);

        if (storedTask is null)
        {
            return new UpdatedTaskResult
            {
                Result = ResultType.Failure,
                Task = storedTask
            };
        }

        storedTask.Title = task.Title;
        storedTask.CompletedPomodoros = task.CompletedPomodoros;
        storedTask.EstimatedPomodoros = task.EstimatedPomodoros;
        storedTask.Done = task.Done;

        await context.SaveChangesAsync();

        return new UpdatedTaskResult
        {
            Result = ResultType.Success,
            Task = storedTask
        };
    }

    public async Task<DeletedTaskResult> DeleteTask(Guid id)
    {
        var user = await userService.GetCurrentUser();
        var task = await context.Tasks.FirstOrDefaultAsync(task => task.User == user && task.Id == id);
        bool taskExists = task is not null;

        if (taskExists)
        {
            context.Tasks.Remove(task!);
            await context.SaveChangesAsync();
        }

        return new DeletedTaskResult
        {
            Result = taskExists ? ResultType.Success : ResultType.Failure,
            Task = task,
        };
    }

    public async Task<DeletedTaskResult> DeleteAllTasks()
    {
        var user = await userService.GetCurrentUser();
        var tasks = await context.Tasks.Where(task => task.User == user).ToListAsync();

        context.Tasks.RemoveRange(tasks);
        await context.SaveChangesAsync();

        return new DeletedTaskResult
        {
            Result = ResultType.Success,
            Task = null,
        };
    }
}