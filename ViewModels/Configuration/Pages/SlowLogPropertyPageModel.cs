using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Configuration
{
    internal class SlowLogPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel SlowlogLogSlowerThan { get; set; }
        public StringPropertyModel SlowlogMaxLen { get; set; }

        public SlowLogPropertyPageModel(EndpointEntry endpoint, Dictionary<string, ConfigKeyValue> config): 
            base(endpoint, config)
        {
            SlowlogLogSlowerThan = MapToString("slowlog-log-slower-than");
            SlowlogMaxLen = MapToString("slowlog-max-len");
        }
    }
}