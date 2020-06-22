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
    internal class HashSetViewModel: BaseCollectionViewModel<HashEntryViewModel>
    {
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; set; }

        public HashSetViewModel(KeyEntry key, TimeSpan? ttl, bool load = true): base(key, ttl)
        {
            if (load) SetupAsync();

            AddCommand = new ParameterCommand(Add);
            DeleteCommand = new ParameterCommand(Delete);
            SaveCommand = new Command(async() => await Save());
        }

        private async void SetupAsync()
        {
            var entries = await Key.GetDatabase().GetHashesAsync(Key);
            var models = entries.Select(HashEntryViewModel.FromEntry);
            Entries = new ObservableCollection<HashEntryViewModel>(models);
        }

        private void Add(object parameter)
        {
            var item = HashEntryViewModel.Empty();
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
                var entries = items.Cast<HashEntryViewModel>().ToList();
                var fields = entries
                    .Where(x => x.OriginalValue != null)
                    .Select(x => (RedisValue) x.Name)
                    .ToArray();

                if (fields.Length > 0)
                {
                    await Key.GetDatabase().HashDelete(Key, fields);
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
            if (items.GroupBy(x => x.Name).Any(g => g.Count() > 1)) return false;

            var fields = items.Select(x => x.ToEntry()).ToArray();
            await Key.GetDatabase().HashSet(Key, fields);
            return true;
        }

        protected override async Task SaveItem()
        {
            if (string.IsNullOrEmpty(Current.Name)) return;
            if (Entries.Any(x => x.Name == Current.Name && x != Current)) return;

            await Key.GetDatabase().HashSet(Key, Current.Name, Current.CurrentValue);
            Current.OriginalValue = Current.CurrentValue;
        }

        protected override async Task DiscardItemChanges()
        {
            Current.CurrentValue = Current.OriginalValue;
        }
    }
}
