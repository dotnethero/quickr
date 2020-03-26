using System;
using System.Windows.Input;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class ValueViewModel : BaseViewModel
    {
        private string _currentValue;
        private string _originalValue;
        
        public event EventHandler ValueSaved;
        public event EventHandler ValueDiscarded;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

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

        private ValueViewModel()
        {
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
        }

        public ValueViewModel(RedisValue originalValue): this()
        {
            OriginalValue = originalValue;
            CurrentValue = originalValue;
        }
        
        public ValueViewModel(RedisValue originalValue, RedisValue currentValue): this()
        {
            _originalValue = originalValue;
            _currentValue = currentValue;
        }

        private void Save()
        {
            OriginalValue = CurrentValue;
            ValueSaved?.Invoke(this, EventArgs.Empty);
        }

        private void Cancel()
        {
            CurrentValue = OriginalValue;
            ValueDiscarded?.Invoke(this, EventArgs.Empty);
        }
    }
}
