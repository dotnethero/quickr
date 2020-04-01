using Quickr.Services;

namespace Quickr.Models.Keys
{
    internal class DatabaseEntry: FolderEntry
    {
        public DatabaseEntry(RedisConnection connection, int dbIndex): base(connection, dbIndex, $"db{dbIndex}", $"db{dbIndex}", null)
        {
        }
    }
}