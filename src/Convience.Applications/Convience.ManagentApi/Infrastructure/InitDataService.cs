using Convience.Background;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure
{
    public class InitDataService : AbstractTimedBackgroundService
    {
        public InitDataService(InitDataJob backgroundJob) : base(backgroundJob)
        {
        }
    }
}
