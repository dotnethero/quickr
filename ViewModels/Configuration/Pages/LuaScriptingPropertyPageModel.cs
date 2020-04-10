using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Configuration
{
    internal class LuaScriptingPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel LuaTimeLimit { get; set; }

        public LuaScriptingPropertyPageModel(EndpointEntry endpoint, Dictionary<string, ConfigKeyValue> config): 
            base(endpoint, config)
        {
            LuaTimeLimit = MapToString("lua-time-limit");
        }
    }
}