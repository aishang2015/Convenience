namespace Convience.ManagentApi.Infrastructure.Logs
{
    public class OperateLogMessage
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public string Uri { get; set; }

        public string HttpResultCode { get; set; }

        public string RequestContent { get; set; }

        public string Account { get; set; }

        public string Name { get; set; }
    }
}
