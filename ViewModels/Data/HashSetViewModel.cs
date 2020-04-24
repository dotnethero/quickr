using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        public HashSetViewModel(KeyEntry key, TimeSpan? ttl): base(key, ttl)
        {
            SetupAsync();
            AddCommand = new ParameterCommand(Add);
            DeleteCommand = new ParameterCommand(Delete);
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

        private void Delete(object parameter)
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
                    Key.GetDatabase().HashDelete(Key, fields);
                }

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

            Key.GetDatabase().HashSet(Key, Current.Name, Current.CurrentValue);
            Current.OriginalValue = Current.CurrentValue;
        }

        protected override void OnValueDiscarded(object sender, EventArgs e)
        {
            Current.CurrentValue = Current.OriginalValue;
        }
    }
}
