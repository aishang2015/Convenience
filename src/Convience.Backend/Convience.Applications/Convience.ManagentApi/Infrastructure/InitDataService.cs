using Convience.Background;

namespace Convience.ManagentApi.Infrastructure
{
    public class InitDataService : AbstractTimedBackgroundService
    {
        public InitDataService(InitDataJob backgroundJob) : base(backgroundJob)
        {
        }
    }
}
