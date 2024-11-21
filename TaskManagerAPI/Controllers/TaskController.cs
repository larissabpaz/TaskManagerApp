using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
    public async Task<IActionResult> GetTasks()
    {
        var userEmailClaim = User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

        if (string.IsNullOrEmpty(userEmailClaim))
        {
            return Unauthorized("Usuário não autenticado.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim);
        if (user == null)
        {
            return Unauthorized("Usuário não encontrado.");
        }

        var tasks = await _context.Tasks
            .Where(t => t.UserId == user.Id)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                Category = t.Category
            })
            .ToListAsync();

        if (!tasks.Any())
        {
            return Ok(new { message = "Nenhuma tarefa encontrada." });
        }

        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var userEmailClaim = User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

        if (string.IsNullOrEmpty(userEmailClaim))
        {
            return Unauthorized("Usuário não autenticado.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim);
        if (user == null)
        {
            return Unauthorized("Usuário não encontrado.");
        }

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);
        if (task == null)
        {
            return NotFound("Tarefa não encontrada.");
        }

        return Ok(new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            Category = task.Category
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(TaskDto dto)
    {
        var userEmailClaim = User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

        if (string.IsNullOrEmpty(userEmailClaim))
        {
            return Unauthorized("Usuário não autenticado.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim);
        if (user == null)
        {
            return Unauthorized("Usuário não encontrado.");
        }

        var task = new Task
        {
            Title = dto.Title,
            Description = dto.Description,
            Category = dto.Category,
            Status = dto.Status,
            IsCompleted = dto.IsCompleted,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UserId = user.Id
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTask(int id, TaskUpdateDto dto)
    {
        var userEmailClaim = User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

        if (string.IsNullOrEmpty(userEmailClaim))
        {
            return Unauthorized("Usuário não autenticado.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim);
        if (user == null)
        {
            return Unauthorized("Usuário não encontrado.");
        }

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);
        if (task == null)
        {
            return NotFound("Tarefa não encontrada.");
        }

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;
        task.Category = dto.Category;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userEmailClaim = User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

        if (string.IsNullOrEmpty(userEmailClaim))
        {
            return Unauthorized("Usuário não autenticado.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmailClaim);
        if (user == null)
        {
            return Unauthorized("Usuário não encontrado.");
        }

        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);
        if (task == null)
        {
            return NotFound("Tarefa não encontrada.");
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
