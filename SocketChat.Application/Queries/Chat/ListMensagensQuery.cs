using SocketChat.Application.Exceptions;
using SocketChat.Domain.Entities;
using SocketChat.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChat.Application.Queries
{
    public class ListMensagensQuery : Query<List<Mensagem>>
    {
        public int IdParticipante { get; set; }
        public int IdConversa { get; set; }
        public int IdMensagemMaisAntiga { get; set; }
    }

    public class ListMensagensQueryHandler : QueryHandler<ListMensagensQuery, List<Mensagem>>
    {
        public ListMensagensQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<List<Mensagem>> Handle(ListMensagensQuery request, CancellationToken cancellationToken)
        {
            var participante = await _unitOfWork.Usuarios.GetAsync(request.IdParticipante);
            if (participante == null) throw new NotFoundException("Participante");

            var mensagemMaisAntiga = await _unitOfWork.Mensagens.GetAsync(request.IdMensagemMaisAntiga);
            if (mensagemMaisAntiga == null) throw new NotFoundException<Mensagem>();

            var mensagens = await _unitOfWork.Mensagens.ListAsync(request.IdConversa, new MensagemFilter()
            {
                BeforeDate = mensagemMaisAntiga.DataEnvio,
            });

            return mensagens;
        }
    }
}