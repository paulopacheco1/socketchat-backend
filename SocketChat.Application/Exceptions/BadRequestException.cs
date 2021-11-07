using SocketChat.Domain.Exceptions;
using System.Net;

namespace SocketChat.Application.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException() : base("Não foi prossivel processar a requisição", HttpStatusCode.BadRequest)
        {
        }

        public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
        {
        }
    }
}
