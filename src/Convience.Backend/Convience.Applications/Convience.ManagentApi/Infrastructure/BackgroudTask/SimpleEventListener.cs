using Convience.ManagentApi.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace Convience.ManagentApi.Infrastructure.BackgroudTask
{
    public class SimpleEventListener : EventListener
    {
        private readonly IHubContext<AppStateHub, IAppStateCient> _hub;

        public SimpleEventListener(IHubContext<AppStateHub, IAppStateCient> hub)
        {
            _hub = hub;
        }

        protected override void OnEventSourceCreated(EventSource source)
        {
            if (!source.Name.Equals("System.Runtime"))
            {
                return;
            }

            EnableEvents(source, EventLevel.Verbose, EventKeywords.All, new Dictionary<string, string>()
            {
                ["EventCounterIntervalSec"] = "2"
            });
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (!eventData.EventName.Equals("EventCounters"))
            {
                return;
            }

            for (int i = 0; i < eventData.Payload.Count; ++i)
            {
                if (eventData.Payload[i] is IDictionary<string, object> eventPayload)
                {
                    var (counterName, counterValue, counterUnit) = GetRelevantMetric(eventPayload);
                    switch (counterName)
                    {
                        case "cpu-usage":
                            _hub.Clients.All.HandleCpuUsage(counterValue);
                            break;
                        case "working-set":
                            _hub.Clients.All.HandleWorkingSet(counterValue);
                            break;

                        case "gc-heap-size":
                            _hub.Clients.All.HandleGCHeapSize(counterValue);
                            break;
                        case "gen-0-gc-count":
                            _hub.Clients.All.HandelGCCount(0, int.Parse(counterValue));
                            break;
                        case "gen-1-gc-count":
                            _hub.Clients.All.HandelGCCount(1, int.Parse(counterValue));
                            break;
                        case "gen-2-gc-count":
                            _hub.Clients.All.HandelGCCount(2, int.Parse(counterValue));
                            break;

                        case "threadpool-thread-count":
                            _hub.Clients.All.HandelThreadPoolThreadCount(counterValue);
                            break;
                        case "monitor-lock-contention-count":
                            _hub.Clients.All.HandelMonitorLockContentionCount(counterValue);
                            break;
                        case "threadpool-queue-length":
                            _hub.Clients.All.ThreadPoolQueueLength(counterValue);
                            break;
                        case "threadpool-completed-items-count":
                            _hub.Clients.All.ThreadPoolCompletedWorkItemCount(counterValue);
                            break;

                        case "gen-0-size":
                            _hub.Clients.All.HandelGcSize(0, long.Parse(counterValue));
                            break;
                        case "gen-1-size":
                            _hub.Clients.All.HandelGcSize(1, long.Parse(counterValue));
                            break;
                        case "gen-2-size":
                            _hub.Clients.All.HandelGcSize(2, long.Parse(counterValue));
                            break;
                        case "loh-size":
                            _hub.Clients.All.HandeLohSize(long.Parse(counterValue));
                            break;
                        case "poh-size":
                            _hub.Clients.All.HandelPohSize(long.Parse(counterValue));
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private static (string counterName, string counterValue, string counterUnit) GetRelevantMetric(
            IDictionary<string, object> eventPayload)
        {
            var counterName = "";
            var counterValue = "";
            var counterUnit = "";

            if (eventPayload.TryGetValue("Name", out object displayValue))
            {
                counterName = displayValue.ToString();
            }
            if (eventPayload.TryGetValue("Mean", out object value) ||
                eventPayload.TryGetValue("Increment", out value))
            {
                counterValue = value.ToString();
            }
            if (eventPayload.TryGetValue("DisplayUnits", out object unit))
            {
                counterUnit = unit.ToString();
            }

            return (counterName, counterValue, counterUnit);
        }
    }
}
