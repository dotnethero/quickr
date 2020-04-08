﻿using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class LuaScriptingPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel LuaTimeLimit { get; set; }

        public LuaScriptingPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            LuaTimeLimit = MapToString("lua-time-limit");
        }
    }
}