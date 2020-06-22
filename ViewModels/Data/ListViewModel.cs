using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Quickr.Models.Keys;
using Quickr.Utils;

namespace Quickr.ViewModels.Data
{
    internal class ListViewModel: BaseCollectionViewModel<ListEntryViewModel>
    {
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }

        public ListViewModel(KeyEntry key, TimeSpan? ttl): base(key, ttl)
        {
            SetupAsync();
            AddCommand = new ParameterCommand(Add);
            DeleteCommand = new ParameterCommand(Delete);
            SaveCommand = new Command(async() => await Save());
        }
        
        private async void SetupAsync()
        {
            var entries = await Key.GetDatabase().GetListAsync(Key);
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

        private async void Delete(object parameter)
        {
            if (parameter is IList items)
            {
                var entries = items.Cast<ListEntryViewModel>().ToList();
                var indexes = entries
                    .Where(x => x.OriginalValue != null)
                    .Select(x => x.Index)
                    .ToList();

                if (indexes.Count > 0)
                {
                    await Key.GetDatabase().ListDelete(Key, indexes);
                }

                foreach (var entry in entries)
                {
                    Entries.Remove(entry);
                }

                foreach (var entry in Entries)
                {
                    entry.Index -= indexes.Count(i => i < entry.Index);
                }
            }
        }
        
        public override async Task<bool> Save()
        {
            var items = Entries.Where(x => x.CurrentValue != null).ToArray();
            var existingItems = items.Where(x => !x.IsNew).ToDictionary(x => x.Index, x => x.ToValue());
            var newItems = items.Where(x => x.IsNew).Select(x => x.ToValue()).ToArray();

            var count = await Key.GetDatabase().ListSave(Key, existingItems, newItems);
            var countBefore = count - items.Length;

            for (var index = 0; index < items.Length; index++)
            {
                var item = items[index];
                item.Index = countBefore + index;
                item.OriginalValue = item.CurrentValue;
            }

            return true;
        }

        protected override async Task SaveItem()
        {
            if (Current.IsNew)
            {
                var count = await Key.GetDatabase().ListRightPush(Key, Current.CurrentValue);
                Current.Index = count - 1;
                Current.OriginalValue = Current.CurrentValue;

                var prev = Entries.IndexOf(Current);
                Entries.Move(prev, (int) Current.Index);
            }
            else
            {
                await Key.GetDatabase().ListSet(Key, Current.Index, Current.CurrentValue);
                Current.OriginalValue = Current.CurrentValue;
            }
        }

        protected override async Task DiscardItemChanges()
        {
            Current.CurrentValue = Current.OriginalValue;
        }
    }
}
