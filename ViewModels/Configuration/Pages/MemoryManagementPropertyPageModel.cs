using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class MemoryManagementPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel Maxmemory { get; set; }
        public StringPropertyModel MaxmemoryPolicy { get; set; }
        public StringPropertyModel MaxmemorySamples { get; set; }
        public StringPropertyModel SlaveIgnoreMaxmemory { get; set; }
        public StringPropertyModel ReplicaIgnoreMaxmemory { get; set; }

        public MemoryManagementPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            Maxmemory = MapToString("maxmemory");
            MaxmemoryPolicy = MapToString("maxmemory-policy");
            MaxmemorySamples = MapToString("maxmemory-samples");
            SlaveIgnoreMaxmemory = MapToString("slave-ignore-maxmemory");
            ReplicaIgnoreMaxmemory = MapToString("replica-ignore-maxmemory");
        }
    }
}
