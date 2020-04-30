using Quickr.Models.Keys;
using Quickr.ViewModels.Data;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Quickr.Utils
{
    internal class KeyViewModelFactory
    {
        public async Task<BaseKeyViewModel> Create(KeyEntry key)
        {
            var (type, ttl) = await key.GetProperties();
            return type switch
            {
                RedisType.String => new StringViewModel(key, ttl),
                RedisType.Set => new UnsortedSetViewModel(key, ttl),
                RedisType.Hash => new HashSetViewModel(key, ttl),
                RedisType.List => new ListViewModel(key, ttl),
                RedisType.SortedSet => new SortedSetViewModel(key, ttl),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
