using StackExchange.Redis;

namespace Quickr.ViewModels.Editors
{
    internal class ValueViewModel : BaseEditorViewModel
    {
        private string _currentValue;
        private string _originalValue;
        
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

        public ValueViewModel(RedisValue originalValue)
        {
            OriginalValue = originalValue;
            CurrentValue = originalValue;
        }
        
        public ValueViewModel(RedisValue originalValue, RedisValue currentValue)
        {
            _originalValue = originalValue;
            _currentValue = currentValue;
        }
    }
}
