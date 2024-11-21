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
            .Where(t => t.Id == userId)
            .Select(t => new TaskWithUserDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                Category = t.Category,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                User = new UserLoginDto
                {
                    Id = t.Id,
                    Email = t.,
                    // Username = t.Username
                }
            })
            .ToListAsync();
    }
}
