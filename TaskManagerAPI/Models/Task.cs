using System.ComponentModel.DataAnnotations.Schema;

[Table("Tarefas")]
public class Task
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public string Category { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
     public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}
