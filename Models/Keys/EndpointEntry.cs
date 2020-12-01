using System.Collections.Generic;
using System.Linq;
using System.Net;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.Models.Keys
{
    class EndpointEntry : BaseEntry
    {
        public EndPoint Endpoint { get; }
        public List<SystemFolderEntry> Entries { get; }
        public bool IsReplica { get; }
        public bool IsConnected { get; }

        public EndpointEntry(RedisConnection connection, IServer server) : base(connection, server.EndPoint.ToString())
        {
            IsReplica = false;
            IsConnected = true;
            Endpoint = server.EndPoint;
            Entries = CreateSystemFolders(connection, server.EndPoint, replicas: new List<EndpointEntry>());
        }

        public EndpointEntry(RedisConnection connection, ClusterNode node) : base(connection, node.EndPoint.ToString())
        {
            var replicas = node.Children.Select(replica => new EndpointEntry(connection, replica));
            IsReplica = node.IsSlave;
            IsConnected = node.IsConnected;
            Endpoint = node.EndPoint;
            Entries = CreateSystemFolders(connection, node.EndPoint, replicas);
        }

        static List<SystemFolderEntry> CreateSystemFolders(RedisConnection connection, EndPoint endpoint, IEnumerable<EndpointEntry> replicas)
        {
            return new List<SystemFolderEntry>
            {
                new InfoEntry(connection, "Info", endpoint),
                new ReplicasEntry(connection, "Replicas", replicas),
                new ClientsEntry(connection, "Clients", endpoint),
                new SlowlogEntry(connection, "Slow log", endpoint),
                new MemoryDoctorEntry(connection, "Memory doctor", endpoint),
                new LatencyDoctorEntry(connection, "Latency doctor", endpoint),
                new SystemFolderEntry(connection, "Monitor"),
            };
        }
    }
}