using Autofac;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.ViewModels.Data;
using StackExchange.Redis;
using System;

namespace Quickr.Utils
{
    internal class KeyViewModelFactory
    {
        private readonly RedisProxy _proxy;
        private readonly ILifetimeScope _scope;

        public KeyViewModelFactory(RedisProxy proxy, ILifetimeScope scope)
        {
            _proxy = proxy;
            _scope = scope;
        }

        public BaseKeyViewModel CreateViewModel(KeyEntry key)
        {
            var (type, ttl) = _proxy.GetTypeTimeToLiveAsync(key).ConfigureAwait(false).GetAwaiter().GetResult();
            return type switch
            {
                RedisType.String => Resolve<StringViewModel>(key, ttl),
                RedisType.Set => Resolve<UnsortedSetViewModel>(key, ttl),
                RedisType.Hash => Resolve<HashSetViewModel>(key, ttl),
                RedisType.List => Resolve<ListViewModel>(key, ttl),
                RedisType.SortedSet => Resolve<SortedSetViewModel>(key, ttl),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private T Resolve<T>(KeyEntry key, TimeSpan? ttl)
        {
            return _scope.Resolve<T>(TypedParameter.From(key), TypedParameter.From(ttl));
        }
    }
}
