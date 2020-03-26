using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class SortedSetEntryViewModel : BaseEntryViewModel
    {
        public double Score { get; set; }

        public static SortedSetEntryViewModel FromEntry(SortedSetEntry entry) => new SortedSetEntryViewModel(entry);
        public static SortedSetEntryViewModel Empty() => new SortedSetEntryViewModel();

        protected SortedSetEntryViewModel()
        {
        }

        protected SortedSetEntryViewModel(SortedSetEntry entry) : base(entry.Element)
        {
            Score = entry.Score;
        }
    }
}