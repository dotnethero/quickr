using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class ClusterNatSupportPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel ClusterAnnounceIp { get; set; }
        public StringPropertyModel ClusterAnnouncePort { get; set; }
        public StringPropertyModel ClusterAnnounceBusPort { get; set; }

        public ClusterNatSupportPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            ClusterAnnounceIp = MapToString("cluster-announce-ip");
            ClusterAnnouncePort = MapToString("cluster-announce-port");
            ClusterAnnounceBusPort = MapToString("cluster-announce-bus-port");
        }
    }
}