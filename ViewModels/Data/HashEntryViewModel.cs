using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class HashEntryViewModel : BaseViewModel
    {
        private string _name;
        private string _currentValue;
        private string _originalValue;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        
        public string OriginalValue
        {
            get => _originalValue;
            set
            {
                if (_originalValue == value) return;
                _originalValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValueSaved));
                OnPropertyChanged(nameof(IsNew));
            }
        }

        public string CurrentValue
        {
            get => _currentValue;
            set
            {
                if (_currentValue == value) return;
                _currentValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValueSaved));
            }
        }

        public bool IsValueSaved => OriginalValue == CurrentValue;
        public bool IsNew => OriginalValue == null;

        public static HashEntryViewModel FromHashEntry(HashEntry entry) => 
            new HashEntryViewModel
            {
                Name = entry.Name, 
                CurrentValue = entry.Value,
                OriginalValue = entry.Value
            };
    }
}