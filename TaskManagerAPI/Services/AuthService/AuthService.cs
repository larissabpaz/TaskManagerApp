using Microsoft.EntityFrameworkCore;

// public class AuthService
// {
//     private readonly TaskManagerContext _context;
//     private readonly IConfiguration _configuration;

//     public AuthService(TaskManagerContext context, IConfiguration configuration)
//     {
//         _context = context;
//         _configuration = configuration;
//     }

//     public string Authenticate(User user)
//     {
//         var tokenHandler = new JwtSecurityTokenHandler();
//         var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
//         var tokenDescriptor = new SecurityTokenDescriptor
//         {
//             Subject = new ClaimsIdentity(new Claim[]
//             {
//                 new Claim(ClaimTypes.Name, user.Username),
//                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
//             }),
//             Expires = DateTime.UtcNow.AddHours(1),
//             Issuer = _configuration["Jwt:Issuer"],
//             Audience = _configuration["Jwt:Audience"],
//             SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//         };
//         var token = tokenHandler.CreateToken(tokenDescriptor);
//         return tokenHandler.WriteToken(token);
//     }

//     public async Task<string> Register(User user)
//     {
//         var existingUser = await _context.Users
//             .FirstOrDefaultAsync(u => u.Username == user.Username);

//         if (existingUser != null)
//         {
//             throw new Exception("User already exists.");
//         }

//         _context.Users.Add(user);
//         await _context.SaveChangesAsync();

//         return "User registered successfully";
//     }
// }

    public class AuthService : IAuthInterface
    {
        private readonly TaskManagerContext _context;
        private readonly ISenhaInterface _senhaInterface;
        public AuthService(TaskManagerContext context, ISenhaInterface senhaInterface)
        {
            _context = context;
            _senhaInterface = senhaInterface;
        }


        public async Task<Response<UserRegisterDto>> Registrar(UserRegisterDto usuarioRegistro)
        {
            Response<UserRegisterDto> respostaServico = new Response<UserRegisterDto>();

            try
            {
                if (!VerificaSeEmaileUsuarioJaExiste(usuarioRegistro))
                {
                    respostaServico.Dados = null;
                    respostaServico.Status = false;
                    respostaServico.Mensagem = "Email/Usuário já cadastrados!";
                    return respostaServico;
                }

                _senhaInterface.CriarSenhaHash(usuarioRegistro.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                User usuario = new User()
                {
                    Username = usuarioRegistro.Usuario,
                    Email = usuarioRegistro.Email,
                    Role = usuarioRegistro.Role,
                    PasswordHash = senhaHash,
                    PasswordSalt = senhaSalt
                };

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                respostaServico.Mensagem = "Usuário criado com sucesso!";

            }
            catch (Exception ex)
            {

                respostaServico.Dados = null;
                respostaServico.Mensagem = ex.Message;
                respostaServico.Status = false;

            }

            return respostaServico;
        }

        public async Task<Response<string>> Login(UserLoginDto usuarioLogin)
        {
            Response<string> respostaServico = new Response<string>();

            try
            {

                var usuario = await _context.Users.FirstOrDefaultAsync(userBanco => userBanco.Email == usuarioLogin.Email);

                if(usuario == null)
                {
                    respostaServico.Mensagem = "Credenciais inválidas!";
                    respostaServico.Status = false;
                    return respostaServico;
                }

                if (!_senhaInterface.VerificaSenhaHash(usuarioLogin.Senha, usuario.PasswordHash, usuario.PasswordSalt))
                {
                    respostaServico.Mensagem = "Credenciais inválidas!";
                    respostaServico.Status = false;
                    return respostaServico;
                }

                var token = _senhaInterface.CriarToken(usuario);

                respostaServico.Dados = token;
                respostaServico.Mensagem = "Usuário logado com sucesso!";

            }catch (Exception ex)
            {
                respostaServico.Dados = null;
                respostaServico.Mensagem = ex.Message;
                respostaServico.Status = false;
            }
            return respostaServico;
        }


        public bool VerificaSeEmaileUsuarioJaExiste(UserRegisterDto usuarioRegistro)
        {
            var usuario = _context.Users.FirstOrDefault(userBanco => userBanco.Email == usuarioRegistro.Email || userBanco.Username == usuarioRegistro.Usuario);

            if (usuario != null) return false;

            return true;
        }
    }

