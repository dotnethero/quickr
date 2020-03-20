using System;
using Quickr.Models.Keys;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class SortedSetViewModel : BaseKeyViewModel
    {
        private SortedSetEntry _current;

        public SortedSetEntry[] Entries { get; set; }

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
            Entries = Proxy.GetSortedSet(Key);
        }

        protected override void OnValueSaved(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
