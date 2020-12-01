using Quickr.Models.Keys;

namespace Quickr.ViewModels
{
    class DatabaseViewModel : BaseViewModel
    {
        protected DatabaseEntry Entry { get; }

        public long KeyCount { get; private set; }

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

        public DatabaseViewModel(DatabaseEntry entry)
        {
            Entry = entry;
            Initialize();
        }

        async void Initialize()
        {
            KeyCount = await Entry.GetSize();
        }
    }
}
