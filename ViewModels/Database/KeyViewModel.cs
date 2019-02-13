using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.ViewModels.Database
{
    internal class KeyViewModel : EntryViewModel
    {
        public string FullName { get; }

        public KeyViewModel(string name, string fullname): base(name)
        {
            FullName = fullname;
        }

        public HashEntry[] GetHashes()
        {
            var connection = RedisMultiplexer.Connect();
            return connection
                .GetDatabase(0)
                .HashGetAll(FullName);
        }

        public override string ToString()
        {
            return $"Key: {FullName}";
        }
    }
}