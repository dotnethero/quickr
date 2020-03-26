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

namespace Quickr.ViewModels.Data
{
    internal class ListViewModel: BaseKeyViewModel
    {
        private ListEntryViewModel _current;

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public ObservableCollection<ListEntryViewModel> Entries { get; set; }

        public ListEntryViewModel Current
        {
            get => _current;
            set
            {
                if (_current == value) return;
                _current = value;
                OnPropertyChanged();
            }
        }

        public ListViewModel(RedisProxy proxy, KeyEntry key): base(proxy, key)
        {
            Entries = new ObservableCollection<ListEntryViewModel>(Proxy
                .GetList(Key)
                .Select(ListEntryViewModel.FromValue));
            
            AddCommand = new Command(Add);
            DeleteCommand = new ParameterCommand(Delete);
        }
        
        private void Add()
        {
            var item = ListEntryViewModel.Empty();
            Entries.Add(item);
        }

        private void Delete(object parameter)
        {
            if (parameter is IList items)
            {
                var entries = items.Cast<ListEntryViewModel>().ToList();
                var indexes = entries
                    .Where(x => x.OriginalValue != null)
                    .Select(Entries.IndexOf)
                    .ToArray();

                Proxy.ListDelete(Key, indexes);
                foreach (var entry in entries)
                {
                    Entries.Remove(entry);
                }
            }
        }

        private void OnValueSaved(object sender, EventArgs e)
        {
            if (Current.IsNew)
            {
                Proxy.ListRightPush(Key, Value.CurrentValue);
                Current.OriginalValue = Current.CurrentValue;
            }
            else
            {
                var index = Entries.IndexOf(Current);
                Proxy.ListSet(Key, index, Value.CurrentValue);
                Current.OriginalValue = Current.CurrentValue;
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

        private void OnValueDiscarded(object sender, EventArgs e)
        {
            Current.CurrentValue = Current.OriginalValue;
        }
    }
}
