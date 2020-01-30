using System;
using System.Collections.Generic;
using System.Linq;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.Models.Keys
{
    internal class FolderEntry : TreeEntry
    {
        private readonly List<KeyEntry> _keys = new List<KeyEntry>();
        private readonly List<FolderEntry> _subfolders = new List<FolderEntry>();

        public string FullName { get; }
        public string SearchPattern => IsRoot ? "*" : FullName + Constants.RegionSeparator + "*";
        public bool IsRoot => Parent == null;

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

        public FolderEntry(int dbIndex, string name, string fullname, FolderEntry parent): base(dbIndex, name, parent)
        {
            FullName = fullname;
        }

        public void UpdateChildren(IEnumerable<RedisKey> keys)
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

        private void Add(string fullname)
        {
            var requiredStart = IsRoot ? "" : FullName + Constants.RegionSeparator;
            if (!fullname.StartsWith(requiredStart)) throw new InvalidOperationException();

            var parts = fullname.Substring(requiredStart.Length).Split(Constants.RegionSeparator).ToList();
            if (parts.Count == 1)
            {
                var name = string.IsNullOrWhiteSpace(parts[0]) ? "(none)" : parts[0];
                var key = new KeyEntry(DbIndex, name, fullname, this);
                _keys.Add(key);
            }
            else
            {
                var name = parts[0];
                var fullFolderName = requiredStart + name;
                var folder = CreateFolder(name, fullFolderName);
                folder.Add(fullname);
            }
        }

        private FolderEntry CreateFolder(string name, string fullname)
        {
            var existing = _subfolders.Find(x => x.FullName == fullname);
            if (existing != null) return existing;
            var folder = new FolderEntry(DbIndex, name, fullname, this);
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