using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
     private readonly TaskManagerContext _context;

        public TasksController(TaskManagerContext context)
        {
            _context = context;
        }
    private static List<TaskDto> tasks = new List<TaskDto>();
    private static int nextId = 1;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
    {
        var tasks = await _context.Tasks.ToListAsync();
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask(TaskDto task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchTask(int id, [FromBody] UpdateTask updatedTask)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        if (!string.IsNullOrWhiteSpace(updatedTask.Title))
        {
            task.Title = updatedTask.Title;
        }

        if (!string.IsNullOrWhiteSpace(updatedTask.Description))
        {
            task.Description = updatedTask.Description;
        }

        if (!string.IsNullOrWhiteSpace(updatedTask.Status))
        {
            task.Status = updatedTask.Status;
        }

        if (!string.IsNullOrWhiteSpace(updatedTask.Category))
        {
            task.Category = updatedTask.Category;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
