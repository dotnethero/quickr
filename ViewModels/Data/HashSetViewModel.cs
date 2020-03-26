﻿using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Quickr.Annotations;
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

        [NotifyPropertyChangedInvocator]
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
            if (e.PropertyName == nameof(HashEntryViewModel.CurrentValue))
            {
                Value.CurrentValue = Current.CurrentValue;
            }
        }

        private void OnValueSaved(object sender, EventArgs e)
        {
            Proxy.HashSet(Key, Current.Name, Value.CurrentValue);
            Current.OriginalValue = Value.OriginalValue;
        }
        
        private void OnValueDiscarded(object sender, EventArgs e)
        {
            Current.CurrentValue = Value.CurrentValue;
        }
    }
}
