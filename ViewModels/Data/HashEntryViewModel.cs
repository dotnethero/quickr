using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    class HashEntryViewModel : BaseEntryViewModel
    {
        public string Name { get; set; }

        public static HashEntryViewModel FromEntry(HashEntry entry) => new HashEntryViewModel(entry);
        public static HashEntryViewModel Empty() => new HashEntryViewModel();

        protected HashEntryViewModel()
        {
        }

        protected HashEntryViewModel(HashEntry entry) : base(entry.Value)
        {
            Name = entry.Name;
        }

        public HashEntry ToEntry() => new HashEntry(Name, CurrentValue);

    }
}