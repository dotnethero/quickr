using System;
using System.Windows.Input;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.Utils;

namespace Quickr.ViewModels.Data
{
    internal class PropertiesViewModel: BaseViewModel
    {
        protected KeyEntry Key { get; }
        protected RedisProxy Proxy { get; }

        private string _name;
        private string _originalName;
        private TimeSpan? _expiration;
        private TimeSpan? _originalExpiration;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        
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

        public string OriginalName
        {
            get => _originalName;
            set
            {
                _originalName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PropertiesChanged));
            }
        }

        public TimeSpan? OriginalExpiration
        {
            get => _originalExpiration;
            set
            {
                _originalExpiration = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PropertiesChanged));
            }
        }

        public bool PropertiesChanged => OriginalName != Name || OriginalExpiration != Expiration;

        public PropertiesViewModel(RedisProxy proxy, KeyEntry key)
        {
            Proxy = proxy;
            Key = key;

            OriginalName = Key.FullName;
            OriginalExpiration = Proxy.GetTimeToLive(Key);

            Name = OriginalName;
            Expiration = OriginalExpiration;

            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
        }

        private void Save()
        {
            if (Expiration != OriginalExpiration)
            {
                Proxy.SetTimeToLive(Key, Expiration);
                OriginalExpiration = Expiration;
            }
            if (Name != OriginalName)
            {
                Proxy.RenameKey(Key, Name);
                OriginalName = Name;
                Key.FullName = Name;
            }
        }
        
        private void Cancel()
        {
            Name = OriginalName;
            Expiration = OriginalExpiration;
        }
    }
}