using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Convience.WebSockets
{
    public interface IWebSocketDataHandler
    {
        Task SendAllAsync(string data);

        Task HandleDataAsync(WebSocket webSocket, byte[] bytes);
    }
}
