using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class HashEntryViewModel : BaseEntryViewModel
    {
        public string Name { get; set; }

        public static HashEntryViewModel FromHashEntry(HashEntry entry) => new HashEntryViewModel(entry);
        public static HashEntryViewModel Empty() => new HashEntryViewModel();

        protected HashEntryViewModel()
        {
        }

        protected HashEntryViewModel(HashEntry entry) : base(entry.Value)
        {
            Name = entry.Name;
        }
    }
}