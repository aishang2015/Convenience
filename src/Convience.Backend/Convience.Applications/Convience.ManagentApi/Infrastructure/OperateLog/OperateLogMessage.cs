﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure.OperateLog
{
    public class OperateLogMessage
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public string Uri { get; set; }

        public string HttpResultCode { get; set; }

        public string RequestContent { get; set; }
    }
}