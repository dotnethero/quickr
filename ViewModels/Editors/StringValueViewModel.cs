using StackExchange.Redis;

namespace Quickr.ViewModels.Editors
{
    class StringValueViewModel : BaseEditorViewModel
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
                OnPropertyChanged(nameof(IsValueChanged));
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
                OnPropertyChanged(nameof(IsValueChanged));
            }
        }

        public bool IsValueChanged => CurrentValue != OriginalValue;

        public StringValueViewModel(RedisValue originalValue)
        {
            OriginalValue = originalValue;
            CurrentValue = originalValue;
        }
        
        public StringValueViewModel(RedisValue originalValue, RedisValue currentValue)
        {
            _originalValue = originalValue;
            _currentValue = currentValue;
        }
    }
}