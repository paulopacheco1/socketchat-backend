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
using SocketChat.Application.Queries;
using SocketChat.Application.ViewModels;
using wsAction = SocketChat.API.SocketsHandlers.wsAction;

namespace SocketChat.API.SocketsActions
{
    public class BuscarSocketAction<THandler> : SocketAction where THandler : SocketHandler
    {
        private readonly THandler _handler;
        private readonly IMediator _mediator;
        private readonly ILogger<BuscarSocketAction<THandler>> _logger;

        public BuscarSocketAction(THandler handler, IMediator mediator, ILogger<BuscarSocketAction<THandler>> logger)
        {
            _handler = handler;
            _mediator = mediator;
            _logger = logger;
        }

        public override async Task Execute(WebSocket socket, string message)
        {
            var mensagem = JsonConvert.DeserializeObject<BuscarMessage>(message);

            var query = new ListConversasBuscaQuery();
            query.idParticipante = _handler.Connections.GetUserId(socket);
            query.Busca = mensagem.Busca;
            var conversas = await _mediator.Send(query);

            var resposta = new Resposta<List<ConversaViewModel>>()
            {
                Action = wsAction.BUSCAR,
                Data = conversas,
            };

            await _handler.SendMessage(socket, JsonConvert.SerializeObject(
                resposta,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            ));
        }
    }

    public class BuscarMessage : SocketMessage
    {
        public string Busca { get; set; }
    }
}