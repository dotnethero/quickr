using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class ShowLogPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel SlowlogLogSlowerThan { get; set; }
        public StringPropertyModel SlowlogMaxLen { get; set; }

        public ShowLogPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            SlowlogLogSlowerThan = MapToString("slowlog-log-slower-than");
            SlowlogMaxLen = MapToString("slowlog-max-len");
        }
    }
}