using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class SecurityPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel Password { get; set; }

        public SecurityPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            Password = MapToString("requirepass");
        }
    }
}
