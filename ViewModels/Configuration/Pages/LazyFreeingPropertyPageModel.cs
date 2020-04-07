using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class LazyFreeingPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel LazyfreeLazyEviction { get; set; }
        public StringPropertyModel LazyfreeLazyExpire { get; set; }
        public StringPropertyModel LazyfreeLazyServerDel { get; set; }
        public StringPropertyModel SlaveLazyFlush { get; set; }
        public StringPropertyModel ReplicaLazyFlush { get; set; }

        public LazyFreeingPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            LazyfreeLazyEviction = MapToString("lazyfree-lazy-eviction");
            LazyfreeLazyExpire = MapToString("lazyfree-lazy-expire");
            LazyfreeLazyServerDel = MapToString("lazyfree-lazy-server-del");
            SlaveLazyFlush = MapToString("slave-lazy-flush");
            ReplicaLazyFlush = MapToString("replica-lazy-flush");
        }
    }
}