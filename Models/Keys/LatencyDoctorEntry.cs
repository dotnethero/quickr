using System.Net;
using Quickr.Services;

namespace Quickr.Models.Keys
{
    class LatencyDoctorEntry : SystemFolderEntry
    {
        public EndPoint Endpoint { get; }

        public LatencyDoctorEntry(RedisConnection connection, string name, EndPoint endpoint) : base(connection, name)
        {
            Endpoint = endpoint;
        }
    }
}