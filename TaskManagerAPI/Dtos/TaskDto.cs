public class TaskDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.Tarefas;    
    public List<string> Categories { get; set; } = new List<string>();
    public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");
    public string? UpdatedAt { get; set; }
}

public enum TaskStatus
{
    Tarefas = 0,   
    Concluída = 1,  
    EmAndamento = 2 
}