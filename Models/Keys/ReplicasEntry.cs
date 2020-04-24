using System.Collections.Generic;
using System.Net;
using Quickr.Services;

namespace Quickr.Models.Keys
{
    internal class SystemFolderEntry : BaseEntry
    {
        public SystemFolderEntry(RedisConnection connection, string name) : base(connection, name)
        {
        }
    }

    internal class InfoEntry : SystemFolderEntry
    {
        public EndPoint Endpoint { get; }

        public InfoEntry(RedisConnection connection, string name, EndPoint endpoint) : base(connection, name)
        {
            Endpoint = endpoint;
        }
    }
    
    internal class ClientsEntry : SystemFolderEntry
    {
        public EndPoint Endpoint { get; }

        public ClientsEntry(RedisConnection connection, string name, EndPoint endpoint) : base(connection, name)
        {
            Endpoint = endpoint;
        }
    }
    
    internal class ReplicasEntry : SystemFolderEntry
    {
        public List<EndpointEntry> Replicas { get; }

        public ReplicasEntry(RedisConnection connection, string name) : base(connection, name)
        {
            Replicas = new List<EndpointEntry>();
        }

        public ReplicasEntry(RedisConnection connection, string name, IEnumerable<EndpointEntry> replicas) : base(connection, name)
        {
            Replicas = new List<EndpointEntry>(replicas);
        }
    }
}