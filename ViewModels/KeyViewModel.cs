using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Quickr.Annotations;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.ViewModels
{
    internal class KeyViewModel: INotifyPropertyChanged
    {
        private HashEntry? _hash;
        private HashEntry[] _dataSet;

        public string Name { get; set; }
        public string Expiration { get; set; }
        public string Value { get; set; }

        public HashEntry[] DataSet
        {
            get => _dataSet;
            set
            {
                _dataSet = value;
                // OnPropertyChanged();
                Hash = value?.FirstOrDefault(x => x.Name == "value");
            }
        }

        public HashEntry? Hash
        {
            get => _hash;
            set
            {
                _hash = value;
                Value = value?.Value.PrettifyJson();
                OnPropertyChanged();
                OnPropertyChanged(nameof(Value));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
