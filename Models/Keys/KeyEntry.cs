using System.Linq;
using System.Runtime.CompilerServices;
using Quickr.Services;
using Quickr.Utils;

namespace Quickr.Models.Keys
{
    internal class KeyEntry : DbEntry
    {
        private string _fullName;

        public string FullName
        {
            get => _fullName;
            set
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

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(FullName))
            {
                RenameKey();
            }
            base.OnPropertyChanged(propertyName);
        }

        private void RenameKey()
        {
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