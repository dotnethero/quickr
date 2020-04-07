using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class RedisClusterPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel ClusterEnabled { get; set; }
        public StringPropertyModel ClusterConfigFile { get; set; }
        public StringPropertyModel ClusterNodeTimeout { get; set; }
        public StringPropertyModel ClusterSlaveValidityFactor { get; set; }
        public StringPropertyModel ClusterReplicaValidityFactor { get; set; }
        public StringPropertyModel ClusterMigrationBarrier { get; set; }
        public StringPropertyModel ClusterRequireFullCoverage { get; set; }
        public StringPropertyModel ClusterSlaveNoFailover { get; set; }
        public StringPropertyModel ClusterReplicaNoFailover { get; set; }

        public RedisClusterPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            ClusterEnabled = MapToString("cluster-enabled");
            ClusterConfigFile = MapToString("cluster-config-file");
            ClusterNodeTimeout = MapToString("cluster-node-timeout");
            ClusterSlaveValidityFactor = MapToString("cluster-slave-validity-factor");
            ClusterReplicaValidityFactor = MapToString("cluster-replica-validity-factor");
            ClusterMigrationBarrier = MapToString("cluster-migration-barrier");
            ClusterRequireFullCoverage = MapToString("cluster-require-full-coverage");
            ClusterSlaveNoFailover = MapToString("cluster-slave-no-failover");
            ClusterReplicaNoFailover = MapToString("cluster-replica-no-failover");
        }
    }
}