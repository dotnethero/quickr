using Quickr.Models.Keys;
using Quickr.Services;

namespace Quickr.ViewModels
{
    internal class DatabaseViewModel : BaseViewModel
    {
        protected DatabaseEntry Entry { get; }
        protected RedisProxy Proxy { get; }

        public long KeyCount { get; }

        public string Filter
        {
            get => Entry.Filter;
            set
            {
                if (value == Entry.Filter) return;
                Entry.Filter = value;
                OnPropertyChanged();
            }
        }

        public DatabaseViewModel(RedisProxy proxy, DatabaseEntry entry)
        {
            Proxy = proxy;
            Entry = entry;
            KeyCount = proxy.GetSize(entry);
        }
    }
}
