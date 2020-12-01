using Quickr.Models.Keys;
using Quickr.ViewModels.Data;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Quickr.Utils
{
    internal class KeyViewModelFactory
    {
        public BaseKeyViewModel Create(KeyEntry key, KeyType type, TimeSpan? ttl)
        {
            return type switch
            {
                KeyType.String => new StringViewModel(key, ttl, false),
                KeyType.Set => new UnsortedSetViewModel(key, ttl, false),
                KeyType.HashSet => new HashSetViewModel(key, ttl, false),
                KeyType.List => new ListViewModel(key, ttl, false),
                KeyType.SortedSet => new SortedSetViewModel(key, ttl, false),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public async Task<BaseKeyViewModel> Load(KeyEntry key)
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
