using System;
using Quickr.Models.Keys;
using Quickr.Services;

namespace Quickr.ViewModels.Data
{
    internal class PropertiesViewModel: BaseEditorViewModel
    {
        private string _name;
        private string _originalName;
        private TimeSpan? _expiration;
        private TimeSpan? _originalExpiration;

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
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
                if (_expiration == value) return;
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
                if (_originalName == value) return;
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
                if (_originalExpiration == value) return;
                _originalExpiration = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PropertiesChanged));
            }
        }

        public bool PropertiesChanged => OriginalName != Name || OriginalExpiration != Expiration;

        public PropertiesViewModel(string name, TimeSpan? expiration)
        {
            OriginalName = name;
            OriginalExpiration = expiration;

            Name = name;
            Expiration = expiration;
        }
    }
}