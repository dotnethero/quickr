using Quickr.Services;

namespace Quickr.Models.Keys
{
    internal abstract class TreeEntry: BaseEntry
    {
        public int DbIndex { get; }
        public FolderEntry Parent { get; }

        protected TreeEntry(RedisConnection connection, int dbIndex, string name, FolderEntry parent): base(connection, name)
        {
            DbIndex = dbIndex;
            Parent = parent;
        }
        
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == nameof(IsSelected) && IsSelected)
            {
                ExpandParents();
            }
            base.OnPropertyChanged(propertyName);
        }
        
        private void ExpandParents()
        {
            var parent = Parent;
            while (parent != null)
            {
                parent.IsExpanded = true;
                parent = parent.Parent;
            }
        }
    }
}