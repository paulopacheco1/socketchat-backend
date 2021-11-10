using SocketChat.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChat.Application.Queries
{
    public class ListUsuariosQuery : Query<List<UsuarioViewModel>>
    {
        public ListUsuariosQuery(UsuarioFilter filter)
        {
            Filter = filter;
        }

        public UsuarioFilter Filter { get; set; }
    }

    public class ListUsuariosQueryHandler : QueryHandler<ListUsuariosQuery, List<UsuarioViewModel>>
    {
        public ListUsuariosQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<List<UsuarioViewModel>> Handle(ListUsuariosQuery request, CancellationToken cancellationToken)
        {
            var usuarios = await _unitOfWork.Usuarios.ListAsync(request.Filter);

            return usuarios.Select(user => new UsuarioViewModel()
            {
                Id = user.Id,
                Nome = user.Nome,
                Email = user.Email,
            }).ToList();
        }
    }
}