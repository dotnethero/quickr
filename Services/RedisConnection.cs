using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Quickr.Models.Configuration;
using Quickr.Models.Keys;
using StackExchange.Redis;

namespace Quickr.Services
{
    internal class RedisConnection: IDisposable
    {
        private readonly ConnectionMultiplexer _connection;

        public RedisConnection(ConnectionMultiplexer connection)
        {
            _connection = connection;
        }
        
        public IGrouping<string, KeyValuePair<string, string>>[] Info()
        {
            var server = GetOriginServer();
            return server.Info();
        }
        
        // section -> key -> key * value * spec
        public Dictionary<string, Dictionary<string, ConfigKeyValue>> ConfigGet()
        {
            var server = GetOriginServer();
            var configs = server.ConfigGet().ToDictionary();
            var redisConfSpec = File.ReadAllText("redis.conf.json");
            var sections = JsonConvert.DeserializeObject<ConfigSection[]>(redisConfSpec);
            var query =
                from section in sections
                from spec in section.Configs
                let loaded = configs.ContainsKey(spec.Key)
                select new
                {
                    Section = section.Section,
                    KeyValue = new ConfigKeyValue
                    {
                        Key = spec.Key,
                        DefaultValue = spec.DefaultValue,
                        PossibleValues = spec.PossibleValues,
                        Description = spec.Description,
                        Value = loaded ? configs[spec.Key] : null,
                        IsReadOnly = spec.IsReadOnly,
                        IsLoaded = loaded
                    }
                };

            var specs = sections
                .SelectMany(x => x.Configs)
                .Select(x => x.Key)
                .ToHashSet();

            var other =
                from config in configs
                where !specs.Contains(config.Key)
                select new
                {
                    Section = "Other",
                    KeyValue = new ConfigKeyValue
                    {
                        Key = config.Key,
                        Value = config.Value,
                        IsLoaded = true
                    },
                };

            return query
                .Concat(other)
                .GroupBy(x => x.Section, x => x.KeyValue)
                .ToDictionary(x => x.Key, x => x.ToDictionary(v => v.Key, v => v));
        }
        
        public void ConfigSet(string key, string value)
        {
            var db = GetOriginServer();
            db.ConfigSet(key, value);
        }

        public DatabaseEntry[] GetDatabases()
        {
            var server = GetOriginServer();
            var count = server.ServerType == ServerType.Cluster || server.DatabaseCount == 0 ? 1 : server.DatabaseCount;
            return Enumerable
                .Range(0, count)
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

        public async Task<HashEntry[]> GetHashesAsync(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return await db.HashGetAllAsync(key.FullName);
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
        
        public async Task<RedisValue[]> GetListAsync(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return await db.ListRangeAsync(key.FullName);
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

        public async Task<RedisValue[]> GetUnsortedSetAsync(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return await db.SetMembersAsync(key.FullName).ConfigureAwait(false);
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

        public async Task<SortedSetEntry[]> GetSortedSetAsync(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return await db.SortedSetRangeByRankWithScoresAsync(key.FullName).ConfigureAwait(false);
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

        public async Task<RedisValue> GetStringAsync(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return await db.StringGetAsync(key.FullName).ConfigureAwait(false);
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
            return GetMasterServers()
                .Select(server => server.DatabaseSize(database.DbIndex))
                .Sum();
        }

        public void Flush(DatabaseEntry database)
        {
            GetMasterServers()
                .ForEach(server => server.FlushDatabase(database.DbIndex));
        }

        public RedisKey[] GetKeys(int dbIndex, RedisValue pattern = default)
        {
            return GetMasterServers()
                .SelectMany(server => server.Keys(dbIndex, pattern, 2500))
                .ToArray();
        }

        private IDatabase GetDatabase(int dbIndex)
        {
            return _connection.GetDatabase(dbIndex);
        }

        private IServer GetOriginServer()
        {
            return _connection.GetServer(_connection.GetEndPoints().First());
        }
        
        private List<IServer> GetMasterServers()
        {
            var server = GetOriginServer();
            if (server.ServerType == ServerType.Standalone)
            {
                return new List<IServer> { server };
            }
            return server
                .ClusterConfiguration
                .Nodes
                .Where(x => !x.IsSlave)
                .Select(x => _connection.GetServer(x.EndPoint))
                .ToList();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
