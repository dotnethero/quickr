using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Configuration
{
    internal class LazyFreeingPropertyPageModel: BasePropertyPageModel
    {
        public YesNoPropertyModel LazyfreeLazyEviction { get; set; }
        public YesNoPropertyModel LazyfreeLazyExpire { get; set; }
        public YesNoPropertyModel LazyfreeLazyServerDel { get; set; }
        public YesNoPropertyModel ReplicaLazyFlush { get; set; }

        public LazyFreeingPropertyPageModel(EndpointEntry endpoint, Dictionary<string, ConfigKeyValue> config): 
            base(endpoint, config)
        {
            LazyfreeLazyEviction = MapToYesNo("lazyfree-lazy-eviction");
            LazyfreeLazyExpire = MapToYesNo("lazyfree-lazy-expire");
            LazyfreeLazyServerDel = MapToYesNo("lazyfree-lazy-server-del");
            ReplicaLazyFlush = HasKey("slave-lazy-flush")
                ? MapToYesNo("slave-lazy-flush")
                : MapToYesNo("replica-lazy-flush");
        }
    }
}