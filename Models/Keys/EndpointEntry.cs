using System.Collections.Generic;
using System.Linq;
using System.Net;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.Models.Keys
{
    internal class EndpointEntry : BaseEntry
    {
        public EndPoint Endpoint { get; }
        public List<EndpointEntry> Replicas { get; }
        public bool IsReplica { get; }

        public EndpointEntry(RedisConnection connection, IServer server) : base(connection, server.EndPoint.ToString())
        {
            IsReplica = false;
            Endpoint = server.EndPoint;
            Replicas = new List<EndpointEntry>();
        }

        public EndpointEntry(RedisConnection connection, ClusterNode node) : base(connection, node.EndPoint.ToString())
        {
            IsReplica = node.IsSlave;
            Endpoint = node.EndPoint;
            Replicas = node
                .Children
                .Select(replica => new EndpointEntry(connection, replica))
                .ToList();
        }
    }
}