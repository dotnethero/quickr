using Quickr.Services;

namespace Quickr.Models.Keys
{
    internal abstract class DbEntry: BaseEntry
    {
        public int DbIndex { get; }
        public FolderEntry Parent { get; }

        protected DbEntry(RedisConnection connection, int dbIndex, string name, FolderEntry parent): base(connection, name)
        {
            DbIndex = dbIndex;
            Parent = parent;
        }

        public DatabaseProxy GetDatabase()
        {
            return Connection.GetDatabase(DbIndex);
        }
        
        public KeyspaceProxy GetKeyspace()
        {
            return Connection.GetKeyspace(DbIndex);
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