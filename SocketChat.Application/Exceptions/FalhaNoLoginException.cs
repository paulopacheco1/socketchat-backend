using SocketChat.Domain.Exceptions;

namespace SocketChat.Application.Exceptions
{
    public class FalhaNoLoginException : AppException
    {
        public FalhaNoLoginException(string message = "") : base(BuildMessage(message))
        {
        }

        public static string BuildMessage(string message)
        {
            if (!string.IsNullOrEmpty(message)) return message;
            return "Falha no login. Verifique suas credenciais.";
        }
    }
}
