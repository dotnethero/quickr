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

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public string OriginalValue
        {
            get => _originalValue;
            set
            {
                _originalValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValueChanged));
            }
        }

        public string CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValueChanged));
            }
        }

        public bool ValueChanged => CurrentValue != OriginalValue;

        public ValueViewModel(RedisValue originalValue)
        {
            OriginalValue = originalValue;
            CurrentValue = originalValue;
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
        }

        private void Save()
        {
            OnValueSaved?.Invoke(this, EventArgs.Empty);
        }

        private void Cancel()
        {
            CurrentValue = OriginalValue;
        }

        public event ValueSavedEventHandler OnValueSaved;
    }

    public delegate void ValueSavedEventHandler(object sender, EventArgs e);
}
