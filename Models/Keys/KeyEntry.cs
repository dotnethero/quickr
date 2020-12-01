using System;
using System.Linq;
using System.Threading.Tasks;
using Quickr.Services;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.Models.Keys
{
    class KeyEntry : DbEntry
    {
        string _fullName;
        bool _exists;

        public string FullName
        {
            get => _fullName;
            private set
            {
                if (value == _fullName) return;
                _fullName = value;
                OnPropertyChanged();
            }
        }

        public bool Exists
        {
            get => _exists;
            set
            {
                if (value == _exists) return;
                _exists = value;
                OnPropertyChanged(nameof(Exists));
            }
        }

        public KeyEntry(RedisConnection connection, int dbIndex, string name, string fullname, bool exists, FolderEntry parent): base(connection, dbIndex, name, parent)
        {
            _fullName = fullname;
            _exists = exists;
        }
        
        public async Task<(RedisType, TimeSpan?)> GetProperties()
        {
            var keyspace = GetKeyspace();
            return await keyspace.GetKeyProperties(FullName);
        }
        
        public async Task<string> CloneAsync()
        {
            var keyspace = GetKeyspace();
            var baseName = FullName + "_copy";
            var name = baseName;
            var index = 1;
            while (await keyspace.KeyExists(name))
            {
                name = baseName + (index++);
            }
            await keyspace.CloneKey(FullName, name);
            return name;
        }
        
        public async Task Delete()
        {
            var keyspace = GetKeyspace();
            await keyspace.DeleteKey(FullName);
            Parent.RemoveChild(this);
        }
        
        public async Task MarkAsExpired()
        {
            var keyspace = GetKeyspace();
            await keyspace.SetKeyTimeToLive(FullName, TimeSpan.Zero);
            Parent.RemoveChild(this);
        }

        public async Task SetTimeToLive(TimeSpan? expiry)
        {
            var keyspace = GetKeyspace();
            await keyspace.SetKeyTimeToLive(FullName, expiry);
        }

        public async Task Rename(string fullname)
        {
            var keyspace = GetKeyspace();
            await keyspace.RenameKey(FullName, fullname);

            FullName = fullname;
       
            if (Parent.IsKeyBelongHere(FullName))
            {
                var requiredStart = Parent.IsRoot ? "" : Parent.FullName + Constants.RegionSeparator;
                var name = FullName.Substring(requiredStart.Length).Split(Constants.RegionSeparator).Last();
                Name = name; // just rename
            }
            else
            {
                var root = Parent;
                while (!root.IsRoot) root = root.Parent;
                IsSelected = false;
                Parent.RemoveChild(this);
                var entry = root.AddChild(FullName, Exists); // move
                entry.IsSelected = true;
            }
        }

        public override string ToString()
        {
            return $"Key: {FullName}";
        }
    }
}