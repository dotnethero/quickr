using System.Collections.Generic;
using Quickr.Services;

namespace Quickr.Models.Keys
{
    internal class ServerEntry: BaseEntry
    {
        private List<DatabaseEntry> _databases = new List<DatabaseEntry>();

        public List<DatabaseEntry> Databases
        {
            get => _databases;
            set
            {
                if (Equals(value, _databases)) return;
                _databases = value;
                OnPropertyChanged();
            }
        }

        public ServerEntry(RedisConnection connection, string name) : base(connection, name)
        {
        }
    }
}