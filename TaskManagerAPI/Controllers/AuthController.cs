using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.ComponentModel.DataAnnotations;

// public class LoginModel
// {
//     [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
//     public string Username { get; set; } = string.Empty;

//     [Required(ErrorMessage = "A senha é obrigatória.")]
//     public string Password { get; set; } = string.Empty;
// }

// [Route("api/[controller]")]
// [ApiController]
// public class AuthController : ControllerBase
// {
//     [HttpPost("login")]
//     public IActionResult Login([FromBody] LoginModel model)
//     {
//         if (model.Username != "admin" || model.Password != "password")
//             return Unauthorized();

//         var claims = new[] 
//         {
//             new Claim(ClaimTypes.Name, model.Username),
//             new Claim(ClaimTypes.Role, "Admin")
//         };

//         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuaChaveSecretaAqui"));
//         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//         var token = new JwtSecurityToken(
//             issuer: "SeuIssuer",
//             audience: "SuaAudience",
//             claims: claims,
//             expires: DateTime.Now.AddHours(1),
//             signingCredentials: creds
//         );

//         return Ok(new
//         {
//             token = new JwtSecurityTokenHandler().WriteToken(token)
//         });
//     }
// }
[Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authInterface;
        public AuthController(IAuthInterface authInterface)
        {
                _authInterface = authInterface;
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginDto usuarioLogin)
        {

            var resposta = await _authInterface.Login(usuarioLogin); 
            return Ok(resposta);
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterDto usuarioRegister)
        {

            var resposta = await _authInterface.Registrar(usuarioRegister);
            return Ok(resposta);
        }

    }