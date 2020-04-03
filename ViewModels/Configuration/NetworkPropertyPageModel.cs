﻿using System.Collections.Generic;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class NetworkPropertyPageModel: BasePropertyPageModel
    {
        public StringPropertyModel Bind { get; set; }
        public StringPropertyModel Port { get; set; }
        public YesNoPropertyModel ProtectedMode { get; set; }
        public StringPropertyModel TcpBacklog { get; set; }
        public StringPropertyModel Unixsocket { get; set; }
        public StringPropertyModel Unixsocketperm { get; set; }
        public StringPropertyModel Timeout { get; set; }
        public StringPropertyModel TcpKeepalive { get; set; }

        public NetworkPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config): 
            base(connection, config)
        {
            Bind = MapToString("bind");
            Port = MapToString("port");
            ProtectedMode = MapToYesNo("protected-mode");
            TcpBacklog = MapToString("tcp-backlog");
            Unixsocket = MapToString("unixsocket");
            Unixsocketperm = MapToString("unixsocketperm");
            Timeout = MapToString("timeout");
            TcpKeepalive= MapToString("tcp-keepalive");
        }
    }
}
