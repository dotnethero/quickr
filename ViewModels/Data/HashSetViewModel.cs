﻿using System;
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
    internal class HashSetViewModel: BaseCollectionViewModel<HashEntryViewModel>
    {
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public HashSetViewModel(RedisProxy proxy, KeyEntry key): base(proxy, key)
        {
            Entries = new ObservableCollection<HashEntryViewModel>(Proxy
                .GetHashes(Key)
                .Select(HashEntryViewModel.FromEntry));

            AddCommand = new Command(Add);
            DeleteCommand = new ParameterCommand(Delete);
        }

        private void Add()
        {
            var item = HashEntryViewModel.Empty();
            Entries.Add(item);
        }

        private void Delete(object parameter)
        {
            if (parameter is IList items)
            {
                var entries = items.Cast<HashEntryViewModel>().ToList();
                var fields = entries
                    .Where(x => x.OriginalValue != null)
                    .Select(x => (RedisValue) x.Name)
                    .ToArray();
                Proxy.HashDelete(Key, fields);
                foreach (var entry in entries)
                {
                    Entries.Remove(entry);
                }
            }
        }

        protected override void OnValueSaved(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Current.Name)) return;
            if (Entries.Any(x => x.Name == Current.Name && x != Current)) return;

            Proxy.HashSet(Key, Current.Name, Current.CurrentValue);
            Current.OriginalValue = Current.CurrentValue;
        }

        protected override void OnValueDiscarded(object sender, EventArgs e)
        {
            Current.CurrentValue = Current.OriginalValue;
        }
    }
}
