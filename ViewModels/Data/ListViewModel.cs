using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Quickr.Models.Keys;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class ListViewModel: BaseKeyViewModel
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

        public ListViewModel(RedisProxy proxy, KeyEntry key): base(proxy, key)
        {
            Entries = proxy.GetList(key);
        }

        protected override void OnValueSaved(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
