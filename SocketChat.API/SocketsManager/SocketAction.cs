using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SocketChat.API.SocketsManager
{
    public abstract class SocketAction
    {
        public abstract Task Execute(WebSocket socket, string message);
    }
}