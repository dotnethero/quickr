using Quickr.Models.Keys;
using Quickr.Services;

namespace Quickr.ViewModels
{
    internal class DatabaseViewModel : BaseViewModel
    {
        protected DatabaseEntry Entry { get; }
        protected RedisConnection Connection { get; }

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

        public DatabaseViewModel(RedisConnection connection, DatabaseEntry entry)
        {
            Entry = entry;
            Connection = connection;
            KeyCount = connection.GetSize(entry);
        }
    }
}
