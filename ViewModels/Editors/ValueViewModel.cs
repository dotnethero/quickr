using System;

namespace Quickr.ViewModels.Editors
{
    internal class ValueViewModel<T> : BaseEditorViewModel where T: IEquatable<T>
    {
        private T _currentValue;
        private T _originalValue;
        
        public T OriginalValue
        {
            get => _originalValue;
            set
            {
                if (_originalValue.Equals(value)) return;
                _originalValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValueChanged));
            }
        }

        public T CurrentValue
        {
            get => _currentValue;
            set
            {
                if (_currentValue.Equals(value)) return;
                _currentValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValueChanged));
            }
        }

        public bool IsValueChanged => !CurrentValue.Equals(OriginalValue);

        public ValueViewModel(T originalValue)
        {
            OriginalValue = originalValue;
            CurrentValue = originalValue;
        }
        
        public ValueViewModel(T originalValue, T currentValue)
        {
            _originalValue = originalValue;
            _currentValue = currentValue;
        }

        public override string ToString() => CurrentValue.ToString();
    }
}
