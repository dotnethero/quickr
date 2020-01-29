using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Quickr.Annotations;
using Quickr.Models;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.ViewModels
{
    internal class KeyViewModel: INotifyPropertyChanged
    {
        private readonly RedisProxy _proxy;
        private object _table;
        private object _current;

        private string _name;
        private ValueViewModel _value;
        private TimeSpan? _expiration;

        public string OriginalName { get; }
        public TimeSpan? OriginalExpiration { get; }
        public RedisType Type { get; }
        public KeyEntry Key { get; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PropertiesChanged));
            }
        }
        
        public TimeSpan? Expiration
        {
            get => _expiration;
            set
            {
                _expiration = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PropertiesChanged));
            }
        }

        public bool PropertiesChanged => 
            OriginalName != Name ||
            OriginalExpiration != Expiration;

        public object Table
        {
            get => _table;
            set
            {
                switch (value)
                {
                    case HashEntry[] hashSet:
                        _table = hashSet;
                        Current = hashSet.FirstOrDefault(x => x.Name == "value");
                        break;

                    case SortedSetEntry[] sorted:
                        _table = sorted;
                        Current = sorted.FirstOrDefault();
                        break;

                    case RedisValue[] list:
                        _table = list;
                        Current = list.FirstOrDefault();
                        break;

                    default:
                        _table = null;
                        Current = null;
                        break;
                }
                OnPropertyChanged();
            }
        }

        public object Current
        {
            get => _current;
            set
            {
                switch (value)
                {
                    case HashEntry hash:
                        _current = hash;
                        Value = new ValueViewModel(hash.Value);
                        break;

                    case SortedSetEntry zset:
                        _current = zset;
                        Value = new ValueViewModel(zset.Element);
                        break;

                    case RedisValue rval:
                        _current = rval;
                        Value = new ValueViewModel(rval);
                        break;

                    default:
                        _current = null;
                        Value = null;
                        break;
                }
                OnPropertyChanged();
            }
        }

        public ValueViewModel Value
        {
            get => _value;
            set
            {
                if (_value != null) 
                    Value.OnValueSaved -= SaveValue;

                _value = value;
                OnPropertyChanged();

                if (_value != null) 
                    Value.OnValueSaved += SaveValue;
            }
        }

        public KeyViewModel(KeyEntry key, RedisProxy proxy)
        {
            // use DI later
            _proxy = proxy;

            // properties
            var type = _proxy.GetType(key);
            var ttl = _proxy.GetTimeToLive(key);
            switch (type)
            {
                case RedisType.Hash:
                    Table = _proxy.GetHashes(key);
                    break;

                case RedisType.List:
                    Table = _proxy.GetList(key);
                    break;

                case RedisType.Set:
                    Table = _proxy.GetUnsortedSet(key);
                    break;

                case RedisType.SortedSet:
                    Table = _proxy.GetSortedSet(key);
                    break;

                case RedisType.String:
                    Table = null;
                    Value = new ValueViewModel(_proxy.GetString(key).Value);
                    break;
            }

            Key = key;
            Type = type;
            OriginalName = key.FullName;
            OriginalExpiration = ttl;

            Name = OriginalName;
            Expiration = OriginalExpiration;
        }

        private void SaveValue(object sender, EventArgs eventArgs)
        {
            switch (Type)
            {
                case RedisType.String:
                    _proxy.SetString(Key, Value.CurrentValue);
                    Value = new ValueViewModel(_proxy.GetString(Key).Value);
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
