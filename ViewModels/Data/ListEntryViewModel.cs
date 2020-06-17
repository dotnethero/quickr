using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class ListEntryViewModel : BaseEntryViewModel
    {
        public long Index { get; set; }

        public static ListEntryViewModel FromValue(RedisValue value, int index) => new ListEntryViewModel(value, index);
        public static ListEntryViewModel Empty() => new ListEntryViewModel();

        protected ListEntryViewModel()
        {
        }

        protected ListEntryViewModel(RedisValue value, int index) : base(value)
        {
            Index = index;
        }

        public RedisValue ToValue() => CurrentValue;

    }
}