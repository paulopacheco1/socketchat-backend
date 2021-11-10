using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SocketChat.Domain.Repositories;

namespace SocketChat.Application.Queries
{
    public abstract class Query<TResponse> : IRequest<TResponse>
    {
    }

    public abstract class QueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : Query<TResponse>
    {
        protected readonly IUnitOfWork _unitOfWork;

        protected QueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}