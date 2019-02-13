using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace Quickr.Models
{
    internal class DatabaseEntry: FolderEntry
    {
        public DatabaseEntry(int dbIndex, IEnumerable<RedisKey> keys): base(dbIndex, $"db{dbIndex}")
        {
            foreach (var key in keys.OrderBy(x => (string) x))
            {
                Add(key);
            }
        }
    }
}