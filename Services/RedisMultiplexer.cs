﻿using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Quickr.Models;
using Quickr.Models.Keys;
using StackExchange.Redis;

namespace Quickr.Services
{
    internal class RedisMultiplexer
    {
        private const string ClientName = "Quickr";

        public async Task<ServerEntry> ConnectAsync(EndpointModel endpoint)
        {
            var originEndpoint = new DnsEndPoint(endpoint.Host, endpoint.Port ?? 6379);
            var options = new ConfigurationOptions
            {
                AllowAdmin = true,
                ClientName = ClientName,
                Password = endpoint.Password,
                Ssl = endpoint.UseSsl,
                EndPoints =
                {
                    originEndpoint
                }
            };

            var multiplexer = await ConnectionMultiplexer.ConnectAsync(options).ConfigureAwait(false);
            var connection = new RedisConnection(multiplexer, originEndpoint);
            return new ServerEntry(connection, endpoint.Name)
            {
                Databases = connection.GetDatabases().ToList(),
                Endpoints = connection.GetEndpoints().ToList(),
                IsExpanded = true
            };
        }
    }
}