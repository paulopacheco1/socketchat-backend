using SocketChat.Domain.Exceptions;
using System.Net;

namespace SocketChat.Application.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException() : base("Não foi prossivel processar a requisição", HttpStatusCode.Unauthorized)
        {
        }

        public UnauthorizedException(string message) : base(message, HttpStatusCode.Unauthorized)
        {
        }
    }
}
