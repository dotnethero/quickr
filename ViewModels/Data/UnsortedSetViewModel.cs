using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class UnsortedSetViewModel: BaseViewModel
    {
        private RedisValue _current;
        private ValueViewModel _value;

        public KeyEntry Key { get; }
        public RedisValue[] Entries { get; set; }
        public PropertiesViewModel Properties { get; }

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

        public ValueViewModel Value
        {
            get => _value;
            set
            {
                if (_value != null)
                {
                    _value.OnValueSaved -= SaveEntry;
                    _value = value;
                    _value.OnValueSaved += SaveEntry;
                }
                else
                {
                    _value = value;
                }
                OnPropertyChanged();
            }
        }

        public UnsortedSetViewModel(RedisProxy proxy, KeyEntry key)
        {
            Key = key;
            Entries = proxy.GetUnsortedSet(key);
            Properties = new PropertiesViewModel(proxy, key);
        }

        private void SaveEntry(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
