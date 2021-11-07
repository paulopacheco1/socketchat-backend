namespace SocketChat.Application.Commands
{
    public class SessaoViewModel
    {
        public string Email { get; private set; }
        public string Token { get; private set; }

        public SessaoViewModel(string email, string token)
        {
            Email = email;
            Token = token;
        }
    }
}