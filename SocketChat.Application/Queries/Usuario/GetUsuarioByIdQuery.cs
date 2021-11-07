using SocketChat.Application.Exceptions;
using SocketChat.Domain.Aggregates;
using SocketChat.Domain.SeedWork;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChat.Application.Queries
{
    public class GetUsuarioByIdQuery : Query<UsuarioViewModel>
    {
        public int Id { get; set; }

        public GetUsuarioByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetUserByIdQueryHandler : QueryHandler<GetUsuarioByIdQuery, UsuarioViewModel>
    {
        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<UsuarioViewModel> Handle(GetUsuarioByIdQuery request, CancellationToken cancellationToken)
        {
            var usuario = await _unitOfWork.Usuarios.GetAsync(request.Id);

            if (usuario == null) throw new NotFoundException<Usuario>();

            return new UsuarioViewModel()
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
            };
        }
    }
}