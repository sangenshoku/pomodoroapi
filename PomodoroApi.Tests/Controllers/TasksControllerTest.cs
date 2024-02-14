using Microsoft.AspNetCore.Mvc;
using Moq;
using PomodoroApi.Controllers;
using PomodoroApi.Services;
using PomodoroApi.Services.Tasks;

namespace PomodoroApi.Tests.Controllers;

public class TasksControllerTest
{
    [Fact]
    public async void GetTasks_ShouldReturnTasks()
    {
        var taskService = new Mock<ITaskService>();
        var controller = new TasksController(taskService.Object);

        List<Models.Task> tasks =
        [
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Test Task",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Test Task 2",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        ];

        taskService.Setup(x => x.GetTasks())
            .ReturnsAsync(new GetTasksResult
            {
                Result = ResultType.Success,
                Tasks = tasks
            });

        var result = await controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<List<PomodoroApi.Contracts.Tasks.TaskResponse>>(okResult.Value);

        var tasksResponse = (okResult.Value as List<PomodoroApi.Contracts.Tasks.TaskResponse>)!;

        Assert.Equal(2, tasksResponse.Count);
        Assert.Contains(tasksResponse, x => x.Title == "Test Task" && x.Id == tasks[0].Id);
    }

    [Fact]
    public async void GetTask_ShouldReturnTask()
    {
        var taskService = new Mock<ITaskService>();
        var controller = new TasksController(taskService.Object);

        var task = new Models.Task
        {
            Id = Guid.NewGuid(),
            Title = "Test Task",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        taskService.Setup(x => x.GetTask(task.Id))
            .ReturnsAsync(new GetTaskResult
            {
                Result = ResultType.Success,
                Task = task
            });

        var result = await controller.Get(task.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<PomodoroApi.Contracts.Tasks.TaskResponse>(okResult.Value);

        var taskResponse = (okResult.Value as PomodoroApi.Contracts.Tasks.TaskResponse)!;

        Assert.Equal(task.Id, taskResponse.Id);
        Assert.Equal(task.Title, taskResponse.Title);
    }

    [Fact]
    public async void Delete_ShouldReturnNoContent()
    {
        var taskService = new Mock<ITaskService>();
        var controller = new TasksController(taskService.Object);

        var taskId = Guid.NewGuid();

        taskService.Setup(x => x.DeleteTask(taskId))
            .ReturnsAsync(new DeletedTaskResult
            {
                Result = ResultType.Success
            });

        var result = await controller.Delete(taskId);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async void DeleteAll_ShouldBeAbleToDeleteAllTasks()
    {
        var taskService = new Mock<ITaskService>();
        var controller = new TasksController(taskService.Object);

        taskService.Setup(x => x.DeleteAllTasks())
            .ReturnsAsync(new DeletedTaskResult
            {
                Result = ResultType.Success
            });

        var result = await controller.DeleteAll();

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async void DeleteAll_ShouldBeAbleToDeleteFinishedTasks()
    {
        var taskService = new Mock<ITaskService>();
        var controller = new TasksController(taskService.Object);

        taskService.Setup(x => x.DeleteFinishedTasks())
            .ReturnsAsync(new DeletedTaskResult
            {
                Result = ResultType.Success
            });

        var result = await controller.DeleteAll(true);

        Assert.IsType<NoContentResult>(result);
    }
}