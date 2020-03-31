using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Convience.WebSockets
{
    public static class WebSocketExtension
    {
        public static void AddWebSocketManager<TWebSocketDataHandler>(this IServiceCollection services)
               where TWebSocketDataHandler : class, IWebSocketDataHandler
        {
            services.AddSingleton<WebSocketConnectionManager>();
            services.AddSingleton<IWebSocketDataHandler, TWebSocketDataHandler>();
        }

        public static void UseWebSocketManager(this IApplicationBuilder app)
        {
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

            app.UseWebSockets(webSocketOptions);
            app.UseMiddleware<WebSocketManagerMiddleware>();
        }
    }
}
