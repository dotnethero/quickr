using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class SortedSetViewModel : BaseCollectionViewModel<SortedSetEntryViewModel>
    {
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public SortedSetViewModel(RedisConnection connection, KeyEntry key, TimeSpan? ttl): base(connection, key, ttl)
        {
            SetupAsync();
            
            AddCommand = new ParameterCommand(Add);
            DeleteCommand = new ParameterCommand(Delete);
        }

        private async void SetupAsync()
        {
            var entries = await Connection.GetSortedSetAsync(Key);
            var models = entries.Select(SortedSetEntryViewModel.FromEntry);
            Entries = new ObservableCollection<SortedSetEntryViewModel>(models);
        }

        private void Add(object parameter)
        {
            var item = SortedSetEntryViewModel.Empty();
            Entries.Add(item);
            Current = item;
            if (parameter is DataGrid grid)
            {
                grid.UpdateLayout();
                grid.Focus();
                grid.CurrentCell = grid.SelectedCells[0];
                grid.BeginEdit();
            }
        }

        private void Delete(object parameter)
        {
            if (parameter is IList items)
            {
                var entries = items.Cast<SortedSetEntryViewModel>().ToList();
                var fields = entries
                    .Where(x => x.OriginalValue != null)
                    .Select(x => (RedisValue) x.OriginalValue)
                    .ToArray();

                if (fields.Length > 0)
                {
                    Connection.SortedSetRemove(Key, fields);
                }

                foreach (var entry in entries)
                {
                    Entries.Remove(entry);
                }
            }
        }

        protected override void OnValueSaved(object sender, EventArgs e)
        {
            if (Current.IsNew)
            {
                if (Entries.Any(x => x.OriginalValue == Current.CurrentValue && x != Current)) return;
                Connection.SortedSetAdd(Key, Value.CurrentValue, Current.Score);
                Current.OriginalValue = Current.CurrentValue;
            }
            else
            {
                // TODO: set score ?
            }
        }

        protected override void OnValueDiscarded(object sender, EventArgs e)
        {
            Current.CurrentValue = Current.OriginalValue;
        }
    }
}
