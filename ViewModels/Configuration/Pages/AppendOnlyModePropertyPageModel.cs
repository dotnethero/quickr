using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class AppendOnlyModePropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel AppendOnly { get; set; }
        public StringPropertyModel AppendFilename { get; set; }
        public StringPropertyModel AppendFsync { get; set; }
        public StringPropertyModel NoAppendFsyncOnRewrite { get; set; }
        public StringPropertyModel AutoAofRewritePercentage { get; set; }
        public StringPropertyModel AutoAofRewriteMinSize { get; set; }
        public StringPropertyModel AofLoadTruncated { get; set; }
        public StringPropertyModel AofUseRdbPreamble { get; set; }

        public AppendOnlyModePropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            AppendOnly = MapToString("appendonly");
            AppendFilename = MapToString("appendfilename");
            AppendFsync = MapToString("appendfsync");
            NoAppendFsyncOnRewrite = MapToString("no-appendfsync-on-rewrite");
            AutoAofRewritePercentage = MapToString("auto-aof-rewrite-percentage");
            AutoAofRewriteMinSize = MapToString("auto-aof-rewrite-min-size");
            AofLoadTruncated = MapToString("aof-load-truncated");
            AofUseRdbPreamble = MapToString("aof-use-rdb-preamble");
        }
    }
}