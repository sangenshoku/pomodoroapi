using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PomodoroApi.Contracts.Tasks;
using PomodoroApi.Services.Tasks;

namespace PomodoroApi.Controllers;

[Authorize]
public class TasksController(ITaskService taskService) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await taskService.GetTasks();
        var tasks = result.Tasks.ConvertAll(task => MapTaskToResponse(task));

        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await taskService.GetTask(id);

        if (result.Result == Services.ResultType.Failure)
        {
            return NotFound();
        }

        return Ok(MapTaskToResponse(result.Task!));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        var result = await taskService.CreateTask(Models.Task.From(request));

        var task = result.Task;

        return CreatedAtGetTask(task);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request)
    {
        var result = await taskService.UpdateTask(Models.Task.From(request), id);

        if (result.Result == Services.ResultType.Failure)
        {
            return Problem();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await taskService.DeleteTask(id);

        if (result.Result == Services.ResultType.Failure)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAll([FromQuery] bool done = false)
    {
        var result = done ? await taskService.DeleteFinishedTasks() : await taskService.DeleteAllTasks();

        if (result.Result == Services.ResultType.Failure)
        {
            return NotFound();
        }

        return NoContent();
    }

    private CreatedAtActionResult CreatedAtGetTask(Models.Task task)
    {
        return CreatedAtAction(
            actionName: nameof(Get),
            routeValues: new { id = task.Id },
            value: MapTaskToResponse(task)
        );
    }

    private TaskResponse MapTaskToResponse(Models.Task task)
    {
        return new TaskResponse(
            task.Id,
            task.Title,
            task.CompletedPomodoros,
            task.EstimatedPomodoros,
            task.Done,
            task.CreatedAt,
            task.UpdatedAt
        );
    }
}