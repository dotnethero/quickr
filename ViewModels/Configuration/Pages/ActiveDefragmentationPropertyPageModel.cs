using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class ActiveDefragmentationPropertyPageModel: BasePropertyPageModel
    {
        public ActiveDefragmentationPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
        }
    }
}