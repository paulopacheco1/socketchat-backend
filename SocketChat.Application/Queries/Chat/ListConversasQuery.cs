using SocketChat.Application.Exceptions;
using SocketChat.Application.ViewModels;
using SocketChat.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChat.Application.Queries
{
    public class ListConversasQuery : Query<List<ConversaViewModel>>
    {
        public ConversaFilter Filter { get; set; } = new ConversaFilter();
        public int idParticipante { get; set; }
    }

    public class ListConversasQueryHandler : QueryHandler<ListConversasQuery, List<ConversaViewModel>>
    {
        public ListConversasQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<List<ConversaViewModel>> Handle(ListConversasQuery request, CancellationToken cancellationToken)
        {
            var participante = await _unitOfWork.Usuarios.GetAsync(request.idParticipante);
            if (participante == null) throw new NotFoundException("Participante");

            var conversas = await _unitOfWork.Conversas.ListAsync(request.idParticipante, request.Filter);

            return conversas.Select(conversa => new ConversaViewModel()
            {
                Id = conversa.Id,
                Nome = conversa.GetDisplayName(participante),
                Mensagens = conversa.Mensagens,
                Participantes = conversa.Participantes.Select(p => new ConversaParticipanteViewModel()
                {
                    Id = p.Id,
                    Nome = p.Nome,
                }).ToList(),
            }).ToList();
        }
    }
}