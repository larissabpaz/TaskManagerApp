
    public interface IAuthInterface
    {
        Task<Response<UserRegisterDto>> Registrar(UserRegisterDto usuarioRegistro);
        Task<Response<string>> Login(UserLoginDto usuarioLogin);
    }
