using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class AdvancedConfigPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel HashMaxZiplistEntries { get; set; }
        public StringPropertyModel HashMaxZiplistValue { get; set; }
        public StringPropertyModel ListMaxZiplistSize { get; set; }
        public StringPropertyModel ListCompressDepth { get; set; }
        public StringPropertyModel SetMaxIntsetEntries { get; set; }
        public StringPropertyModel ZsetMaxZiplistEntries { get; set; }
        public StringPropertyModel ZsetMaxZiplistValue { get; set; }
        public StringPropertyModel HllSparseMaxBytes { get; set; }
        public StringPropertyModel StreamNodeMaxBytes { get; set; }
        public StringPropertyModel StreamNodeMaxEntries { get; set; }
        public StringPropertyModel Activerehashing { get; set; }
        public StringPropertyModel ClientOutputBufferLimit { get; set; }
        public StringPropertyModel ClientQueryBufferLimit { get; set; }
        public StringPropertyModel ProtoMaxBulkLen { get; set; }
        public StringPropertyModel Hz { get; set; }
        public StringPropertyModel DynamicHz { get; set; }
        public StringPropertyModel AofRewriteIncrementalFsync { get; set; }
        public StringPropertyModel RdbSaveIncrementalFsync { get; set; }
        public StringPropertyModel LfuLogFactor { get; set; }
        public StringPropertyModel LfuDecayTime { get; set; }

        public AdvancedConfigPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            HashMaxZiplistEntries = MapToString("hash-max-ziplist-entries");
            HashMaxZiplistValue = MapToString("hash-max-ziplist-value");
            ListMaxZiplistSize = MapToString("list-max-ziplist-size");
            ListCompressDepth = MapToString("list-compress-depth");
            SetMaxIntsetEntries = MapToString("set-max-intset-entries");
            ZsetMaxZiplistEntries = MapToString("zset-max-ziplist-entries");
            ZsetMaxZiplistValue = MapToString("zset-max-ziplist-value");
            HllSparseMaxBytes = MapToString("hll-sparse-max-bytes");
            StreamNodeMaxBytes = MapToString("stream-node-max-bytes");
            StreamNodeMaxEntries = MapToString("stream-node-max-entries");
            Activerehashing = MapToString("activerehashing");
            ClientOutputBufferLimit = MapToString("client-output-buffer-limit");
            ClientQueryBufferLimit = MapToString("client-query-buffer-limit");
            ProtoMaxBulkLen = MapToString("proto-max-bulk-len");
            Hz = MapToString("hz");
            DynamicHz = MapToString("dynamic-hz");
            AofRewriteIncrementalFsync = MapToString("aof-rewrite-incremental-fsync");
            RdbSaveIncrementalFsync = MapToString("rdb-save-incremental-fsync");
            LfuLogFactor = MapToString("lfu-log-factor");
            LfuDecayTime = MapToString("lfu-decay-time");
        }
    }
}