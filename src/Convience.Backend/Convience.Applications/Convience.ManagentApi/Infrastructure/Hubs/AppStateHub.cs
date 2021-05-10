using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure.Hubs
{
    public class AppStateHub : Hub<IAppStateCient>
    {
    }
}
