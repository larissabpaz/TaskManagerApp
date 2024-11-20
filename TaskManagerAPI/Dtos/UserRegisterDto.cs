using System.ComponentModel.DataAnnotations;

    public class UserRegisterDto
    {
        [Required(ErrorMessage = "O campo usuário é obrigatório")]
        public string Usuario { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo email é obrigatório"), EmailAddress(ErrorMessage = "Email inválido!")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo senha é obrigatório")]
        public string Senha { get; set; } = string.Empty;
        public RoleEnum Role { get; set; }
    }
