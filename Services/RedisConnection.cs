using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        private readonly DnsEndPoint _originEndpoint;

        public RedisConnection(ConnectionMultiplexer connection, DnsEndPoint originEndpoint)
        {
            _connection = connection;
            _originEndpoint = originEndpoint;
        }
        
        public async Task<IGrouping<string, KeyValuePair<string, string>>[]> Info(EndPoint endpoint)
        {
            var server = _connection.GetServer(endpoint);
            return await server.InfoAsync().ConfigureAwait(false);
        }

        public async Task<ClientInfo[]> ClientList(EndPoint endpoint)
        {
            var server = _connection.GetServer(endpoint);
            return await server.ClientListAsync().ConfigureAwait(false);
        }

        public async Task<CommandTrace[]> SlowlogGet(EndPoint endpoint)
        {
            var server = _connection.GetServer(endpoint);
            return await server.SlowlogGetAsync().ConfigureAwait(false);
        }
        
        public async Task<string> MemoryDoctor(EndPoint endpoint)
        {
            var server = _connection.GetServer(endpoint);
            return await server.MemoryDoctorAsync().ConfigureAwait(false);
        }
        
        public async Task<string> LatencyDoctor(EndPoint endpoint)
        {
            var server = _connection.GetServer(endpoint);
            return await server.LatencyDoctorAsync().ConfigureAwait(false);
        }

        // section -> key -> key * value * spec
        public Dictionary<string, Dictionary<string, ConfigKeyValue>> ConfigGet(EndPoint endpoint)
        {
            var server = _connection.GetServer(endpoint);
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
                        IsLoaded = true // unsure if this option is editable
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
        
        public void ConfigSet(EndPoint endpoint, string key, string value)
        {
            var server = _connection.GetServer(endpoint);
            server.ConfigSet(key, value);
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

        private IServer GetOriginServer()
        {
            return _connection.GetServer(_originEndpoint);
        }

        public List<IServer> GetMasterServers()
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
        
        public EndpointEntry[] GetEndpoints()
        {
            var server = GetOriginServer();
            if (server.ServerType == ServerType.Standalone)
            {
                return new[] { new EndpointEntry(this, server) };
            }

            return server.ClusterConfiguration.Nodes
                .Where(x => !x.IsSlave && !x.EndPoint.Equals(_originEndpoint))
                .OrderBy(node => node.EndPoint.AddressFamily)
                .ThenBy(node => node.EndPoint.ToString())
                .Select(node => new EndpointEntry(this, node))
                .ToArray();
        }
        
        public IDatabase GetDatabaseInternal(int dbIndex)
        {
            return _connection.GetDatabase(dbIndex);
        }

        public DatabaseProxy GetDatabase(int dbIndex)
        {
            var db = _connection.GetDatabase(dbIndex);
            return new DatabaseProxy(db);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
