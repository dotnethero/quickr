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

        public async Task<RedisValue> GetStringAsync(KeyEntry key)
        {
            return await _db.StringGetAsync(key.FullName).ConfigureAwait(false);
        }

        public bool SetString(KeyEntry key, string value)
        {
            return _db.StringSet(key.FullName, value);
        }

        // List operations

        public async Task<RedisValue[]> GetListAsync(KeyEntry key)
        {
            return await _db.ListRangeAsync(key.FullName);
        }
        
        public void ListSet(KeyEntry key, int index, RedisValue value)
        {
            _db.ListSetByIndex(key.FullName, index, value);
        }

        public void ListRightPush(KeyEntry key, RedisValue value)
        {
            _db.ListRightPush(key.FullName, value);
        }

        public async Task ListDelete(KeyEntry key, List<int> indexes) // TODO: test
        {
            const string name = "\u0001#removed";

            var tran = _db.CreateTransaction();
            indexes.ForEach(index => tran.ListSetByIndexAsync(key.FullName, index, name).ConfigureAwait(false));
            var result = tran.ListRemoveAsync(key.FullName, name).ConfigureAwait(false);
            await tran.ExecuteAsync().ConfigureAwait(false);
            await result;
        }

        // Set operations

        public async Task<RedisValue[]> GetUnsortedSetAsync(KeyEntry key)
        {
            return await _db.SetMembersAsync(key.FullName).ConfigureAwait(false);
        }
        
        public void UnsortedSetAdd(KeyEntry key, RedisValue value)
        {
            _db.SetAdd(key.FullName, value);
        }
        
        public void UnsortedSetRemove(KeyEntry key, RedisValue[] values)
        {
            _db.SetRemove(key.FullName, values);
        }

        // SortedSet operations

        public async Task<SortedSetEntry[]> GetSortedSetAsync(KeyEntry key)
        {
            return await _db.SortedSetRangeByRankWithScoresAsync(key.FullName).ConfigureAwait(false);
        }
        
        public void SortedSetAdd(KeyEntry key, RedisValue value, double score)
        {
            _db.SortedSetAdd(key.FullName, value, score);
        }
        
        public void SortedSetRemove(KeyEntry key, RedisValue[] values)
        {
            _db.SortedSetRemove(key.FullName, values);
        }

        // Hash operations
        
        public async Task<HashEntry[]> GetHashesAsync(KeyEntry key)
        {
            return await _db.HashGetAllAsync(key.FullName);
        }
        
        public bool HashSet(KeyEntry key, RedisValue hashField, RedisValue value)
        {
            return _db.HashSet(key.FullName, hashField, value);
        }
        
        public long HashDelete(KeyEntry key, RedisValue[] hashFields)
        {
            return _db.HashDelete(key.FullName, hashFields);
        }
    }
}
