using System.ComponentModel.DataAnnotations;

public class UserLoginDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "O campo email é obrigatório"), EmailAddress(ErrorMessage = "Email inválido!")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "O campo senha é obrigatória")]
    public string Senha { get; set; } = string.Empty;
}