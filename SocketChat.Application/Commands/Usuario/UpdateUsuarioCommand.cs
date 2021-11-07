using SocketChat.Application.Exceptions;
using SocketChat.Domain.Aggregates;
using SocketChat.Domain.SeedWork;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChat.Application.Commands
{
    public class UpdateUsuarioCommand : Command<Unit>
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    public class UpdateUsuarioCommandHandler : CommandHandler<UpdateUsuarioCommand, Unit>
    {
        public UpdateUsuarioCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<Unit> Handle(UpdateUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _unitOfWork.Usuarios.GetAsync(request.Id);

            if (usuario == null) throw new NotFoundException<Usuario>();

            usuario.Nome = request.Nome;

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}