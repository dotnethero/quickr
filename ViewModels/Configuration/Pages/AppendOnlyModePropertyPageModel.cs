using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class AppendOnlyModePropertyPageModel: BasePropertyPageModel
    {
        public YesNoPropertyModel AppendOnly { get; set; }
        public StringPropertyModel AppendFilename { get; set; }
        public StringPropertyModel AppendFsync { get; set; }
        public YesNoPropertyModel NoAppendFsyncOnRewrite { get; set; }
        public StringPropertyModel AutoAofRewritePercentage { get; set; }
        public StringPropertyModel AutoAofRewriteMinSize { get; set; }
        public YesNoPropertyModel AofLoadTruncated { get; set; }
        public YesNoPropertyModel AofUseRdbPreamble { get; set; }

        public AppendOnlyModePropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            AppendOnly = MapToYesNo("appendonly");
            AppendFilename = MapToString("appendfilename");
            AppendFsync = MapToString("appendfsync");
            NoAppendFsyncOnRewrite = MapToYesNo("no-appendfsync-on-rewrite");
            AutoAofRewritePercentage = MapToString("auto-aof-rewrite-percentage");
            AutoAofRewriteMinSize = MapToString("auto-aof-rewrite-min-size");
            AofLoadTruncated = MapToYesNo("aof-load-truncated");
            AofUseRdbPreamble = MapToYesNo("aof-use-rdb-preamble");
        }
    }
}