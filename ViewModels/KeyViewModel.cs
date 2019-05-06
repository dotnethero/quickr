using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Quickr.Annotations;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.ViewModels
{
    internal class KeyViewModel: INotifyPropertyChanged
    {
        private object _table;
        private object _current;
        private string _value;
        private string _name;
        private string _expiration;

        public string OriginalName { get; set; }
        public string OriginalExpiration { get; set; }

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
        
        public string Expiration
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
                        Value = hash.Value.PrettifyJson();
                        break;

                    case SortedSetEntry zset:
                        _current = zset;
                        Value = zset.Element.PrettifyJson();
                        break;

                    case RedisValue rval:
                        _current = rval;
                        Value = rval.PrettifyJson();
                        break;

                    default:
                        _current = null;
                        Value = null;
                        break;
                }
                OnPropertyChanged();
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
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
