using System.Net;
using Quickr.Services;

namespace Quickr.ViewModels.Server
{
    class LatencyDoctorViewModel : BaseViewModel
    {
        protected RedisConnection Connection { get; }
        protected EndPoint Endpoint { get; }

        public string Message { get; set; }

        public LatencyDoctorViewModel(RedisConnection connection, EndPoint endpoint)
        {
            Connection = connection;
            Endpoint = endpoint;
            Message = Connection.LatencyDoctor(endpoint).GetAwaiter().GetResult();
        }
    }
}