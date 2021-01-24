using Convience.WebSockets;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Convience.Util.Middlewares
{
    public class WebSocketMiddleware
    {

        private readonly RequestDelegate _next;

        private readonly ILogger<WebSocketMiddleware> _logger;

        private readonly string _path;

        public WebSocketMiddleware(RequestDelegate next,
            ILogger<WebSocketMiddleware> logger,
            string path)
        {
            _logger = logger;
            _next = next;
            _path = path;
        }

        public async Task Invoke(HttpContext context, IEnumerable<IWebSocketService> webSocketServices)
        {
            if (context.Request.Path == _path)
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    // 找到处理当前路径的service
                    var service = webSocketServices.Where(s => s.GetPath() == _path).FirstOrDefault();

                    if (service != null)
                    {
                        // 开始监听
                        await Listening(webSocket, service);
                    }
                }
            }

            await _next(context);
        }

        private async Task Listening(WebSocket webSocket, IWebSocketService webSocketService)
        {
            var buffer = new byte[1024 * 1];

            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                // 发送数据
                var data = webSocketService.GetSendData(null);
                if (data != null)
                {
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count),
                        WebSocketMessageType.Binary, true, CancellationToken.None);
                }

                // 处理数据
                buffer = new byte[1024 * 1];
                result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                await webSocketService.HandleReceivedData(result);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
