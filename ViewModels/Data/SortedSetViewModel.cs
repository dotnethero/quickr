using System;
using System.Collections.ObjectModel;
using Quickr.Models.Keys;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class SortedSetViewModel : BaseKeyViewModel
    {
        private SortedSetEntry _current;

        public ObservableCollection<SortedSetEntry> Entries { get; set; }

        public SortedSetEntry Current
        {
            get => _current;
            set
            {
                _current = value;
                Value = new ValueViewModel(_current.Element);
                OnPropertyChanged();
            }
        }
        
        public SortedSetViewModel(RedisProxy proxy, KeyEntry key): base(proxy, key)
        {
            Entries = new ObservableCollection<SortedSetEntry>(Proxy.GetSortedSet(Key));
        }

        protected override void OnValueSaved(object sender, EventArgs e)
        {
            var index = Entries.IndexOf(Current);
            var entry = new SortedSetEntry(Value.CurrentValue, Current.Score);
            Proxy.SortedSetRemove(Key, Value.OriginalValue); // TODO: incorrect paradigm, make add & remove
            Proxy.SortedSetAdd(Key, Value.CurrentValue, Current.Score);
            Entries[index] = entry;
            Current = entry;
        }
    }
}
