using System;
using System.Collections.ObjectModel;
using Quickr.Models.Keys;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class UnsortedSetViewModel: BaseKeyViewModel
    {
        private RedisValue _current;

        public ObservableCollection<RedisValue> Entries { get; set; }

        public RedisValue Current
        {
            get => _current;
            set
            {
                _current = value;
                Value = new ValueViewModel(_current);
                OnPropertyChanged();
            }
        }

        public UnsortedSetViewModel(RedisProxy proxy, KeyEntry key): base(proxy, key)
        {
            Entries = new ObservableCollection<RedisValue>(Proxy.GetUnsortedSet(Key));
        }

        protected void OnValueSaved(object sender, EventArgs e)
        {
            var index = Entries.IndexOf(Current);
            var entry = new RedisValue(Value.CurrentValue);
            Proxy.UnsortedSetRemove(Key, Value.OriginalValue); // TODO: incorrect paradigm, make add & remove
            Proxy.UnsortedSetAdd(Key, Value.CurrentValue);
            Entries.RemoveAt(index);
            Entries.Insert(0, entry);
            Current = entry;
        }
    }
}
