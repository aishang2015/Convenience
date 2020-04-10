using Microsoft.AspNetCore.Http;

using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Convience.WebSockets
{
    public class WebSocketManagerMiddleware
    {

        private readonly RequestDelegate _next;

        private WebSocketConnectionManager _webSocketConnectionManager;

        private IWebSocketDataHandler _webSocketDataHandler;

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    _webSocketConnectionManager.Add(webSocket);

                    // 开始监听
                    await Listening(webSocket);
                }
            }

            await _next(context);
        }

        private async Task Listening(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await _webSocketDataHandler.HandleDataAsync(webSocket, buffer);
                buffer = new byte[1024 * 4];
                result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            _webSocketConnectionManager.Remove(webSocket);
        }
    }
}
