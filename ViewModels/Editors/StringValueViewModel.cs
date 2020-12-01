using StackExchange.Redis;

namespace Quickr.ViewModels.Editors
{
    class StringValueViewModel : BaseViewModel
    {
        string _currentValue;
        string _originalValue;
        
        public string OriginalValue
        {
            get => _originalValue;
            set
            {
                if (_originalValue == value) return;
                _originalValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsUnsaved));
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
                OnPropertyChanged(nameof(IsUnsaved));
            }
        }

        public bool IsUnsaved => CurrentValue != OriginalValue;

        public StringValueViewModel(RedisValue originalValue)
        {
            OriginalValue = originalValue;
            CurrentValue = originalValue;
        }
    }
}