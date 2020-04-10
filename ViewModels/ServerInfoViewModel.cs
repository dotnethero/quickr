using System.Collections.Generic;
using System.Linq;
using Quickr.Models.Keys;
using Quickr.Services;

namespace Quickr.ViewModels
{
    internal class ServerInfoViewModel : BaseViewModel
    {
        protected EndpointEntry Entry { get; }
        protected RedisConnection Connection { get; }

        public IGrouping<string, KeyValuePair<string, string>>[] Info { get; }
        
        public ServerInfoViewModel(RedisConnection connection, EndpointEntry entry)
        {
            Entry = entry;
            Connection = connection;
            Info = connection.Info(entry.Endpoint);
        }
    }
}