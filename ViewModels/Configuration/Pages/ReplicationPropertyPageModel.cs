using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Configuration
{
    class ReplicationPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel ReplicaOf { get; set; }
        public StringPropertyModel MasterAuth { get; set; }
        public YesNoPropertyModel ReplicaServeStaleData { get; set; }
        public YesNoPropertyModel ReplicaReadOnly { get; set; }
        public YesNoPropertyModel ReplDisklessSync { get; set; }
        public StringPropertyModel ReplDisklessSyncDelay { get; set; }
        public StringPropertyModel ReplPingReplicaPeriod { get; set; }
        public StringPropertyModel ReplTimeout { get; set; }
        public YesNoPropertyModel ReplDisableTcpNodelay { get; set; }
        public StringPropertyModel ReplBacklogSize { get; set; }
        public StringPropertyModel ReplBacklogTtl { get; set; }
        public StringPropertyModel ReplicaPriority { get; set; }
        public StringPropertyModel MinReplicasToWrite { get; set; }
        public StringPropertyModel MinReplicasMaxLag { get; set; }
        public StringPropertyModel ReplicaAnnounceIp { get; set; }
        public StringPropertyModel ReplicaAnnouncePort { get; set; }

        public ReplicationPropertyPageModel(EndpointEntry endpoint, Dictionary<string, ConfigKeyValue> config): 
            base(endpoint, config)
        {
            var old =
                HasKey("slaveof") ||
                HasKey("slave-serve-stale-data") ||
                HasKey("slave-read-only") ||
                HasKey("slave-priority");

            ReplicaOf = old
                ? MapToString("slaveof") 
                : MapToString("replicaof");

            MasterAuth = MapToString("masterauth");

            ReplicaServeStaleData = old
                ? MapToYesNo("slave-serve-stale-data") 
                : MapToYesNo("replica-serve-stale-data");

            ReplicaReadOnly = old
                ? MapToYesNo("slave-read-only")
                : MapToYesNo("replica-read-only");

            ReplDisklessSync = MapToYesNo("repl-diskless-sync");
            ReplDisklessSyncDelay = MapToString("repl-diskless-sync-delay");
            
            ReplPingReplicaPeriod = old
                ? MapToString("repl-ping-slave-period")
                : MapToString("repl-ping-replica-period");

            ReplTimeout = MapToString("repl-timeout");
            ReplDisableTcpNodelay = MapToYesNo("repl-disable-tcp-nodelay");
            ReplBacklogSize = MapToString("repl-backlog-size");
            ReplBacklogTtl = MapToString("repl-backlog-ttl");

            ReplicaPriority = old
                ? MapToString("slave-priority")
                : MapToString("replica-priority");

            MinReplicasToWrite = old
                ? MapToString("min-slaves-to-write")
                : MapToString("min-replicas-to-write");

            MinReplicasMaxLag = old
                ? MapToString("min-slaves-max-lag")
                : MapToString("min-replicas-max-lag");

            ReplicaAnnounceIp = old
                ? MapToString("slave-announce-ip")
                : MapToString("replica-announce-ip");

            ReplicaAnnouncePort = old
                ? MapToString("slave-announce-port")
                : MapToString("replica-announce-port");
        }
    }
}