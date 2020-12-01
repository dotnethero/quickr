using System.Net;
using Quickr.Services;

namespace Quickr.Models.Keys
{
    class MemoryDoctorEntry : SystemFolderEntry
    {
        public EndPoint Endpoint { get; }

        public MemoryDoctorEntry(RedisConnection connection, string name, EndPoint endpoint) : base(connection, name)
        {
            Endpoint = endpoint;
        }
    }
}