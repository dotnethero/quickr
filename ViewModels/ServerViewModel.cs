using System.Collections.Generic;
using System.Linq;
using Quickr.Models.Keys;
using Quickr.Services;

namespace Quickr.ViewModels
{
    internal class ServerViewModel : BaseViewModel
    {
        protected ServerEntry Entry { get; }
        protected RedisConnection Connection { get; }

        public IGrouping<string, KeyValuePair<string, string>>[] Info { get; }
        public IGrouping<string, KeyValuePair<string, string>>[] Config { get; }
        
        public ServerViewModel(RedisConnection connection, ServerEntry entry)
        {
            Entry = entry;
            Connection = connection;
            Info = connection.Info();
            Config = connection.Config();
        }
    }
}