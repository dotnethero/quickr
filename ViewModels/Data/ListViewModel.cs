using System;
using System.Collections.ObjectModel;
using Quickr.Models.Keys;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class ListViewModel: BaseKeyViewModel
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

        public ListViewModel(RedisProxy proxy, KeyEntry key): base(proxy, key)
        {
            Entries = new ObservableCollection<RedisValue>(Proxy.GetList(Key));
        }

        protected override void OnValueSaved(object sender, EventArgs e)
        {
            Proxy.HashSet(Key, Current.Name, Value.CurrentValue);
            var index = Entries.IndexOf(Current);
            var entry = new RedisValue(Value.CurrentValue);
            Entries[index] = entry;
            Current = entry;
        }
    }
}
