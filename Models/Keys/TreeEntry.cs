using System.ComponentModel;
using System.Runtime.CompilerServices;
using Quickr.Annotations;
using Quickr.Services;

namespace Quickr.Models.Keys
{
    internal abstract class TreeEntry: INotifyPropertyChanged
    {
        protected RedisProxy Proxy { get; }

        private string _name;
        private bool _isSelected;
        private bool _isExpanded;

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

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                var parent = Parent;
                while (parent != null)
                {
                    parent.IsExpanded = true;
                    parent = parent.Parent;
                }
                OnPropertyChanged();
            }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (value == _isExpanded) return;
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        public int DbIndex { get; }
        public FolderEntry Parent { get; }

        protected TreeEntry(RedisProxy proxy, int dbIndex, string name, FolderEntry parent)
        {
            Proxy = proxy;
            DbIndex = dbIndex;
            Parent = parent;
            _name = name;
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