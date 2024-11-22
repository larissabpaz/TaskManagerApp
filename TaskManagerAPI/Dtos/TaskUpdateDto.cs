public class TaskUpdateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.Tarefas;
    public List<string> Categories { get; set; } = new List<string>();
}