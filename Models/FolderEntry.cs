using System;
using System.Collections.Generic;
using System.Linq;

namespace Quickr.Models
{
    internal class FolderEntry : TreeEntry
    {
        public List<KeyEntry> Keys { get; } = new List<KeyEntry>();
        public List<FolderEntry> Subfolders { get; } = new List<FolderEntry>();
        public List<TreeEntry> Children => Subfolders.OrderBy(x => x.Name).OfType<TreeEntry>().Concat(Keys.OrderBy(x => x.Name)).ToList();

        public FolderEntry(int dbIndex, string name): base(dbIndex, name)
        {
        }

        public void Add(string fullname)
        {
            var parts = fullname
                .Split(new[] { "." }, StringSplitOptions.None)
                .Select(x => string.IsNullOrWhiteSpace(x) ? "(none)" : x);

            var stack = new Queue<string>(parts);
            Add(fullname, stack);
        }

        private void Add(string fullname, Queue<string> stack)
        {
            if (stack.Count == 1)
            {
                var name = stack.Dequeue();
                var key = new KeyEntry(DbIndex, name, fullname);
                Keys.Add(key);
            }
            else
            {
                var name = stack.Dequeue();
                var folder = CreateFolder(name);
                folder.Add(fullname, stack);
            }
        }

        private FolderEntry CreateFolder(string name)
        {
            var existing = Children.OfType<FolderEntry>().FirstOrDefault(x => x.Name == name);
            if (existing != null) return existing;
            var folder = new FolderEntry(DbIndex, name);
            Subfolders.Add(folder);
            return folder;
        }

        public override string ToString()
        {
            return $"Folder: {Name}";
        }
    }
}