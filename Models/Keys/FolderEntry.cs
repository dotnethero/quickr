using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quickr.Services;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.Models.Keys
{
    internal class FolderEntry : DbEntry
    {
        private readonly List<KeyEntry> _keys = new List<KeyEntry>();
        private readonly List<FolderEntry> _subfolders = new List<FolderEntry>();
        private string _filter = "*";
        private string _fullName;

        public string FullName
        {
            get => _fullName;
            set
            {
                if (value == _fullName) return;
                _fullName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SearchPattern));
            }
        }

        public string Filter
        {
            get => _filter;
            set
            {
                if (value == _filter) return;
                _filter = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SearchPattern));
            }
        }

        public string SearchPattern => IsRoot ? Filter : FullName + Constants.RegionSeparator + Filter;

        public bool IsRoot => Parent == null;

        public bool IsFilterApplied => Filter != "" && Filter != "*";

        public List<DbEntry> Children
        {
            get
            {
                var children = new List<DbEntry>(_subfolders.Count + _keys.Count);
                _subfolders.Sort(Compare);
                _keys.Sort(Compare);
                children.AddRange(_subfolders);
                children.AddRange(_keys);
                return children;
            }
        }

        public FolderEntry(RedisConnection connection, int dbIndex, string name, string fullname, FolderEntry parent): base(connection, dbIndex, name, parent)
        {
            _fullName = fullname;
        }
        
        public virtual async Task MarkAsExpiredAsync()
        {
            await MarkAllKeysAsExpiredAsync();
            Parent.RemoveChild(this);
        }

        protected async Task MarkAllKeysAsExpiredAsync()
        {
            var keyspace = GetKeyspace();
            var keys = GetKeys().Select(key => key.FullName).ToList();
            await keyspace.SetKeyTimeToLive(keys, TimeSpan.Zero);
        }

        public async Task Delete()
        {
            var keyspace = GetKeyspace();
            await keyspace.DeleteKeys(SearchPattern);
            Parent.RemoveChild(this);
        }
        
        public async Task Refresh()
        {
            var keyspace = GetKeyspace();
            var keys = await keyspace.GetKeys(SearchPattern);
            UpdateChildren(keys);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == nameof(SearchPattern))
            {
                Refresh(); // TODO: make event subscription
            }
            base.OnPropertyChanged(propertyName);
        }

        public IEnumerable<KeyEntry> GetKeys()
        {
            return _subfolders
                .SelectMany(f => f.GetKeys())
                .Concat(_keys);
        }

        public bool IsKeyBelongHere(string fullname)
        {
            var requiredStart = IsRoot ? "" : FullName + Constants.RegionSeparator;
            if (!fullname.StartsWith(requiredStart)) return false;
            var parts = fullname.Substring(requiredStart.Length).Split(Constants.RegionSeparator).ToList();
            return parts.Count == 1;
        }

        private void UpdateChildren(IEnumerable<RedisKey> keys)
        {
            _keys.Clear();
            _subfolders.Clear();
            foreach (var key in keys)
            {
                Add(key);
            }
            OnPropertyChanged(nameof(Children));
        }

        public void RemoveChildren()
        {
            _keys.Clear();
            _subfolders.Clear();
            OnPropertyChanged(nameof(Children));
        }

        public void RemoveChild(KeyEntry key)
        {
            _keys.Remove(key);
            OnPropertyChanged(nameof(Children));
        }

        public void RemoveChild(FolderEntry folder)
        {
            _subfolders.Remove(folder);
            OnPropertyChanged(nameof(Children));
        }

        public KeyEntry AddChild(string fullname)
        {
            var entry = Add(fullname);
            OnPropertyChanged(nameof(Children));
            return entry;
        }

        private KeyEntry Add(string fullname)
        {
            var requiredStart = IsRoot ? "" : FullName + Constants.RegionSeparator;
            if (!fullname.StartsWith(requiredStart)) throw new InvalidOperationException();

            var parts = fullname.Substring(requiredStart.Length).Split(Constants.RegionSeparator).ToList();
            if (parts.Count == 1)
            {
                var name = string.IsNullOrWhiteSpace(parts[0]) ? "(none)" : parts[0];
                var key = new KeyEntry(Connection, DbIndex, name, fullname, this);
                _keys.Add(key);
                return key;
            }
            else
            {
                var name = parts[0];
                var fullFolderName = requiredStart + name;
                var folder = CreateFolder(name, fullFolderName);
                return folder.Add(fullname);
            }
        }

        private FolderEntry CreateFolder(string name, string fullname)
        {
            var existing = _subfolders.Find(x => x.FullName == fullname);
            if (existing != null) return existing;
            var folder = new FolderEntry(Connection, DbIndex, name, fullname, this);
            _subfolders.Add(folder);
            return folder;
        }

        private static int Compare(DbEntry a, DbEntry b)
        {
            return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return $"Folder: {Name}";
        }
    }
}