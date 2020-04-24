using Quickr.Services;
using StackExchange.Redis;

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

        // TODO: review what is mandatory:
        
        public DatabaseProxy GetDatabase()
        {
            return Connection.GetDatabase(DbIndex);
        }
        
        public IDatabase GetDatabaseInternal()
        {
            return Connection.GetDatabaseInternal(DbIndex);
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