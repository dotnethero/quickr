using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Quickr.Services
{
    internal class KeyspaceProxy
    {
        private readonly IDatabase _db;
        private readonly RedisConnection _connection;

        public KeyspaceProxy(IDatabase db, RedisConnection connection)
        {
            _db = db;
            _connection = connection;
        }
        
        public async Task<long> GetSize()
        {
            var tasks = _connection
                .GetMasterServers()
                .Select(server => server.DatabaseSizeAsync(_db.Database))
                .ToArray();

            var sizes = await Task.WhenAll(tasks);
            return sizes.Sum();
        }

        public async Task<List<RedisKey>> GetKeys(RedisValue pattern = default)
        {
            var servers = GetKeysByServer(pattern);
            var all = new List<RedisKey>();
            foreach (var server in servers)
            {
                await foreach (var key in server)
                {
                    all.Add(key);
                }
            }
            return all;
        }
        
        public async Task DeleteKeys(RedisValue pattern = default)
        {
            var keys = await GetKeys(pattern);
            await _db.KeyDeleteAsync(keys.ToArray());
        }

        public async Task Flush()
        {
            var tasks = _connection
                .GetMasterServers()
                .Select(server => server.FlushDatabaseAsync(_db.Database))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        private IEnumerable<ConfiguredCancelableAsyncEnumerable<RedisKey>> GetKeysByServer(RedisValue pattern, int pageSize = 2500) // TODO: make configurable
        {
            return _connection
                .GetMasterServers()
                .Select(server => server.KeysAsync(_db.Database, pattern, pageSize).ConfigureAwait(false));
        }

        public async Task<bool> KeyExists(string fullname)
        {
            return await _db.KeyExistsAsync(fullname).ConfigureAwait(false);
        }

        public async Task<(RedisType, TimeSpan?)> GetKeyProperties(string fullname)
        {
            var batch = _db.CreateBatch();
            var type = batch.KeyTypeAsync(fullname).ConfigureAwait(false);
            var ttl = batch.KeyTimeToLiveAsync(fullname).ConfigureAwait(false);
            batch.Execute();
            return (await type, await ttl);
        }
        
        public async Task CloneKey(string fullname, string newName)
        {
            var batch = _db.CreateBatch();
            var data = batch.KeyDumpAsync(fullname).ConfigureAwait(false);
            var ttl = batch.KeyTimeToLiveAsync(fullname).ConfigureAwait(false);
            batch.Execute();
            await _db.KeyRestoreAsync(newName, await data, await ttl);
        }
        
        public async Task RenameKey(string fullname, string newName)
        {
            await _db.KeyRenameAsync(fullname, newName);
        }

        public async Task DeleteKey(string fullname)
        {
            await _db.KeyDeleteAsync(fullname);
        }
        
        public async Task SetKeyTimeToLive(string fullname, TimeSpan? expiry)
        {
            await _db.KeyExpireAsync(fullname, expiry);
        }

        public async Task SetKeyTimeToLive(List<string> keys, TimeSpan? expiry)
        {
            var tran = _db.CreateTransaction();
            keys.ForEach(key => tran.KeyExpireAsync(key, expiry).ConfigureAwait(false));
            await tran.ExecuteAsync().ConfigureAwait(false);
        }
    }
}