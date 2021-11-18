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
using SocketChat.Domain.Entities;
using wsAction = SocketChat.API.SocketsHandlers.wsAction;

namespace SocketChat.API.SocketsActions
{
    public class GetMensagensSocketAction<THandler> : SocketAction where THandler : SocketHandler
    {
        private readonly THandler _handler;
        private readonly IMediator _mediator;
        private readonly ILogger<GetMensagensSocketAction<THandler>> _logger;

        public GetMensagensSocketAction(THandler handler, IMediator mediator, ILogger<GetMensagensSocketAction<THandler>> logger)
        {
            _handler = handler;
            _mediator = mediator;
            _logger = logger;
        }

        public override async Task Execute(WebSocket socket, string message)
        {
            var mensagem = JsonConvert.DeserializeObject<GetMensagensMessage>(message);

            var command = new ListMensagensQuery();
            command.IdParticipante = _handler.Connections.GetUserId(socket);
            command.IdConversa = mensagem.IdConversa;
            command.IdMensagemMaisAntiga = mensagem.IdMensagemMaisAntiga;
            var mensagens = await _mediator.Send(command);

            var resposta = new Resposta<GetMensagensResposta>()
            {
                Action = wsAction.GET_MENSAGENS,
                Data = new GetMensagensResposta()
                {
                    IdConversa = mensagem.IdConversa,
                    Mensagens = mensagens,
                },
            };

            await _handler.SendMessage(socket, JsonConvert.SerializeObject(
                resposta,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            ));
        }
    }

    public class GetMensagensMessage : SocketMessage
    {
        public int IdConversa { get; set; }
        public int IdMensagemMaisAntiga { get; set; }
    }

    public class GetMensagensResposta
    {
        public int IdConversa { get; set; }
        public List<Mensagem> Mensagens { get; set; }
    }
}