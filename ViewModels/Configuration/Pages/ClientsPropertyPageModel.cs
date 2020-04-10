using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Configuration
{
    internal class ClientsPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel MaxClients { get; set; }

        public ClientsPropertyPageModel(EndpointEntry endpoint, Dictionary<string, ConfigKeyValue> config): 
            base(endpoint, config)
        {
            MaxClients = MapToString("maxclients");
        }
    }
}
