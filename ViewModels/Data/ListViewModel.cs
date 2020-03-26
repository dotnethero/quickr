using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
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
