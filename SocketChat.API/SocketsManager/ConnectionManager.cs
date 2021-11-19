using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using SocketChat.API.AccessContexts;
using SocketChat.Application.Exceptions;
using SocketChat.Domain.Providers;

namespace SocketChat.API.SocketsManager
{
    public class ConnectionManager
    {
        private readonly IAuthServiceProvider _authService;
        private ConcurrentDictionary<string, Connection> _connections = new();

        public ConnectionManager(IAuthServiceProvider authService)
        {
            _authService = authService;
        }

        public WebSocket GetConnectionById(string id)
        {
            return _connections.FirstOrDefault(x => x.Key == id).Value.Socket;
        }

        public List<WebSocket> GetUserConnections(int idUser)
        {
            return _connections.Where(x => x.Value.AccessContext.UserId == idUser).Select(c => c.Value.Socket).ToList();
        }

        public ConcurrentDictionary<string, Connection> GetAllConnections()
        {
            return _connections;
        }

        public string GetId(WebSocket socket)
        {
            return _connections.FirstOrDefault(x => x.Value.Socket == socket).Key;
        }

        public async Task RemoveConnectionAsync(string id)
        {
            _connections.TryRemove(id, out var conn);
            await conn.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket connection closed", CancellationToken.None);
        }

        public void AddConnection(WebSocket socket)
        {
            _connections.TryAdd(GetConnectionId(), new Connection() { Socket = socket });
        }

        private string GetConnectionId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public void Authenticate(WebSocket socket, string token)
        {
            var connection = _connections.FirstOrDefault(x => x.Value.Socket == socket).Value;
            var accessContextProvider = new WsAccessContextProvider(_authService, token);
            connection.AccessContext = accessContextProvider.Get();
        }

        public bool IsAuthenticated(WebSocket socket)
        {
            var connection = _connections.FirstOrDefault(x => x.Value.Socket == socket).Value;
            if (connection.AccessContext == null) return false;
            return true;
        }

        public int GetUserId(WebSocket socket)
        {
            var conn = _connections.FirstOrDefault(x => x.Value.Socket == socket).Value;
            if (conn.AccessContext == null) throw new UnauthorizedException("Usuário não autenticado");
            return conn.AccessContext.UserId;
        }
    }

    public class Connection
    {
        public WebSocket Socket { get; set; }
        public AccessContext AccessContext { get; set; }
    }
}
