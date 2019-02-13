using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace Quickr.ViewModels.Database
{
    internal class DatabaseViewModel: FolderViewModel
    {
        public int Index { get; }

        public DatabaseViewModel(int index, IEnumerable<RedisKey> keys): base($"db{index}")
        {
            Index = index;
            foreach (var key in keys.OrderBy(x => (string) x))
            {
                Add(key);
            }
        }
    }
}