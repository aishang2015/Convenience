using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Convience.ManagentApi.Infrastructure.BackgroudTask
{
    public class SystemMonitorBackgroundService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var process = Process.GetCurrentProcess();
            var isUnix = Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX;
            var instanceName = isUnix ? string.Format("{0}/{1}", process.Id, process.ProcessName) : process.ProcessName;

            var m_CpuUsagePC = new PerformanceCounter("Process", "% Processor Time", instanceName);
            var m_ThreadCountPC = new PerformanceCounter("Process", "Thread Count", instanceName);
            var m_WorkingSetPC = new PerformanceCounter("Process", "Working Set", instanceName);

            while (true)
            {

                Console.WriteLine("Processor Time = " + m_CpuUsagePC.NextValue() + " %.");
                Console.WriteLine("Thread Count = " + m_ThreadCountPC.NextValue() + "");
                Console.WriteLine("Working Set = " + m_WorkingSetPC.NextValue() + "");

                Thread.Sleep(20000);
            }
        }
    }
}
