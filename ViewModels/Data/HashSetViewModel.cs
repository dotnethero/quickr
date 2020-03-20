using System;
using System.Collections.ObjectModel;
using Quickr.Models.Keys;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class HashSetViewModel: BaseKeyViewModel
    {
        private HashEntry _current;

        public ObservableCollection<HashEntry> Entries { get; set; }

        public HashEntry Current
        {
            get => _current;
            set
            {
                _current = value;
                Value = new ValueViewModel(_current.Value);
                OnPropertyChanged();
            }
        }

        public HashSetViewModel(RedisProxy proxy, KeyEntry key): base(proxy, key)
        {
            Entries = new ObservableCollection<HashEntry>(Proxy.GetHashes(Key));
        }

        protected override void OnValueSaved(object sender, EventArgs e)
        {
            Proxy.HashSet(Key, Current.Name, Value.CurrentValue);
            var index = Entries.IndexOf(Current);
            var entry = new HashEntry(Current.Name, Value.CurrentValue);
            Entries[index] = entry;
            Current = entry;
        }
    }
}
