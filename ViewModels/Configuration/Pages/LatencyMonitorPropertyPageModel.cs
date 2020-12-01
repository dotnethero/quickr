using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Configuration
{
    class LatencyMonitorPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel LatencyMonitorThreshold { get; set; }

        public LatencyMonitorPropertyPageModel(EndpointEntry endpoint, Dictionary<string, ConfigKeyValue> config): 
            base(endpoint, config)
        {
            LatencyMonitorThreshold = MapToString("latency-monitor-threshold");
        }
    }
}