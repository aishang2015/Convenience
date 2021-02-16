using System.Collections.Concurrent;

namespace Convience.ManagentApi.Infrastructure.Logs.LoginLog
{
    public class LoginLogQueue
    {
        public static ConcurrentQueue<LoginLogMessage> Queue { get; private set; }
            = new ConcurrentQueue<LoginLogMessage>();
    }
}
