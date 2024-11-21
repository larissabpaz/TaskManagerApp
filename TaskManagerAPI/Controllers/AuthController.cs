using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthInterface _authInterface;
    private readonly IUserRepository _userRepository;
    private readonly ISenhaInterface _senhaService;

    public AuthController(IAuthInterface authInterface, IUserRepository userRepository, ISenhaInterface senhaService)
    {
        _authInterface = authInterface;
        _userRepository = userRepository;
        _senhaService = senhaService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto model)
    {
        var user = await _userRepository.GetUserByEmailAsync(model.Email);
        if (user == null)
            return Unauthorized("Email ou senha inválidos.");

        bool isPasswordValid = VerificarSenha(model.Senha, user.PasswordHash, user.PasswordSalt);
        if (!isPasswordValid)
            return Unauthorized("Email ou senha inválidos.");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Uvk46iYol0m57Jtq6rNCz/0NhEhYWnyn12V9UsolN8M="));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "https://localhost:7218",
            audience: "https://localhost:7218",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto dto)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return BadRequest("Email já cadastrado.");
        }

        byte[] passwordHash, passwordSalt;
        _senhaService.CriarSenhaHash(dto.Senha, out passwordHash, out passwordSalt);

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = (RoleEnum)dto.Role
        };

        var createdUser = await _userRepository.CreateUserAsync(user);

        return Ok(new { mensagem = "Usuário criado com sucesso!" });
    }

    private bool VerificarSenha(string senha, byte[] senhaHash, byte[] senhaSalt)
    {
        using (var hmac = new HMACSHA512(senhaSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return computedHash.SequenceEqual(senhaHash);
        }
    }
}
