using System.Threading.Tasks;
using Quickr.Services;

namespace Quickr.Models.Keys
{
    internal class DatabaseEntry: FolderEntry
    {
        public DatabaseEntry(RedisConnection connection, int dbIndex): base(connection, dbIndex, $"db{dbIndex}", $"db{dbIndex}", null)
        {
        }
        
        public async Task<long> GetSize()
        {
            return await GetKeyspace().GetSize();
        }

        public async Task Flush()
        {
            await GetKeyspace().Flush();
            RemoveChildren();
        }

        public async Task MarkChildrenAsExpired()
        {
            await MarkKeysAsExpired();
            RemoveChildren();
        }
    }
}