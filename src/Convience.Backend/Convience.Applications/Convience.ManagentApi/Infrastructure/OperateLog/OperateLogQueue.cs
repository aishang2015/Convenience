using System.Collections.Concurrent;

namespace Convience.ManagentApi.Infrastructure.OperateLog
{
    public class OperateLogQueue
    {
        public static ConcurrentQueue<OperateLogMessage> Queue { get; private set; }
            = new ConcurrentQueue<OperateLogMessage>();
    }
}
