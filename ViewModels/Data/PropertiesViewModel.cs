using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Quickr.Models.Keys;
using Quickr.Services;

namespace Quickr.ViewModels.Data
{
    internal class PropertiesViewModel: INotifyPropertyChanged
    {
        private string _name;
        private TimeSpan? _expiration;

        public PropertiesViewModel(RedisProxy proxy, KeyEntry key)
        {
            OriginalName = key.FullName;
            OriginalExpiration = proxy.GetTimeToLive(key);

            Name = OriginalName;
            Expiration = OriginalExpiration;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PropertiesChanged));
            }
        }

        public TimeSpan? Expiration
        {
            get => _expiration;
            set
            {
                _expiration = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PropertiesChanged));
            }
        }

        public string OriginalName { get; }
        public TimeSpan? OriginalExpiration { get; }

        public bool PropertiesChanged =>
            OriginalName != Name ||
            OriginalExpiration != Expiration;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}