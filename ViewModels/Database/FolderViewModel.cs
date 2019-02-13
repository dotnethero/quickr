using System;
using System.Collections.Generic;
using System.Linq;

namespace Quickr.ViewModels.Database
{
    internal class FolderViewModel : EntryViewModel
    {
        public List<KeyViewModel> Keys { get; } = new List<KeyViewModel>();
        public List<FolderViewModel> Subfolders { get; } = new List<FolderViewModel>();
        public List<EntryViewModel> Children => Subfolders.OrderBy(x => x.Name).OfType<EntryViewModel>().Concat(Keys.OrderBy(x => x.Name)).ToList();

        public FolderViewModel(string name): base(name)
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
                var key = new KeyViewModel(name, fullname);
                Keys.Add(key);
            }
            else
            {
                var name = stack.Dequeue();
                var folder = CreateFolder(name);
                folder.Add(fullname, stack);
            }
        }

        private FolderViewModel CreateFolder(string name)
        {
            var existing = Children.OfType<FolderViewModel>().FirstOrDefault(x => x.Name == name);
            if (existing != null) return existing;
            var folder = new FolderViewModel(name);
            Subfolders.Add(folder);
            return folder;
        }

        public override string ToString()
        {
            return $"Folder: {Name}";
        }
    }
}