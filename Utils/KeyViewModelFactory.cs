using Autofac;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.ViewModels.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

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

        public object CreateViewModel(KeyEntry key)
        {
            var type = _proxy.GetType(key);
            switch (type)
            {
                case RedisType.String:
                    return Resolve<StringViewModel>(key);

                case RedisType.Set:
                    return Resolve<UnsortedSetViewModel>(key);

                case RedisType.Hash:
                    return Resolve<HashSetViewModel>(key);

                case RedisType.List:
                    return Resolve<ListViewModel>(key);

                case RedisType.SortedSet:
                    return Resolve<SortedSetViewModel>(key);

                default:
                    return null;
            }
        }

        private T Resolve<T>(KeyEntry key)
        {
            return _scope.Resolve<T>(TypedParameter.From(key));
        }
    }
}
