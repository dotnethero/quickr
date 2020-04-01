using System.Linq;
using System.Net;
using Quickr.Models;
using Quickr.Models.Keys;
using StackExchange.Redis;

namespace Quickr.Services
{
    internal class RedisMultiplexer
    {
        private const string ClientName = "Quickr";

        public ServerEntry Connect(EndPointModel endpoint)
        {
            var endPoint = new DnsEndPoint(endpoint.Host, endpoint.Port ?? 6379);
            var options = new ConfigurationOptions
            {
                AllowAdmin = true,
                ClientName = ClientName,
                Password = endpoint.Password,
                Ssl = endpoint.UseSsl,
                EndPoints =
                {
                    endPoint
                }
            };

            var multiplexer = ConnectionMultiplexer.Connect(options);
            var connection = new RedisConnection(multiplexer);
            return new ServerEntry
            {
                Name = endpoint.Name,
                Connection = connection,
                Databases = connection.GetDatabases().ToList(),
                IsExpanded = true
            };
        }
    }
}