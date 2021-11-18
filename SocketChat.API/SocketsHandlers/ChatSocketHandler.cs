using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocketChat.API.SocketsActions;
using SocketChat.API.SocketsManager;
using SocketChat.Application.Exceptions;
using SocketChat.Domain.Exceptions;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketChat.API.SocketsHandlers
{
    public class ChatSocketHandler : SocketHandler
    {
        private readonly ILogger<ChatSocketHandler> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ChatSocketHandler(ConnectionManager connections, ILogger<ChatSocketHandler> logger, IServiceScopeFactory serviceScopeFactory) : base(connections)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
            var socketId = Connections.GetId(socket);
            _logger.LogInformation($"{socketId} connected.");
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var socketId = Connections.GetId(socket);
            _logger.LogInformation($"{socketId} left.");
            await base.OnDisconnected(socket);
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            try
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var action = JsonConvert.DeserializeObject<SocketMessage>(message).Action;

                var IsAuthenticated = Connections.IsAuthenticated(socket);
                if (!IsAuthenticated && action != wsAction.AUTH) throw new UnauthorizedException("Usuário não autenticado");

                using (var scope = _serviceScopeFactory.CreateScope())
                    switch (action)
                    {
                        case wsAction.AUTH:
                            await ((AuthSocketAction<ChatSocketHandler>)scope.ServiceProvider.GetService(typeof(AuthSocketAction<ChatSocketHandler>))).Execute(socket, message);
                            break;

                        case wsAction.BUSCAR:
                            await ((BuscarSocketAction<ChatSocketHandler>)scope.ServiceProvider.GetService(typeof(BuscarSocketAction<ChatSocketHandler>))).Execute(socket, message);
                            break;

                        case wsAction.NOVA_CONVERSA:
                        case wsAction.NOVA_MENSAGEM:
                            await ((NovaMensagemSocketAction<ChatSocketHandler>)scope.ServiceProvider.GetService(typeof(NovaMensagemSocketAction<ChatSocketHandler>))).Execute(socket, message);
                            break;

                        case wsAction.GET_MENSAGENS:
                            await ((GetMensagensSocketAction<ChatSocketHandler>)scope.ServiceProvider.GetService(typeof(GetMensagensSocketAction<ChatSocketHandler>))).Execute(socket, message);
                            break;

                        default:
                            throw new AppException("Action não identificada");
                    }

            }
            catch (AppException e)
            {
                _logger.LogError($"Error ao processar requisição WS: {e.Message}", e);
                await SendMessage(socket, e.Message);
            }
        }
    }

    public class SocketMessage
    {
        public wsAction Action { get; set; }
    }

    public class Resposta<T>
    {
        public wsAction Action { get; set; }
        public T Data { get; set; }
    }

    public enum wsAction
    {
        AUTH = 1,
        BUSCAR = 2,
        NOVA_CONVERSA = 3,
        NOVA_MENSAGEM = 4,
        GET_MENSAGENS = 5,
    }
}
