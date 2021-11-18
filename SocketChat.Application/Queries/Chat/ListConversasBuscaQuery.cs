using SocketChat.Application.Exceptions;
using SocketChat.Application.ViewModels;
using SocketChat.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChat.Application.Queries
{
    public class ListConversasBuscaQuery : Query<List<ConversaViewModel>>
    {
        public int idParticipante { get; set; }
        public string Busca { get; set; }
    }

    public class ListConversasBuscaQueryHandler : QueryHandler<ListConversasBuscaQuery, List<ConversaViewModel>>
    {
        public ListConversasBuscaQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<List<ConversaViewModel>> Handle(ListConversasBuscaQuery request, CancellationToken cancellationToken)
        {
            var participante = await _unitOfWork.Usuarios.GetAsync(request.idParticipante);
            if (participante == null) throw new NotFoundException("Participante");

            var conversaFilter = new ConversaFilter()
            {
                BuscaNomeOuParticipante = request.Busca,
            };

            var conversas = await _unitOfWork.Conversas.ListAsync(request.idParticipante, conversaFilter);

            var participantesFilter = new UsuarioFilter()
            {
                Nome = request.Busca,
                PageSize = 50,
            };

            var participantes = (await _unitOfWork.Usuarios.ListAsync(participantesFilter))
                .Where(p => p.Id != request.idParticipante)
                .Where(p => !conversas.Any(c => c.Participantes.Count == 2 && c.Participantes.Any(pp => pp.Id == p.Id)))
                .ToList();

            var conversasVWM = conversas.Select(conversa => new ConversaViewModel()
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

            var participantesVWM = participantes.Select(p => new ConversaViewModel()
            {
                Id = 0,
                Nome = p.Nome,
                Mensagens = new List<Domain.Entities.Mensagem>(),
                Participantes = new List<ConversaParticipanteViewModel>()
                {
                    new ConversaParticipanteViewModel()
                    {
                        Id = participante.Id,
                        Nome = participante.Nome,
                    },
                    new ConversaParticipanteViewModel()
                    {
                        Id = p.Id,
                        Nome = p.Nome,
                    },
                }
            }).ToList();

            conversasVWM.AddRange(participantesVWM);

            return conversasVWM;
        }
    }
}