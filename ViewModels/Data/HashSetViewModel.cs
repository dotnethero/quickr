using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class HashSetViewModel: BaseKeyViewModel
    {
        private HashEntry _current;

        public ICommand DeleteCommand { get; }

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
            DeleteCommand = new ParameterCommand(Delete);
        }

        private void Delete(object parameter)
        {
            if (parameter is IList items)
            {
                var entries = items.Cast<HashEntry>().ToList();
                var fields = entries.Select(x => x.Name).ToArray();
                Proxy.HashDelete(Key, fields);
                foreach (var entry in entries)
                {
                    Entries.Remove(entry);
                }
            }
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
