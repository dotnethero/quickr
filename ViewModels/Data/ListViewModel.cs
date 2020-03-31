using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.Utils;

namespace Quickr.ViewModels.Data
{
    internal class ListViewModel: BaseCollectionViewModel<ListEntryViewModel>
    {
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public ListViewModel(RedisProxy proxy, KeyEntry key, TimeSpan? ttl): base(proxy, key, ttl)
        {
            SetupAsync();
            AddCommand = new ParameterCommand(Add);
            DeleteCommand = new ParameterCommand(Delete);
        }
        
        private async void SetupAsync()
        {
            var entries = await Proxy.GetListAsync(Key);
            var models = entries.Select(ListEntryViewModel.FromValue);
            Entries = new ObservableCollection<ListEntryViewModel>(models);
        }

        private void Add(object parameter)
        {
            var item = ListEntryViewModel.Empty();
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
                var entries = items.Cast<ListEntryViewModel>().ToList();
                var indexes = entries
                    .Where(x => x.OriginalValue != null)
                    .Select(Entries.IndexOf)
                    .ToArray();

                if (indexes.Length > 0)
                {
                    Proxy.ListDelete(Key, indexes);
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

        protected override void OnValueDiscarded(object sender, EventArgs e)
        {
            Current.CurrentValue = Current.OriginalValue;
        }
    }
}
