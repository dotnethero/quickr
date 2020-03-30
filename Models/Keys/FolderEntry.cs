using System;
using System.Collections.Generic;
using System.Linq;
using Quickr.Services;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.Models.Keys
{
    internal class FolderEntry : TreeEntry
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

        public List<TreeEntry> Children
        {
            get
            {
                var children = new List<TreeEntry>(_subfolders.Count + _keys.Count);
                _subfolders.Sort(Compare);
                _keys.Sort(Compare);
                children.AddRange(_subfolders);
                children.AddRange(_keys);
                return children;
            }
        }

        public FolderEntry(RedisProxy proxy, int dbIndex, string name, string fullname, FolderEntry parent): base(proxy, dbIndex, name, parent)
        {
            _fullName = fullname;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == nameof(SearchPattern))
            {
                Refresh();
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

        public void Refresh()
        {
            var keys = Proxy.GetKeys(DbIndex, SearchPattern);
            UpdateChildren(keys);
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
                var key = new KeyEntry(Proxy, DbIndex, name, fullname, this);
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
            var folder = new FolderEntry(Proxy, DbIndex, name, fullname, this);
            _subfolders.Add(folder);
            return folder;
        }

        private static int Compare(TreeEntry a, TreeEntry b)
        {
            return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return $"Folder: {Name}";
        }
    }
}