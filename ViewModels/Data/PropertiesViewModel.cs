using System;
using System.Threading.Tasks;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Data
{
    class PropertiesViewModel : BaseViewModel
    {
        string _name;
        TimeSpan? _expiration;
        TimeSpan? _originalExpiration;

        public KeyEntry Key { get; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsUnsaved));
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
                OnPropertyChanged(nameof(IsUnsaved));
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
                OnPropertyChanged(nameof(IsUnsaved));
            }
        }

        public bool IsUnsaved =>
            Key.FullName != Name ||
            OriginalExpiration != Expiration;

        public PropertiesViewModel(KeyEntry key, TimeSpan? expiration)
        {
            Key = key;
            Name = key.FullName;
            Expiration = OriginalExpiration = expiration;
        }

        public async Task<bool> Save()
        {
            if (Expiration < TimeSpan.Zero) 
                Expiration = null;

            if (Expiration != OriginalExpiration)
            {
                await Key.SetTimeToLive(Expiration);
                OriginalExpiration = Expiration;
            }

            if (Name != Key.FullName)
            {
                await Key.Rename(Name);
            }

            return true;
        }
    }
}