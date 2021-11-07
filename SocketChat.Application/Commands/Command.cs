using SocketChat.Domain.SeedWork;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChat.Application.Commands
{
    public abstract class Command<TResponse> : IRequest<TResponse>
    {
    }

    public abstract class CommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : Command<TResponse>
    {
        protected readonly IUnitOfWork _unitOfWork;

        protected CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}