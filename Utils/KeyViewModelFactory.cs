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
            var type = _proxy.GetType(key);
            return type switch
            {
                RedisType.String => Resolve<StringViewModel>(key),
                RedisType.Set => Resolve<UnsortedSetViewModel>(key),
                RedisType.Hash => Resolve<HashSetViewModel>(key),
                RedisType.List => Resolve<ListViewModel>(key),
                RedisType.SortedSet => Resolve<SortedSetViewModel>(key),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private T Resolve<T>(KeyEntry key)
        {
            return _scope.Resolve<T>(TypedParameter.From(key));
        }
    }
}
