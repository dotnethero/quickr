using System.ComponentModel;
using System.Runtime.CompilerServices;
using Quickr.Annotations;
using Quickr.Services;

namespace Quickr.Models.Keys
{
    internal abstract class BaseEntry : INotifyPropertyChanged
    {
        public RedisConnection Connection { get; }

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

        protected BaseEntry(RedisConnection connection, string name)
        {
            Name = name;
            Connection = connection;
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