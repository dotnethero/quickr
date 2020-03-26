using System.ComponentModel;
using System.Runtime.CompilerServices;
using Quickr.Annotations;

namespace Quickr.Models.Keys
{
    internal abstract class TreeEntry: INotifyPropertyChanged
    {
        private string _name;

        public string Name
        {
            get => _name;
            protected set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

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