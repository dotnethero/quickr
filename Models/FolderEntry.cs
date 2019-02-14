using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace Quickr.Models
{
    internal class FolderEntry : TreeEntry
    {
        private readonly List<KeyEntry> _keys = new List<KeyEntry>();
        private readonly List<FolderEntry> _subfolders = new List<FolderEntry>();

        public string FullName { get; }
        public string SearchPattern => IsRoot ? "*" : FullName + "." + "*";
        public bool IsRoot => Parent == null;

        public List<TreeEntry> Children => _subfolders
            .OrderBy(x => x.Name)
            .OfType<TreeEntry>()
            .Concat(_keys.OrderBy(x => x.Name))
            .ToList();

        public FolderEntry(int dbIndex, string name, string fullname, FolderEntry parent): base(dbIndex, name, parent)
        {
            FullName = fullname;
        }

        public void UpdateChildren(IEnumerable<RedisKey> keys)
        {
            _keys.Clear();
            _subfolders.Clear();
            foreach (var key in keys.OrderBy(x => (string) x))
            {
                Add(key);
            }
            OnPropertyChanged(nameof(Children));
        }

        private void Add(string fullname)
        {
            var requiredStart = IsRoot ? "" : FullName + ".";
            if (!fullname.StartsWith(requiredStart)) throw new InvalidOperationException();

            var parts = fullname.Substring(requiredStart.Length).Split('.').ToList();
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
            var existing = Children.OfType<FolderEntry>().FirstOrDefault(x => x.Name == name);
            if (existing != null) return existing;
            var folder = new FolderEntry(DbIndex, name, fullname, this);
            _subfolders.Add(folder);
            return folder;
        }

        public override string ToString()
        {
            return $"Folder: {Name}";
        }
    }
}