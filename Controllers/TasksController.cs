using Microsoft.AspNetCore.Mvc;
using WebApiLab2.Models;

namespace WebApiLab2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private static readonly List<TaskItem> Tasks = new();

    [HttpGet]
    public ActionResult<List<TaskItem>> GetAll() => Ok(Tasks);

    [HttpGet("{id:int}")]
    public ActionResult<TaskItem> GetById(int id)
    {
        var task = Tasks.FirstOrDefault(item => item.Id == id);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpPost]
    public ActionResult<TaskItem> Create(TaskItem task)
    {
        if (string.IsNullOrWhiteSpace(task.Title)) return BadRequest("Title is required.");
        task.Id = Tasks.Count == 0 ? 1 : Tasks.Max(item => item.Id) + 1;
        Tasks.Add(task);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id:int}")]
    public ActionResult<TaskItem> Update(int id, TaskItem updatedTask)
    {
        if (string.IsNullOrWhiteSpace(updatedTask.Title)) return BadRequest("Title is required.");
        var task = Tasks.FirstOrDefault(item => item.Id == id);
        if (task is null) return NotFound();
        task.Title = updatedTask.Title;
        task.Description = updatedTask.Description;
        task.IsCompleted = updatedTask.IsCompleted;
        return Ok(task);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var task = Tasks.FirstOrDefault(item => item.Id == id);
        if (task is null) return NotFound();
        Tasks.Remove(task);
        return NoContent();
    }
}
