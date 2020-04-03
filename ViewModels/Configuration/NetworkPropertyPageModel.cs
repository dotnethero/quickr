using System.Collections.Generic;
using System.Linq;
using Quickr.Models.Configuration;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class NetworkPropertyPageModel
    {
        private readonly Dictionary<string, IPropertyModel> _mapping;
        private readonly Dictionary<string, ConfigKeyValue> _config;
        private readonly RedisConnection _connection;

        public StringPropertyModel Bind { get; set; }
        public StringPropertyModel Port { get; set; }
        public YesNoPropertyModel ProtectedMode { get; set; }
        public StringPropertyModel TcpBacklog { get; set; }
        public StringPropertyModel Unixsocket { get; set; }
        public StringPropertyModel Unixsocketperm { get; set; }
        public StringPropertyModel Timeout { get; set; }
        public StringPropertyModel TcpKeepalive { get; set; }

        public NetworkPropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config)
        {
            _connection = connection;
            _config = config;
            _mapping = new Dictionary<string, IPropertyModel>();

            Bind = MapToString("bind");
            Port = MapToString("port");
            ProtectedMode = MapToYesNo("protected-mode");
            TcpBacklog = MapToString("tcp-backlog");
            Unixsocket = MapToString("unixsocket");
            Unixsocketperm = MapToString("unixsocketperm");
            Timeout = MapToString("timeout");
            TcpKeepalive= MapToString("tcp-keepalive");
        }

        private StringPropertyModel MapToString(string key)
        {
            var model = new StringPropertyModel(_config[key]);
            _mapping.Add(key, model);
            return model;
        }
        
        private YesNoPropertyModel MapToYesNo(string key)
        {
            var model = new YesNoPropertyModel(_config[key]);
            _mapping.Add(key, model);
            return model;
        }

        public void Save()
        {
            foreach (var (key, value) in _mapping.Where(x => x.Value.IsPropertyChanged))
            {
                _connection.ConfigSet(key, value.Serialize());
            }
        }
    }
}
