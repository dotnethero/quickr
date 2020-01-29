using StackExchange.Redis;

namespace Quickr.Models
{
    internal class KeyData
    {
        public KeyEntry Entry { get; set; }
        public RedisType Type { get; set; }
        public object Data { get; set; }

        public override string ToString()
        {
            return $"Key: {Entry.FullName}";
        }
    }
}