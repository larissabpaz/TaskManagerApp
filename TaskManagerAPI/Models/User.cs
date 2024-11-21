using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Usuario")]
public class User
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } 
    public byte[] PasswordSalt { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public RoleEnum Role {get; set;}
    public List<TaskDto> Tasks { get; set; } = new List<TaskDto>();
}