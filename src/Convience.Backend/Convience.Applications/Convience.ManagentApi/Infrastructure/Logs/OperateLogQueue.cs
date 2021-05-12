
using System.Collections.Concurrent;

namespace Convience.ManagentApi.Infrastructure.Logs
{
    public class OperateLogQueue
    {
        public static BlockingCollection<OperateLogMessage> blockingCollection { get; private set; }
            = new BlockingCollection<OperateLogMessage>();
    }
}
