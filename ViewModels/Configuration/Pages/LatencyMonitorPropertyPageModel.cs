using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class LatencyMonitorPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel LatencyMonitorThreshold { get; set; }

        public LatencyMonitorPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            LatencyMonitorThreshold = MapToString("latency-monitor-threshold");
        }
    }
}