using System.Linq;
using Quickr.Models;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.Services
{
    internal class RedisProxy
    {
        private IConnectionMultiplexer _connection;

        public DatabaseEntry[] GetDatabases()
        {
            var count = GetConnection()
                .GetServer()
                .ConfigGet("databases")
                .FirstOrDefault()
                .Value
                .ToInt32();

            return Enumerable
                .Range(0, count)
                .Select(x => new DatabaseEntry(x, GetKeys(x)))
                .ToArray();
        }

        public RedisType GetType(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.KeyType(key.FullName);
        }

        public HashEntry[] GetHashes(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.HashGetAll(key.FullName);
        }

        public RedisValue? GetString(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.StringGet(key.FullName);
        }

        public RedisKey[] GetKeys(int dbIndex)
        {
            return GetConnection()
                .GetServer()
                .Keys(dbIndex)
                .ToArray();
        }

        private IDatabase GetDatabase(int dbIndex)
        {
            return GetConnection().GetDatabase(dbIndex);
        }

        private IConnectionMultiplexer GetConnection()
        {
            return _connection ?? (_connection = RedisMultiplexer.Connect());
        }
    }
}
