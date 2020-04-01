using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Quickr.Annotations;
using Quickr.Services;

namespace Quickr.Models.Keys
{
    internal class ServerEntry: INotifyPropertyChanged
    {
        private List<DatabaseEntry> _databases = new List<DatabaseEntry>();

        public RedisConnection Connection { get; set; }
        public string Name { get; set; }
        public bool IsExpanded { get; set; }

        public List<DatabaseEntry> Databases
        {
            get => _databases;
            set
            {
                if (Equals(value, _databases)) return;
                _databases = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}