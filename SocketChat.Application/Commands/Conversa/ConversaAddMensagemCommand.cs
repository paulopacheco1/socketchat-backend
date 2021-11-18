using SocketChat.Application.Exceptions;
using SocketChat.Domain.Entities;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using SocketChat.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using SocketChat.Domain.Exceptions;
using SocketChat.Application.ViewModels;

namespace SocketChat.Application.Commands
{
    public class ConversaAddMensagemCommand : Command<ConversaViewModel>
    {
        public int IdRemetente { get; set; }
        public int? IdConversa { get; set; }
        public List<int> IdParticipantes { get; set; }
        public string Mensagem { get; set; }
    }

    public class ConversaAddMensagemCommandHandler : CommandHandler<ConversaAddMensagemCommand, ConversaViewModel>
    {
        public ConversaAddMensagemCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<ConversaViewModel> Handle(ConversaAddMensagemCommand request, CancellationToken cancellationToken)
        {
            Conversa conversa;

            if (request.IdConversa == null)
            {
                if (request.IdParticipantes == null || request.IdParticipantes.Count < 2) throw new AppException("Participantes inválidos");
                conversa = await _unitOfWork.Conversas.GetAsync(request.IdParticipantes);

                if (conversa == null)
                {
                    var participantes = new List<Usuario>();
                    foreach (var idParticipante in request.IdParticipantes)
                    {
                        participantes.Add(await _unitOfWork.Usuarios.GetAsync(idParticipante));
                    }

                    if (participantes.Any(p => p == null)) throw new AppException("Participantes inválidos");

                    conversa = Conversa.Create(participantes);
                    await _unitOfWork.Conversas.AddAsync(conversa);
                    await _unitOfWork.CommitAsync();
                }
            }
            else
            {
                conversa = await _unitOfWork.Conversas.GetAsync((int)request.IdConversa);
            }

            var remetente = await _unitOfWork.Usuarios.GetAsync(request.IdRemetente);
            var mensagem = Mensagem.Create(conversa, remetente, request.Mensagem);
            await _unitOfWork.Mensagens.AddAsync(mensagem);
            await _unitOfWork.CommitAsync();

            return new ConversaViewModel()
            {
                Id = conversa.Id,
                Mensagens = new List<Mensagem>() { mensagem },
                Nome = conversa.GetDisplayName(remetente),
                Participantes = conversa.Participantes.Select(p => new ConversaParticipanteViewModel()
                {
                    Id = p.Id,
                    Nome = p.Nome,
                }).ToList(),
            };
        }
    }
}