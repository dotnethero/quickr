using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.Utils;
using Quickr.ViewModels.Editors;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class UnsortedSetViewModel: BaseKeyViewModel
    {
        private UnsortedSetEntryViewModel _current;
        
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public ObservableCollection<UnsortedSetEntryViewModel> Entries { get; set; }

        public UnsortedSetEntryViewModel Current
        {
            get => _current;
            set
            {
                if (_current == value) return;
                _current = value;
                OnPropertyChanged();
            }
        }

        public UnsortedSetViewModel(RedisProxy proxy, KeyEntry key): base(proxy, key)
        {
            Entries = new ObservableCollection<UnsortedSetEntryViewModel>(Proxy
                .GetUnsortedSet(Key)
                .Select(UnsortedSetEntryViewModel.FromValue));
            
            AddCommand = new Command(Add);
            DeleteCommand = new ParameterCommand(Delete);
        }

        private void Add()
        {
            var item = UnsortedSetEntryViewModel.Empty();
            Entries.Add(item);
        }

        private void Delete(object parameter)
        {
            if (parameter is IList items)
            {
                var entries = items.Cast<UnsortedSetEntryViewModel>().ToList();
                var values = entries
                    .Where(x => x.OriginalValue != null)
                    .Select(x => (RedisValue) x.OriginalValue)
                    .ToArray();

                Proxy.UnsortedSetRemove(Key, values);
                foreach (var entry in entries)
                {
                    Entries.Remove(entry);
                }
            }
        }
        
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Current))
            {
                if (Current != null)
                {
                    Value = new ValueViewModel(Current.OriginalValue, Current.CurrentValue);
                    Value.ValueSaved += OnValueSaved;
                    Value.ValueDiscarded += OnValueDiscarded;
                    Value.PropertyChanged += OnValuePropertyChanged;
                    Current.PropertyChanged += OnCurrentPropertyChanged;
                }
                else
                {
                    Value = null;
                }
            }
            base.OnPropertyChanged(propertyName);
        }

        private void OnValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ValueViewModel.CurrentValue))
            {
                Current.CurrentValue = Value.CurrentValue;
            }
        }

        private void OnCurrentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HashEntryViewModel.OriginalValue))
            {
                Value.OriginalValue = Current.OriginalValue;
            }
            if (e.PropertyName == nameof(HashEntryViewModel.CurrentValue))
            {
                Value.CurrentValue = Current.CurrentValue;
            }
        }
        
        private void OnValueSaved(object sender, EventArgs e)
        {
            if (Current.IsNew)
            {
                if (Entries.Any(x => x.OriginalValue == Current.CurrentValue && x != Current)) return;
                Proxy.UnsortedSetAdd(Key, Value.CurrentValue);
                Current.OriginalValue = Current.CurrentValue;
            }
        }

        private void OnValueDiscarded(object sender, EventArgs e)
        {
            Current.CurrentValue = Current.OriginalValue;
        }
    }
}
