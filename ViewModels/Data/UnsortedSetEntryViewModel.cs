using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    class UnsortedSetEntryViewModel : BaseEntryViewModel
    {
        public static UnsortedSetEntryViewModel FromValue(RedisValue value) => new UnsortedSetEntryViewModel(value);
        public static UnsortedSetEntryViewModel Empty() => new UnsortedSetEntryViewModel();

        protected UnsortedSetEntryViewModel()
        {
        }

        protected UnsortedSetEntryViewModel(RedisValue value) : base(value)
        {
        }

        public RedisValue ToOriginalEntry() => OriginalValue;
        public RedisValue ToEntry() => CurrentValue;
    }
}