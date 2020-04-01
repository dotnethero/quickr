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
            var connection = key.Connection;
            var (type, ttl) = connection
                .GetTypeTimeToLiveAsync(key)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            return type switch
            {
                RedisType.String => new StringViewModel(connection, key, ttl),
                RedisType.Set => new UnsortedSetViewModel(connection, key, ttl),
                RedisType.Hash => new HashSetViewModel(connection, key, ttl),
                RedisType.List => new ListViewModel(connection, key, ttl),
                RedisType.SortedSet => new SortedSetViewModel(connection, key, ttl),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
