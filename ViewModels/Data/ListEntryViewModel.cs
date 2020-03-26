using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class ListEntryViewModel : BaseEntryViewModel
    {
        public static ListEntryViewModel FromValue(RedisValue value) => new ListEntryViewModel(value);
        public static ListEntryViewModel Empty() => new ListEntryViewModel();

        protected ListEntryViewModel()
        {
        }

        protected ListEntryViewModel(RedisValue value) : base(value)
        {
        }
    }
}