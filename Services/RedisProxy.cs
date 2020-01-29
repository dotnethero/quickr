using System;
using System.Linq;
using System.Net;
using Quickr.Models;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.Services
{
    internal class RedisProxy
    {
        private IConnectionMultiplexer _connection;
        private EndPoint _endPoint;

        public DatabaseEntry[] GetDatabases()
        {
            var count = GetServer()
                .ConfigGet("databases")
                .FirstOrDefault()
                .Value
                .ToInt32();

            return Enumerable
                .Range(0, count)
                .Select(x => new DatabaseEntry(x))
                .ToArray();
        }

        public KeyData GetKeyData(KeyEntry key)
        {
            var model = new KeyData
            {
                Entry = key, 
                Type = GetType(key)
            };
            switch (model.Type)
            {
                case RedisType.Hash:
                    model.Data = GetHashes(key);
                    break;

                case RedisType.List:
                    model.Data = GetList(key);
                    break;

                case RedisType.Set:
                    model.Data = GetUnsortedSet(key);
                    break;

                case RedisType.SortedSet:
                    model.Data = GetSortedSet(key);
                    break;

                case RedisType.String:
                    model.Data = GetString(key);
                    break;
            }
            return model;
        }

        public RedisType GetType(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.KeyType(key.FullName);
        }

        public TimeSpan? GetTimeToLive(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.KeyTimeToLive(key.FullName);
        }

        public HashEntry[] GetHashes(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.HashGetAll(key.FullName);
        }

        public RedisValue[] GetList(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.ListRange(key.FullName);
        }

        public RedisValue[] GetUnsortedSet(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.SetMembers(key.FullName);
        }

        public SortedSetEntry[] GetSortedSet(KeyEntry key)
        {
            var db = GetDatabase(key.DbIndex);
            return db.SortedSetRangeByRankWithScores(key.FullName);
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

            var endPoint = new DnsEndPoint(model.Server, model.Port);
            var options = new ConfigurationOptions
            {
                AllowAdmin = true,
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
