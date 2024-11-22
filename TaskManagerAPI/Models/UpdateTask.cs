using System;

public class UpdateTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;    
    public TaskStatus Status { get; set; } = TaskStatus.Tarefas;    
    public List<string> Categories { get; set; } = new List<string>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}
