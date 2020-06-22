using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Quickr.Models.Keys;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class UnsortedSetViewModel: BaseCollectionViewModel<UnsortedSetEntryViewModel>
    {
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }

        public UnsortedSetViewModel(KeyEntry key, TimeSpan? ttl, bool load = true): base(key, ttl)
        {
            if (load) SetupAsync();

            AddCommand = new ParameterCommand(Add);
            DeleteCommand = new ParameterCommand(Delete);
            SaveCommand = new Command(async() => await Save());
        }
        
        private async void SetupAsync()
        {
            var entries = await Key.GetDatabase().GetUnsortedSetAsync(Key);
            var models = entries.Select(UnsortedSetEntryViewModel.FromValue);
            Entries = new ObservableCollection<UnsortedSetEntryViewModel>(models);
        }

        private void Add(object parameter)
        {
            var item = UnsortedSetEntryViewModel.Empty();
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
                var entries = items.Cast<UnsortedSetEntryViewModel>().ToList();
                var values = entries
                    .Where(x => x.OriginalValue != null)
                    .Select(x => (RedisValue) x.OriginalValue)
                    .ToArray();

                if (values.Length > 0)
                {
                    await Key.GetDatabase().UnsortedSetRemove(Key, values);
                }

                foreach (var entry in entries)
                {
                    Entries.Remove(entry);
                }
            }
        }
        
        public override async Task<bool> Save()
        {
            var items = Entries.Where(x => x.IsValueChanged && x.CurrentValue != null).ToArray();
            if (items.GroupBy(x => x.CurrentValue).Any(g => g.Count() > 1)) return false;

            var removed = items.Where(x => !x.IsNew).Select(x => x.ToOriginalEntry()).ToArray();
            var added = items.Select(x => x.ToEntry()).ToArray();
            await Key.GetDatabase().UnsortedSetSave(Key, removed, added);

            foreach (var item in items)
            {
                item.OriginalValue = item.CurrentValue;
            }

            return true;
        }

        protected override async Task SaveItem()
        {
            if (Entries.Any(x => x.OriginalValue == Current.CurrentValue && x != Current))
            {
                return;
            }
            if (Current.IsNew)
            {
                await Key.GetDatabase().UnsortedSetAdd(Key, Current.CurrentValue);
                Current.OriginalValue = Current.CurrentValue;
            }
            else
            {
                await Key.GetDatabase().UnsortedSetUpdate(Key, Current.OriginalValue, Current.CurrentValue);
                Current.OriginalValue = Current.CurrentValue;
            }
        }

        protected override async Task DiscardItemChanges()
        {
            Current.CurrentValue = Current.OriginalValue;
        }
    }
}
