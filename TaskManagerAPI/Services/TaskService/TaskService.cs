using Microsoft.EntityFrameworkCore;

public class TaskService
{
    private readonly TaskManagerContext _context;

    public TaskService(TaskManagerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskWithUserDto>> GetUserTasks(int userId)
    {
        return await _context.Tasks
            .Where(t => t.UserId == userId)
            .Select(t => new TaskWithUserDto
            {
                Id = t.Id.ToString(),
                Title = t.Title,
                Description = t.Description,
                Priority = t.Priority,
                Categories = t.Categories,
                Status = t.Status,
                CreatedAt = t.CreatedAt.ToString("o"),
                UpdatedAt = t.UpdatedAt.HasValue ? t.UpdatedAt.Value.ToString("o") : null,
                User = new UserLoginDto
                {
                    Id = t.User.Id,
                    Email = t.User.Email
                }
            })
            .ToListAsync();
    }
}
