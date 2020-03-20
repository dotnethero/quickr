using System;
using Quickr.Models.Keys;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class UnsortedSetViewModel: BaseKeyViewModel
    {
        private RedisValue _current;

        public RedisValue[] Entries { get; set; }

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
            Entries = Proxy.GetUnsortedSet(Key);
        }

        protected override void OnValueSaved(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
