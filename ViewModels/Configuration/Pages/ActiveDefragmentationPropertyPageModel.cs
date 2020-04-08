using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class ActiveDefragmentationPropertyPageModel: BasePropertyPageModel
    {
        public YesNoPropertyModel ActiveDefrag { get; set; }
        public StringPropertyModel ActiveDefragIgnoreBytes { get; set; }
        public StringPropertyModel ActiveDefragThresholdLower { get; set; }
        public StringPropertyModel ActiveDefragThresholdUpper { get; set; }
        public StringPropertyModel ActiveDefragCycleMin { get; set; }
        public StringPropertyModel ActiveDefragCycleMax { get; set; }
        public StringPropertyModel ActiveDefragMaxScanFields { get; set; }

        public ActiveDefragmentationPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            ActiveDefrag = MapToYesNo("activedefrag");
            ActiveDefragIgnoreBytes = MapToString("active-defrag-ignore-bytes");
            ActiveDefragThresholdLower = MapToString("active-defrag-threshold-lower");
            ActiveDefragThresholdUpper = MapToString("active-defrag-threshold-upper");
            ActiveDefragCycleMin = MapToString("active-defrag-cycle-min");
            ActiveDefragCycleMax = MapToString("active-defrag-cycle-max");
            ActiveDefragMaxScanFields = MapToString("active-defrag-max-scan-fields");
        }
    }
}