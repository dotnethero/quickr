using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Configuration
{
    class EventNotificationPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel NotifyKeyspaceEvents { get; set; }

        public EventNotificationPropertyPageModel(EndpointEntry endpoint, Dictionary<string, ConfigKeyValue> config): 
            base(endpoint, config)
        {
            NotifyKeyspaceEvents = MapToString("notify-keyspace-events");
        }
    }
}