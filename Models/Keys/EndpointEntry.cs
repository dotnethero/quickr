using System.Net;
using Quickr.Services;

namespace Quickr.Models.Keys
{
    internal class EndpointEntry : BaseEntry
    {
        public EndPoint Endpoint { get; }

        public EndpointEntry(RedisConnection connection, EndPoint endpoint, string name) : base(connection, name)
        {
            Endpoint = endpoint;
        }
    }
}