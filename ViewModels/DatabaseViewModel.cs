using System.ComponentModel;
using System.Runtime.CompilerServices;
using Quickr.Annotations;

namespace Quickr.ViewModels
{
    internal class DatabaseViewModel : INotifyPropertyChanged
    {
        public long KeyCount { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
