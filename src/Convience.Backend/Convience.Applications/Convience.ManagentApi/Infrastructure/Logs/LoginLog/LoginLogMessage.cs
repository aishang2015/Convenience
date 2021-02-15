
using System;

namespace Convience.ManagentApi.Infrastructure.Logs.LoginLog
{
    public class LoginLogMessage
    {
        public string OperatorAccount { get; set; }

        public string OperatorName { get; set; }

        public DateTime OperateAt { get; set; }

        public string IpAddress { get; set; }

        public bool IsSuccess { get; set; }
    }
}
