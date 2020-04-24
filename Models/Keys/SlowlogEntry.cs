using System.Net;
using Quickr.Services;

namespace Quickr.Models.Keys
{
    internal class SlowlogEntry : SystemFolderEntry
    {
        public EndPoint Endpoint { get; }

        public SlowlogEntry(RedisConnection connection, string name, EndPoint endpoint) : base(connection, name)
        {
            Endpoint = endpoint;
        }
    }
}