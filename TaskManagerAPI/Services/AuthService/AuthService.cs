using Microsoft.EntityFrameworkCore;

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

