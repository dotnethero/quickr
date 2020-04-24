using System;
using System.Linq;
using System.Threading.Tasks;
using Quickr.Services;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.Models.Keys
{
    internal class KeyEntry : DbEntry
    {
        private string _fullName;

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

        public KeyEntry(RedisConnection connection, int dbIndex, string name, string fullname, FolderEntry parent): base(connection, dbIndex, name, parent)
        {
            _fullName = fullname;
        }
        
        public async Task<(RedisType, TimeSpan?)> GetProperties()
        {
            var database = GetDatabaseInternal();
            var batch = database.CreateBatch();
            var type = batch.KeyTypeAsync(FullName).ConfigureAwait(false);
            var ttl = batch.KeyTimeToLiveAsync(FullName).ConfigureAwait(false);
            batch.Execute();
            return (await type, await ttl);
        }
        
        public async Task<string> CloneAsync()
        {
            var database = GetDatabaseInternal();
            var baseName = FullName + "_copy";
            var name = baseName;
            var index = 1;
            while (database.KeyExists(name))
            {
                name = baseName + (index++);
            }
            var batch = database.CreateBatch();
            var data = batch.KeyDumpAsync(FullName).ConfigureAwait(false);
            var ttl = batch.KeyTimeToLiveAsync(FullName).ConfigureAwait(false);
            batch.Execute();

            await database.KeyRestoreAsync(name, await data, await ttl);
            return name;
        }
        
        public void Delete()
        {
            var database = GetDatabaseInternal();
            database.KeyDelete(FullName);
            Parent.RemoveChild(this);
        }

        public void SetTimeToLive(TimeSpan? timeSpan)
        {
            var database = GetDatabaseInternal();
            database.KeyExpire(FullName, timeSpan);
        }
        
        public void Rename(string fullname)
        {
            var database = GetDatabaseInternal();
            database.KeyRename(FullName, fullname);

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
                Parent.RemoveChild(this);
                var entry = root.AddChild(FullName); // move
                entry.IsSelected = true;
            }
        }

        public override string ToString()
        {
            return $"Key: {FullName}";
        }
    }
}