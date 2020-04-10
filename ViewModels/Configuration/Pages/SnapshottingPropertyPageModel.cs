using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Configuration
{
    internal class SnapshottingPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel SaveProperty { get; set; }
        public YesNoPropertyModel StopWritesOnBgSaveError { get; set; }
        public YesNoPropertyModel RdbCompression { get; set; }
        public YesNoPropertyModel RdbChecksum { get; set; }
        public StringPropertyModel DbFilename { get; set; }
        public StringPropertyModel Dir { get; set; }

        public SnapshottingPropertyPageModel(EndpointEntry endpoint, Dictionary<string, ConfigKeyValue> config): 
            base(endpoint, config)
        {
            SaveProperty = MapToString("save");
            StopWritesOnBgSaveError = MapToYesNo("stop-writes-on-bgsave-error");
            RdbCompression = MapToYesNo("rdbcompression");
            RdbChecksum = MapToYesNo("rdbchecksum");
            DbFilename = MapToString("dbfilename");
            Dir  = MapToString("dir");
        }
    }
}