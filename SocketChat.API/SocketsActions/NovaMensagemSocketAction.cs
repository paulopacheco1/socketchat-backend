using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SocketChat.API.SocketsHandlers;
using SocketChat.API.SocketsManager;
using SocketChat.Application.Commands;
using SocketChat.Application.Queries;
using SocketChat.Application.ViewModels;
using SocketChat.Domain.Entities;
using wsAction = SocketChat.API.SocketsHandlers.wsAction;

namespace SocketChat.API.SocketsActions
{
    public class NovaMensagemSocketAction<THandler> : SocketAction where THandler : SocketHandler
    {
        private readonly THandler _handler;
        private readonly IMediator _mediator;
        private readonly ILogger<NovaMensagemSocketAction<THandler>> _logger;

        public NovaMensagemSocketAction(THandler handler, IMediator mediator, ILogger<NovaMensagemSocketAction<THandler>> logger)
        {
            _handler = handler;
            _mediator = mediator;
            _logger = logger;
        }

        public override async Task Execute(WebSocket socket, string message)
        {
            var mensagem = JsonConvert.DeserializeObject<NovaMensagemMessage>(message);

            var command = new ConversaAddMensagemCommand();
            command.IdRemetente = _handler.Connections.GetUserId(socket);
            command.IdConversa = mensagem.IdConversa;
            command.IdParticipantes = mensagem.IdParticipantes;
            command.Mensagem = mensagem.Mensagem;
            var conversaVWM = await _mediator.Send(command);

            var resposta = new Resposta<ConversaViewModel>()
            {
                Action = wsAction.NOVA_MENSAGEM,
                Data = conversaVWM,
            };

            var promises = conversaVWM.Participantes.Select(async participante => 
                await _handler.SendMessage(participante.Id, JsonConvert.SerializeObject(
                    resposta, 
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                ))
            );

            await Task.WhenAll(promises);
        }
    }

    public class NovaMensagemMessage : SocketMessage
    {
        public int? IdConversa { get; set; }
        public string Mensagem { get; set; }
        public List<int> IdParticipantes { get; set; }
    }
}