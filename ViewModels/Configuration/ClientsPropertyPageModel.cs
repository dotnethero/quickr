using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class ClientsPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel MaxClients { get; set; }

        public ClientsPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            MaxClients = MapToString("maxclients");
        }
    }
}
