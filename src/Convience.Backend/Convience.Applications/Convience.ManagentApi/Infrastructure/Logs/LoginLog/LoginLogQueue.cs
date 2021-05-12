using System.Collections.Concurrent;

namespace Convience.ManagentApi.Infrastructure.Logs.LoginLog
{
    public class LoginLogQueue
    {
        public static BlockingCollection<LoginLogMessage> blockingCollection { get; private set; }
            = new BlockingCollection<LoginLogMessage>();
    }
}
