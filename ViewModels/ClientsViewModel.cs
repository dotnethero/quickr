using System.Net;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.ViewModels
{
    internal class ClientsViewModel : BaseViewModel
    {
        protected RedisConnection Connection { get; }
        protected EndPoint Endpoint { get; }

        public ClientInfo[] Clients { get; set; }

        public ClientsViewModel(RedisConnection connection, EndPoint endpoint)
        {
            Connection = connection;
            Endpoint = endpoint;
            Clients = Connection.ClientList(endpoint).GetAwaiter().GetResult();
        }
    }
}