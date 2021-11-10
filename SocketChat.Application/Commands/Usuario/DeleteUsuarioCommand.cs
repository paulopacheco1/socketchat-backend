using SocketChat.Application.Exceptions;
using SocketChat.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SocketChat.Domain.Repositories;

namespace SocketChat.Application.Commands
{
    public class DeleteUsuarioCommand : Command<Unit>
    {
        public int Id { get; set; }

    }

    public class DeleteUsuarioCommandHandler : CommandHandler<DeleteUsuarioCommand, Unit>
    {
        public DeleteUsuarioCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<Unit> Handle(DeleteUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _unitOfWork.Usuarios.GetAsync(request.Id);

            if (usuario == null) throw new NotFoundException<Usuario>();

            _unitOfWork.Usuarios.Remove(usuario);

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}