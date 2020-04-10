using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Configuration
{
    internal class SecurityPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel Password { get; set; }

        public SecurityPropertyPageModel(EndpointEntry endpoint, Dictionary<string, ConfigKeyValue> config): 
            base(endpoint, config)
        {
            Password = MapToString("requirepass");
        }
    }
}
