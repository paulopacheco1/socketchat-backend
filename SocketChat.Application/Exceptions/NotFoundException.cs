using SocketChat.Domain.Exceptions;
using SocketChat.Domain.SeedWork;
using System.Net;

namespace SocketChat.Application.Exceptions
{
    public class NotFoundException : AppException
    {
        public virtual string Resource { get; }

        public NotFoundException() : base("Recurso não encontrado", HttpStatusCode.NotFound)
        {
        }

        public NotFoundException(string resource) : base($"{resource} não encontrado", HttpStatusCode.NotFound)
        {
            Resource = resource;
        }

        public NotFoundException(string resource, string message) : base(message, HttpStatusCode.NotFound)
        {
            Resource = resource;
        }
    }

    public class NotFoundException<T> : NotFoundException where T : Entity
    {
        public NotFoundException() : base(typeof(T).Name)
        {
        }

        public NotFoundException(string message) : base(typeof(T).Name, message)
        {
        }
    }
}
