using MediatR;
using SocketChat.API.SocketsManager;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketChat.API.SocketsManager
{
    public abstract class SocketHandler
    {
        public ConnectionManager Connections { get; set; }

        public SocketHandler(ConnectionManager connections)
        {
            Connections = connections;
        }

        public virtual async Task OnConnected(WebSocket socket)
        {
            await Task.Run(() => { Connections.AddConnection(socket); });
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await Connections.RemoveConnectionAsync(Connections.GetId(socket));
        }

        public async Task SendMessage(WebSocket socket, string message)
        {
            if (socket == null || socket.State != WebSocketState.Open) return;

            var msgBytes = Encoding.UTF8.GetBytes(message);
            var msgArrSeg = new ArraySegment<byte>(msgBytes, 0, msgBytes.Length);
            await socket.SendAsync(msgArrSeg, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task SendMessage(string id, string message)
        {
            await SendMessage(Connections.GetConnectionById(id), message);
        }

        public async Task SendMessage(int userId, string message)
        {
            var userConnections = Connections.GetUserConnections(userId);
            foreach (var socket in userConnections) await SendMessage(socket, message);
        }

        public async Task SendMessageToAll(string message)
        {
            foreach (var con in Connections.GetAllConnections()) await SendMessage(con.Value.Socket, message);
        }

        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
