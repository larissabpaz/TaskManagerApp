using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Usuario")]
public class User
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Role {get; set;}
    public List<Task> Tasks { get; set; } = new List<Task>();
}