using Quickr.Services;

namespace Quickr.Models.Keys
{
    internal class DatabaseEntry: FolderEntry
    {
        public DatabaseEntry(RedisProxy proxy, int dbIndex): base(proxy, dbIndex, $"db{dbIndex}", $"db{dbIndex}", null)
        {
        }
    }
}