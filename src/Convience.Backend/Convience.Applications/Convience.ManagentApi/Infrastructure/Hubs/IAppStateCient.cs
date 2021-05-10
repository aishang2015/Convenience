using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure.Hubs
{
    public interface IAppStateCient
    {
        Task HandleCpuUsage(string message);
        Task HandleWorkingSet(string message);
        Task HandleGCHeapSize(string message);
        Task HandelGCCount(int gen, int count);

        Task HandelThreadPoolThreadCount(string message);
        Task HandelMonitorLockContentionCount(string message);
        Task ThreadPoolQueueLength(string message);
        Task ThreadPoolCompletedWorkItemCount(string message);

        Task HandelGcSize(int gen, long count);
        Task HandeLohSize(long size);
        Task HandelPohSize(long size);
    }
}
