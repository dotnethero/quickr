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
        private HashEntryViewModel _current;

        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; }

        public ObservableCollection<HashEntryViewModel> Entries { get; set; }

        public HashEntryViewModel Current
        {
            get => _current;
            set
            {
                _current = value;
                Value = _current != null ? new ValueViewModel(_current.Value) : null;
                OnPropertyChanged();
            }
        }

        public HashSetViewModel(RedisProxy proxy, KeyEntry key): base(proxy, key)
        {
            Entries = new ObservableCollection<HashEntryViewModel>(Proxy
                .GetHashes(Key)
                .Select(HashEntryViewModel.FromHashEntry));

            AddCommand = new Command(Add);
            DeleteCommand = new ParameterCommand(Delete);
        }

        private void Add()
        {
            var item = new HashEntryViewModel();
            Entries.Add(item);
        }

        private void Delete(object parameter)
        {
            if (parameter is IList items)
            {
                var entries = items.Cast<HashEntryViewModel>().ToList();
                var fields = entries.Select(x => (RedisValue) x.Name).ToArray();
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
            Current.Value = Value.CurrentValue;
        }
    }
}
