using Convience.Entity.Data;

using Microsoft.AspNetCore.SignalR;

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure
{
    public class TestHub : Hub
    {
        public static ConcurrentDictionary<string, string> Onlines = new ConcurrentDictionary<string, string>();

        public TestHub()
        {
        }

        public override Task OnConnectedAsync()
        {
            var user = Context.User;
            var connectionId = Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
