﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using Quickr.Services;

namespace Quickr.ViewModels
{
    internal class ServerInfoViewModel : BaseViewModel
    {
        protected RedisConnection Connection { get; }
        protected EndPoint Endpoint { get; }

        public IGrouping<string, KeyValuePair<string, string>>[] Info { get; }
        
        public ServerInfoViewModel(RedisConnection connection, EndPoint endpoint)
        {
            Connection = connection;
            Endpoint = endpoint;
            Info = Connection.Info(Endpoint).GetAwaiter().GetResult();
        }
    }
}