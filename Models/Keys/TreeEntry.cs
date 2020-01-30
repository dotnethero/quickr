using System.ComponentModel;
using System.Runtime.CompilerServices;
using Quickr.Annotations;

namespace Quickr.Models.Keys
{
    internal abstract class TreeEntry: INotifyPropertyChanged
    {
        public string Name { get; }
        public int DbIndex { get; }
        public FolderEntry Parent { get; }

        protected TreeEntry(int dbIndex, string name, FolderEntry parent)
        {
            DbIndex = dbIndex;
            Name = name;
            Parent = parent;
        }

        public override string ToString()
        {
            return Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}