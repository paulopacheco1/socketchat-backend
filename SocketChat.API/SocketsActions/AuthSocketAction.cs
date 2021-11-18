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
    public class AuthSocketAction<THandler> : SocketAction where THandler : SocketHandler
    {
        private readonly THandler _handler;
        private readonly IMediator _mediator;
        private readonly ILogger<AuthSocketAction<THandler>> _logger;

        public AuthSocketAction(THandler handler, IMediator mediator, ILogger<AuthSocketAction<THandler>> logger)
        {
            _handler = handler;
            _mediator = mediator;
            _logger = logger;
        }

        public override async Task Execute(WebSocket socket, string message)
        {
            var mensagem = JsonConvert.DeserializeObject<AuthMessage>(message);

            _handler.Connections.Authenticate(socket, mensagem.Token);
            _logger.LogInformation($"{_handler.Connections.GetId(socket)} successfully authenticated.");

            var query = new ListConversasQuery();
            query.idParticipante = _handler.Connections.GetUserId(socket);
            var conversas = await _mediator.Send(query);

            var resposta = new Resposta<List<ConversaViewModel>>()
            {
                Action = wsAction.AUTH,
                Data = conversas,
            };

            await _handler.SendMessage(socket, JsonConvert.SerializeObject(
                resposta,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            ));
        }
    }

    public class AuthMessage : SocketMessage
    {
        public string Token { get; set; }
    }
}