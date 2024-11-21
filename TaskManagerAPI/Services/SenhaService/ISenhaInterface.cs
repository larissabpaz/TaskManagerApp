        public interface ISenhaInterface
        {
        void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt);
        string CriarToken(User usuario);
        }
