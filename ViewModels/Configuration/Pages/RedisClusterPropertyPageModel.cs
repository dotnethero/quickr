using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class RedisClusterPropertyPageModel: BasePropertyPageModel
    {
        public YesNoPropertyModel ClusterEnabled { get; set; }
        public StringPropertyModel ClusterConfigFile { get; set; }
        public StringPropertyModel ClusterNodeTimeout { get; set; }
        public StringPropertyModel ClusterReplicaValidityFactor { get; set; }
        public StringPropertyModel ClusterMigrationBarrier { get; set; }
        public YesNoPropertyModel ClusterRequireFullCoverage { get; set; }
        public YesNoPropertyModel ClusterReplicaNoFailover { get; set; }

        public RedisClusterPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            var old =
                HasKey("cluster-slave-no-failover") ||
                HasKey("cluster-slave-validity-factor");

            ClusterEnabled = MapToYesNo("cluster-enabled");
            ClusterConfigFile = MapToString("cluster-config-file");
            ClusterNodeTimeout = MapToString("cluster-node-timeout");
            ClusterReplicaValidityFactor = old
                ? MapToString("cluster-slave-validity-factor")
                : MapToString("cluster-replica-validity-factor");
            ClusterMigrationBarrier = MapToString("cluster-migration-barrier");
            ClusterRequireFullCoverage = MapToYesNo("cluster-require-full-coverage");
            ClusterReplicaNoFailover = old
                ? MapToYesNo("cluster-slave-no-failover")
                : MapToYesNo("cluster-replica-no-failover");
        }
    }
}