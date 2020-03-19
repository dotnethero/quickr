using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Quickr.Models.Keys;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class SortedSetViewModel : INotifyPropertyChanged
    {
        private SortedSetEntry _current;
        private ValueViewModel _value;

        public KeyEntry Key { get; }
        public SortedSetEntry[] Entries { get; set; }
        public PropertiesViewModel Properties { get; }

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

        public SortedSetViewModel(RedisProxy proxy, KeyEntry key)
        {
            Key = key;
            Entries = proxy.GetSortedSet(key);
            Properties = new PropertiesViewModel(proxy, key);
        }

        private void SaveEntry(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
