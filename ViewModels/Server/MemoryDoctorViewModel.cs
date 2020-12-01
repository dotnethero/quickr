using System.Net;
using Quickr.Services;

namespace Quickr.ViewModels.Server
{
    class MemoryDoctorViewModel : BaseViewModel
    {
        protected RedisConnection Connection { get; }
        protected EndPoint Endpoint { get; }

        public string Message { get; set; }

        public MemoryDoctorViewModel(RedisConnection connection, EndPoint endpoint)
        {
            Connection = connection;
            Endpoint = endpoint;
            Message = Connection.MemoryDoctor(endpoint).GetAwaiter().GetResult();
        }
    }
}