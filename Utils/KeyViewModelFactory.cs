using Quickr.Models.Keys;
using Quickr.ViewModels.Data;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Quickr.Utils
{
    class KeyViewModelFactory
    {
        public async Task<KeyViewModel> LoadKey(KeyEntry key)
        {
            var (type, ttl) = await key.GetProperties();
            var properties = new PropertiesViewModel(key, ttl);
            var value = LoadValueViewModel(key, type);
            return new KeyViewModel(properties, value);
        }

        public BaseValueViewModel CreateValueViewModel(KeyEntry key, KeyType type) =>
            type switch
            {
                KeyType.String => new StringViewModel(key),
                KeyType.Set => new UnsortedSetViewModel(key),
                KeyType.HashSet => new HashSetViewModel(key),
                KeyType.List => new ListViewModel(key),
                KeyType.SortedSet => new SortedSetViewModel(key),
                _ => throw new ArgumentOutOfRangeException()
            };

        static BaseValueViewModel LoadValueViewModel(KeyEntry key, RedisType type) =>
            type switch
            {
                RedisType.String => new StringViewModel(key) as BaseValueViewModel,
                RedisType.Set => new UnsortedSetViewModel(key),
                RedisType.Hash => new HashSetViewModel(key),
                RedisType.List => new ListViewModel(key),
                RedisType.SortedSet => new SortedSetViewModel(key),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
