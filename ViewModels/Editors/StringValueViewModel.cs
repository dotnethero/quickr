using StackExchange.Redis;

namespace Quickr.ViewModels.Editors
{
    internal class StringValueViewModel : ValueViewModel<RedisValue>
    {
        public StringValueViewModel(RedisValue originalValue) : base(originalValue)
        {
        }

        public StringValueViewModel(RedisValue originalValue, RedisValue currentValue) : base(originalValue, currentValue)
        {
        }
    }
}