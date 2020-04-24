using Quickr.Models.Keys;
using Quickr.ViewModels.Data;
using StackExchange.Redis;
using System;

namespace Quickr.Utils
{
    internal class KeyViewModelFactory
    {
        public BaseKeyViewModel Create(KeyEntry key)
        {
            var (type, ttl) = key
                .GetProperties()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

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
