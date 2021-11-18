namespace SocketChat.Application.ViewModels
{
    public class SessaoViewModel
    {
        public SessaoUsuarioViewModel Usuario { get; private set; }
        public string Token { get; private set; }

        public SessaoViewModel(SessaoUsuarioViewModel usuario, string token)
        {
            Usuario = usuario;
            Token = token;
        }
    }

    public class SessaoUsuarioViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}