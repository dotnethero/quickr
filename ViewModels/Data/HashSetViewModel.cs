using System;
using Quickr.Models.Keys;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class HashSetViewModel: BaseKeyViewModel
    {
        private HashEntry _current;

        public HashEntry[] Entries { get; set; }

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
            Entries = proxy.GetHashes(key);
        }

        protected override void OnValueSaved(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
