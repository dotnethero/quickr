using System.Net;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.ViewModels.Server
{
    internal class SlowlogViewModel : BaseViewModel
    {
        protected RedisConnection Connection { get; }
        protected EndPoint Endpoint { get; }

        public CommandTrace[] Log { get; set; }

        public SlowlogViewModel(RedisConnection connection, EndPoint endpoint)
        {
            Connection = connection;
            Endpoint = endpoint;
            Log = Connection.SlowlogGet(endpoint).GetAwaiter().GetResult();
        }
    }
}