using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Configuration
{
    class ActiveDefragmentationPropertyPageModel: BasePropertyPageModel
    {
        public YesNoPropertyModel ActiveDefrag { get; set; }
        public StringPropertyModel ActiveDefragIgnoreBytes { get; set; }
        public StringPropertyModel ActiveDefragThresholdLower { get; set; }
        public StringPropertyModel ActiveDefragThresholdUpper { get; set; }
        public StringPropertyModel ActiveDefragCycleMin { get; set; }
        public StringPropertyModel ActiveDefragCycleMax { get; set; }
        public StringPropertyModel ActiveDefragMaxScanFields { get; set; }

        public ActiveDefragmentationPropertyPageModel(EndpointEntry endpoint, Dictionary<string, ConfigKeyValue> config): 
            base(endpoint, config)
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