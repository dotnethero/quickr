using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Quickr.Models;
using Quickr.Models.Keys;
using StackExchange.Redis;

namespace Quickr.Services
{
    internal class RedisProxy
    {
        private IConnectionMultiplexer _connection;
        private EndPoint _endPoint;

        public DatabaseEntry[] GetDatabases()
        {
            var server = GetServer();
            var count = server.DatabaseCount != 0 ? server.DatabaseCount : 1;
            return Enumerable
                .Range(0, count )
                .Select(x => new DatabaseEntry(this, x))
                .ToArray();
        }

        public async Task<(RedisType, TimeSpan?)> GetTypeTimeToLiveAsync(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            var batch = db.CreateBatch();
            var type = batch.KeyTypeAsync(key.FullName).ConfigureAwait(false);
            var ttl = batch.KeyTimeToLiveAsync(key.FullName).ConfigureAwait(false);
            batch.Execute();
            return (await type, await ttl);
        }

        public TimeSpan? GetTimeToLive(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.KeyTimeToLive(key.FullName);
        }
        
        public bool SetTimeToLive(KeyEntry key, TimeSpan? timeSpan)
        {
            var db = GetDatabase(key.DbIndex);
            return db.KeyExpire(key.FullName, timeSpan);
        }
        
        public bool RenameKey(KeyEntry key, string name)
        {
            var db = GetDatabase(key.DbIndex);
            return db.KeyRename(key.FullName, name);
        }
        
        public string CloneKey(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            var baseName = key.FullName + "_copy";
            var name = baseName;
            var index = 1;
            while (db.KeyExists(name))
            {
                name = baseName + (index++);
            }
            var data = db.KeyDump(key.FullName);
            var ttl = db.KeyTimeToLive(key.FullName);
            db.KeyRestore(name, data, ttl);
            return name;
        }

        public HashEntry[] GetHashes(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.HashGetAll(key.FullName);
        }
        
        public bool HashSet(KeyEntry key, RedisValue hashField, RedisValue value)
        {
            var db = GetDatabase(key.DbIndex);
            return db.HashSet(key.FullName, hashField, value);
        }
        
        public long HashDelete(KeyEntry key, RedisValue[] hashFields)
        {
            var db = GetDatabase(key.DbIndex);
            return db.HashDelete(key.FullName, hashFields);
        }
        
        public RedisValue[] GetList(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.ListRange(key.FullName);
        }
        
        public void ListSet(KeyEntry key, int index, RedisValue value)
        {
            var db = GetDatabase(key.DbIndex);
            db.ListSetByIndex(key.FullName, index, value);
        }

        public void ListRightPush(KeyEntry key, RedisValue value)
        {
            var db = GetDatabase(key.DbIndex);
            db.ListRightPush(key.FullName, value);
        }

        public void ListDelete(KeyEntry key, int[] indexes)
        {
            var name =  "\u0001#removed";
            var db = GetDatabase(key.DbIndex);
            foreach (var index in indexes)
            {
                db.ListSetByIndex(key.FullName, index, name);
            }
            db.ListRemove(key.FullName, name, 0);
        }

        public RedisValue[] GetUnsortedSet(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.SetMembers(key.FullName);
        }
        
        public void UnsortedSetAdd(KeyEntry key, RedisValue value)
        {
            var db = GetDatabase(key.DbIndex);
            db.SetAdd(key.FullName, value);
        }
        
        public void UnsortedSetRemove(KeyEntry key, RedisValue[] values)
        {
            var db = GetDatabase(key.DbIndex);
            db.SetRemove(key.FullName, values);
        }

        public SortedSetEntry[] GetSortedSet(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.SortedSetRangeByRankWithScores(key.FullName);
        }
        
        public void SortedSetAdd(KeyEntry key, RedisValue value, double score)
        {
            var db = GetDatabase(key.DbIndex);
            db.SortedSetAdd(key.FullName, value, score);
        }
        
        public void SortedSetRemove(KeyEntry key, RedisValue[] values)
        {
            var db = GetDatabase(key.DbIndex);
            db.SortedSetRemove(key.FullName, values);
        }

        public RedisValue GetString(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.StringGet(key.FullName);
        }

        public bool SetString(KeyEntry key, string value)
        {
            var db = GetDatabase(key.DbIndex);
            return db.StringSet(key.FullName, value);
        }

        public bool Delete(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.KeyDelete(key.FullName);
        }

        public long Delete(FolderEntry folder)
        {
            var db = GetDatabase(folder.DbIndex);
            var keys = GetKeys(folder.DbIndex, folder.SearchPattern);
            return db.KeyDelete(keys);
        }

        public long GetSize(DatabaseEntry database)
        {
            return GetServer().DatabaseSize(database.DbIndex);
        }

        public void Flush(DatabaseEntry database)
        {
            GetServer().FlushDatabase(database.DbIndex);
        }

        public RedisKey[] GetKeys(int dbIndex, RedisValue pattern = default)
        {
            return GetServer()
                .Keys(dbIndex, pattern, 50000)
                .ToArray();
        }

        public void ChangeConnection(EndPointModel model)
        {
            _connection?.Dispose();
            _connection = null;

            var endPoint = new DnsEndPoint(model.Server, model.Port ?? 6379);
            var options = new ConfigurationOptions
            {
                AllowAdmin = true,
                ClientName = "Quickr",
                Password = model.Password,
                Ssl = model.UseSsl,
                EndPoints =
                {
                    endPoint
                }
            };

            _endPoint = endPoint;
            _connection = ConnectionMultiplexer.Connect(options);
        }

        private IDatabase GetDatabase(int dbIndex)
        {
            return GetConnection().GetDatabase(dbIndex);
        }

        private IServer GetServer()
        {
            return GetConnection().GetServer(_connection.GetEndPoints().First());
        }

        private IConnectionMultiplexer GetConnection()
        {
            return _connection ?? throw new InvalidOperationException();
        }
    }
}
