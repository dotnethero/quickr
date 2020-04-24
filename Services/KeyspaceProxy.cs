using System;
using System.Linq;
using System.Threading.Tasks;
using Quickr.Models.Keys;
using StackExchange.Redis;

namespace Quickr.Services
{
    internal class KeyspaceProxy
    {
        private readonly RedisConnection _connection;

        public KeyspaceProxy(RedisConnection connection)
        {
            _connection = connection;
        }

        public RedisKey[] GetKeys(int dbIndex, RedisValue pattern = default)
        {
            return _connection.GetMasterServers()
                .SelectMany(server => server.Keys(dbIndex, pattern, 2500))
                .ToArray();
        }

        public async Task<(RedisType, TimeSpan?)> GetKeyProperties(KeyEntry key) // TODO: move to key??
        {
            var database = _connection.GetDatabaseInternal(key.DbIndex);
            var batch = database.CreateBatch();
            var type = batch.KeyTypeAsync(key.FullName).ConfigureAwait(false);
            var ttl = batch.KeyTimeToLiveAsync(key.FullName).ConfigureAwait(false);
            batch.Execute();
            return (await type, await ttl);
        }
        
        public bool RenameKey(KeyEntry key, string name)
        {
            var database = _connection.GetDatabaseInternal(key.DbIndex);
            return database.KeyRename(key.FullName, name);
        }

        public async Task<string> CloneKey(KeyEntry key)
        {
            var database = _connection.GetDatabaseInternal(key.DbIndex);
            var baseName = key.FullName + "_copy";
            var name = baseName;
            var index = 1;
            while (database.KeyExists(name))
            {
                name = baseName + (index++);
            }
            var batch = database.CreateBatch();
            var data = batch.KeyDumpAsync(key.FullName).ConfigureAwait(false);
            var ttl = batch.KeyTimeToLiveAsync(key.FullName).ConfigureAwait(false);
            batch.Execute();

            await database.KeyRestoreAsync(name, await data, await ttl);
            return name;
        }
        
        public bool Delete(KeyEntry key)
        {
            var database = _connection.GetDatabaseInternal(key.DbIndex);
            return database.KeyDelete(key.FullName);
        }

        public long Delete(FolderEntry folder) // TODO: rework for cluster
        {
            var database = _connection.GetDatabaseInternal(folder.DbIndex);
            var keys = GetKeys(folder.DbIndex, folder.SearchPattern); // Invalid
            return database.KeyDelete(keys);
        }

        public bool SetTimeToLive(KeyEntry key, TimeSpan? timeSpan) // TODO: move to key??
        {
            var database = _connection.GetDatabaseInternal(key.DbIndex);
            return database.KeyExpire(key.FullName, timeSpan);
        }
    }
}