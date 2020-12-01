using System.Collections.Generic;
using System.Linq;
using Quickr.Services;

namespace Quickr.Models.Keys
{
    class ServerEntry: BaseEntry
    {
        List<DatabaseEntry> _databases = new List<DatabaseEntry>();
        List<EndpointEntry> _endpoints = new List<EndpointEntry>();

        public List<DatabaseEntry> Databases
        {
            get => _databases;
            set
            {
                if (Equals(value, _databases)) return;
                _databases = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Children));
            }
        }
        
        public List<EndpointEntry> Endpoints
        {
            get => _endpoints;
            set
            {
                if (Equals(value, _endpoints)) return;
                _endpoints = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Children));
            }
        }

        public List<BaseEntry> Children
        {
            get
            {
                var children = new List<BaseEntry>(Endpoints.Count + Databases.Count);
                children.AddRange(Endpoints);
                children.AddRange(Databases);
                return children;
            }
        }

        public ServerEntry(RedisConnection connection, string name) : base(connection, name)
        {
        }

        public void Refresh()
        {
            Endpoints = Connection.GetEndpoints().ToList();
        }
    }
}