using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class RedisClusterPropertyPageModel: BasePropertyPageModel
    {
        public RedisClusterPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
        }
    }
}