using System.Collections.Generic;
using System.Threading.Tasks;
using Quickr.Models.Keys;
using StackExchange.Redis;

namespace Quickr.Services
{
    internal class DatabaseProxy
    {
        private readonly IDatabase _db;

        public DatabaseProxy(IDatabase db)
        {
            _db = db;
        }

        // String operations

        public async Task<RedisValue> GetString(KeyEntry key)
        {
            return await _db.StringGetAsync(key.FullName);
        }

        public async Task<bool> SetString(KeyEntry key, string value)
        {
            return await _db.StringSetAsync(key.FullName, value);
        }

        // List operations

        public async Task<RedisValue[]> GetListAsync(KeyEntry key)
        {
            return await _db.ListRangeAsync(key.FullName);
        }
        
        public async Task ListSet(KeyEntry key, long index, RedisValue value)
        {
            await _db.ListSetByIndexAsync(key.FullName, index, value);
        }
        
        public async Task<long> ListRightPush(KeyEntry key, RedisValue value)
        {
            return await _db.ListRightPushAsync(key.FullName, value);
        }
        
        public async Task<long> ListSave(KeyEntry key, Dictionary<long, RedisValue> existingItems, RedisValue[] newItems)
        {
            var tran = _db.CreateTransaction();
            foreach (var item in existingItems)
            {
                var _ = tran.ListSetByIndexAsync(key.FullName, item.Key, item.Value);
            }

            var addResult = tran.ListRightPushAsync(key.FullName, newItems);
            await tran.ExecuteAsync();
            return await addResult + existingItems.Count;
        }

        public async Task<long> ListDelete(KeyEntry key, List<long> indexes)
        {
            const string name = "\u0001#removed";

            var tran = _db.CreateTransaction();
            indexes.ForEach(index => tran.ListSetByIndexAsync(key.FullName, index, name));
            var result = tran.ListRemoveAsync(key.FullName, name);
            await tran.ExecuteAsync();
            return await result;
        }

        // Set operations

        public async Task<RedisValue[]> GetUnsortedSetAsync(KeyEntry key)
        {
            return await _db.SetMembersAsync(key.FullName);
        }
        
        public async Task UnsortedSetAdd(KeyEntry key, params RedisValue[] values)
        {
            await _db.SetAddAsync(key.FullName, values);
        }
        
        public async Task UnsortedSetRemove(KeyEntry key, params RedisValue[] values)
        {
            await _db.SetRemoveAsync(key.FullName, values);
        }
        
        public async Task<bool> UnsortedSetUpdate(KeyEntry key, RedisValue originValue, RedisValue newValue)
        {
            var tran = _db.CreateTransaction();
            var removeResult = tran.SetRemoveAsync(key.FullName, originValue);
            var addResult = tran.SetAddAsync(key.FullName, newValue);
            await tran.ExecuteAsync();
            return await removeResult && await addResult;
        }

        public async Task<long> UnsortedSetSave(KeyEntry key, RedisValue[] removed, RedisValue[] added)
        {
            var tran = _db.CreateTransaction();
            var removeResult = removed.Length > 0 
                ? tran.SetRemoveAsync(key.FullName, removed)
                : Task.FromResult(0L);

            var addResult = added.Length > 0
                ? tran.SetAddAsync(key.FullName, added)
                : Task.FromResult(0L);

            await tran.ExecuteAsync();
            return await removeResult + await addResult;
        }

        // SortedSet operations

        public async Task<SortedSetEntry[]> GetSortedSetAsync(KeyEntry key)
        {
            return await _db.SortedSetRangeByRankWithScoresAsync(key.FullName);
        }
        
        public async Task SortedSetAdd(KeyEntry key, RedisValue value, double score)
        {
            await _db.SortedSetAddAsync(key.FullName, value, score);
        }
        
        public async Task SortedSetRemove(KeyEntry key, params RedisValue[] values)
        {
            await _db.SortedSetRemoveAsync(key.FullName, values);
        }
        
        public async Task<bool> SortedSetUpdate(KeyEntry key, RedisValue originValue, RedisValue newValue, double newScore)
        {
            var tran = _db.CreateTransaction();
            var removeResult = tran.SortedSetRemoveAsync(key.FullName, originValue);
            var addResult = tran.SortedSetAddAsync(key.FullName, newValue, newScore);
            await tran.ExecuteAsync();
            return await removeResult && await addResult;
        }
        
        public async Task<long> SortedSetSave(KeyEntry key, RedisValue[] removed, SortedSetEntry[] added)
        {
            var tran = _db.CreateTransaction();
           
            var removeResult = removed.Length > 0 
                ? tran.SortedSetRemoveAsync(key.FullName, removed)
                : Task.FromResult(0L);

            var addResult = added.Length > 0
                ? tran.SortedSetAddAsync(key.FullName, added)
                : Task.FromResult(0L);

            await tran.ExecuteAsync();
            return await removeResult + await addResult;
        }

        // Hash operations
        
        public async Task<HashEntry[]> GetHashesAsync(KeyEntry key)
        {
            return await _db.HashGetAllAsync(key.FullName);
        }
        
        public async Task<bool> HashSet(KeyEntry key, RedisValue hashField, RedisValue value)
        {
            return await _db.HashSetAsync(key.FullName, hashField, value);
        }
        
        public async Task HashSet(KeyEntry key, HashEntry[] hashFields)
        {
            await _db.HashSetAsync(key.FullName, hashFields);
        }

        public async Task<long> HashDelete(KeyEntry key, params RedisValue[] hashFields)
        {
            return await _db.HashDeleteAsync(key.FullName, hashFields);
        }
    }
}
