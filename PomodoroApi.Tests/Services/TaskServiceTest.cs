using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using PomodoroApi.Data;
using PomodoroApi.Services.Tasks;
using PomodoroApi.Services.Users;

namespace PomodoroAp.Tests.Services;

public class TaskServiceTest : IDisposable
{

    private static readonly DbConnection _connection;
    private static readonly DbContextOptions<ApplicationDbContext> _options;

    static TaskServiceTest()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new ApplicationDbContext(_options);
        context.Database.EnsureCreated();
    }

    static ApplicationDbContext CreateContext() => new(_options);

    static void ClearDatabase(ApplicationDbContext context)
    {
        context.RemoveRange(context.Tasks);
        context.SaveChanges();
    }

    static void InitializeDatabase(ApplicationDbContext context)
    {
        context.AddRange(
            new PomodoroApi.Models.Task
            {
                Title = "Test Task",
            },
            new PomodoroApi.Models.Task
            {
                Title = "Test Task 2",
                Done = true
            }
        );

        context.SaveChanges();
    }

    public void Dispose()
    {
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }

    [Collection("TaskService")]
    public class Get()
    {
        [Fact]
        public async void GetTasks_ShoulBeAbleToGetAllTasks()
        {
            var context = CreateContext();
            ClearDatabase(context);
            InitializeDatabase(context);

            var taskService = new TaskService(context, new Mock<IUserService>().Object);

            var result = await taskService.GetTasks();

            Assert.Equal(PomodoroApi.Services.ResultType.Success, result.Result);
            Assert.Equal(2, result.Tasks.Count);
        }

        [Fact]
        public async void GetTasks_ShoulBeAbleToGetTask()
        {
            var context = CreateContext();
            ClearDatabase(context);
            InitializeDatabase(context);

            var taskService = new TaskService(context, new Mock<IUserService>().Object);

            var tasksResult = await taskService.GetTasks();

            var task = tasksResult.Tasks[0];

            var result = await taskService.GetTask(task.Id);

            Assert.Equal(PomodoroApi.Services.ResultType.Success, result.Result);
            Assert.Equivalent(result.Task, task);
        }

    }

    [Collection("TaskService")]
    public class Create
    {

        [Fact]
        public async void CreateTask_ShouldBeAbleToCreateTask()
        {
            var context = CreateContext();
            ClearDatabase(context);
            InitializeDatabase(context);

            var taskService = new TaskService(context, new Mock<IUserService>().Object);

            var task = new PomodoroApi.Models.Task
            {
                Title = "Test Task 3"
            };

            var result = await taskService.CreateTask(task);

            Assert.Equal(PomodoroApi.Services.ResultType.Success, result.Result);
            Assert.Equivalent(result.Task, task);
        }
    }

    [Collection("TaskService")]
    public class Update
    {
        [Fact]
        public async void UpdateTask_ShouldBeAbleToUpdateTask()
        {
            var context = CreateContext();
            ClearDatabase(context);
            InitializeDatabase(context);

            var taskService = new TaskService(context, new Mock<IUserService>().Object);

            var tasksResult = await taskService.GetTasks();

            var task = tasksResult.Tasks.Where(task => task.Title == "Test Task").First();

            task.Title = "Updated Task";
            task.Done = true;

            var result = await taskService.UpdateTask(task, task.Id);

            Assert.Equal(PomodoroApi.Services.ResultType.Success, result.Result);
            Assert.Equivalent(result.Task, task);
        }
    }

    [Collection("TaskService")]
    public class Delete
    {
        [Fact]
        public async void DeleteTask_ShouldBeAbleToDeleteTask()
        {
            var context = CreateContext();
            ClearDatabase(context);
            InitializeDatabase(context);

            var taskService = new TaskService(context, new Mock<IUserService>().Object);

            var tasksResult = await taskService.GetTasks();

            var task = tasksResult.Tasks.Where(task => task.Title == "Test Task").First();

            var result = await taskService.DeleteTask(task.Id);

            tasksResult = await taskService.GetTasks();

            Assert.Equal(PomodoroApi.Services.ResultType.Success, result.Result);
            Assert.DoesNotContain(task, tasksResult.Tasks);
        }

        [Fact]
        public async void DeleteTask_ShouldBeAbleToDeleteFinishedTask()
        {
            var context = CreateContext();
            ClearDatabase(context);
            InitializeDatabase(context);

            var taskService = new TaskService(context, new Mock<IUserService>().Object);

            var finishedTask = (await taskService.GetTasks()).Tasks.Where(task => task.Done).First();

            var result = await taskService.DeleteFinishedTasks();
            var tasksResult = await taskService.GetTasks();

            Assert.Equal(PomodoroApi.Services.ResultType.Success, result.Result);
            Assert.DoesNotContain(finishedTask, tasksResult.Tasks);
        }

        [Fact]
        public async void DeleteAllTasks_ShouldBeAbleToDeleteAllTasks()
        {
            var context = CreateContext();
            ClearDatabase(context);
            InitializeDatabase(context);

            var taskService = new TaskService(context, new Mock<IUserService>().Object);

            var result = await taskService.DeleteAllTasks();
            var tasksResult = await taskService.GetTasks();

            Assert.Equal(PomodoroApi.Services.ResultType.Success, result.Result);
            Assert.Empty(tasksResult.Tasks);
        }
    }
}