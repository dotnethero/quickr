using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class EventNotificationPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel NotifyKeyspaceEvents { get; set; }

        public EventNotificationPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            NotifyKeyspaceEvents = MapToString("notify-keyspace-events");
        }
    }
}